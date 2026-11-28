using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class CopycatBehaviour : MonoBehaviour
{
    //states
    public enum State 
    { 
        Idle,
        Follow,
        Carry,
        Distracted,
        Breaking,
        Dead
    };
    //percent to be distracted
    public float distractPercent;
    //affinity to player
    public float affinity;
    //speed
    public float speed;
    //player object
    public GameObject Player;
    //seperation variables
    public float seperationThresh;
    public float decayCoefficent;
    //state of the cat
    public State catstate;
    //rigidbody
    private Rigidbody rb;
    //bool for testing reinforcment
    public bool learningOn;
    //can recieve commands
    public bool hearCommands;
    //time till we run coroutine again
    public float distractCheckTime;
    //check if coroutine is running
    private bool coRuning = false;
    //bool for distraction tests
    public bool distractOn;
    //lowest affinity can go before the cats stop listining
    public float affinityLimit;
    //how much distract and affinity change
    public int disChange;
    public int affChange;
    //variables for carrying
    public GameObject matToCarry;
    //nav mesh variables
    public LayerMask groundMask;
    private NavMeshAgent agent;
    public GameObject testObject;
    //carrying bool
    public bool pathfinding;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        matToCarry = null;
        rb = gameObject.GetComponent<Rigidbody>();
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (affinity > affinityLimit)
        {
            if (catstate != State.Distracted)
            {
                //follow state instructions
                if (catstate == State.Follow)
                {
                    SeekPlayer();
                    Seperate();
                }
                if (catstate == State.Idle)
                {
                    rb.linearVelocity = new Vector3(0, 0, 0);
                }
                if (catstate == State.Carry)
                {
                    if (pathfinding) SeekObject();

                }
                //code to check if cats will become distracted
                if (coRuning == false && distractOn)
                {
                    //stop this coroutine if it is running
                    StopCoroutine(Distract());
                    //if couroutine is not running, start it
                    StartCoroutine(Distract());
                }

            }
            else
            {
                rb.linearVelocity = new Vector3(0, 0, 0);
            }
        }
        else
        {
            Debug.Log("SCREW YOU GUYS I'M GOING HOME!!!");
            Seperate();
        }
    }

    void SeekPlayer()
    {
        //move to player
        Vector3 direction = Player.transform.position - gameObject.transform.position;
        //reset vector
        rb.linearVelocity = new Vector3(0,0,0);
        direction.Normalize();
        direction *= speed;
        rb.linearVelocity = direction - rb.linearVelocity;
        
    }

    void SeekObject()
    {
        if (matToCarry != null)
        {
            Debug.Log("seek object");
            //move to object
            Vector3 direction = matToCarry.transform.position - gameObject.transform.position;
            //reset vector
            rb.linearVelocity = new Vector3(0, 0, 0);
            direction.Normalize();
            direction *= speed;
            rb.linearVelocity = direction - rb.linearVelocity;
        }
        
    }

    void Seperate()
    {
        //seperation code
        Vector3 direction = Player.transform.position - gameObject.transform.position;
        float distance = direction.magnitude;
        if (distance < seperationThresh)
        {
            float strength = Mathf.Min(decayCoefficent / (distance * distance), speed);
            direction.Normalize();
            rb.linearVelocity += strength * -direction;
        }
    }

    //state switches
    public void StateIdle()
    {
        //Debug.Log("Idle called");
        if (hearCommands) 
        {
            if(catstate == State.Carry)Drop();
            catstate = State.Idle; 
        }
    }

    public void StateFollow()
    {
        //Debug.Log("Follow called");
        if (hearCommands)
        {
            if(catstate == State.Carry)Drop();
            catstate = State.Follow;
        }
    }

    public void StateDistracted()
    {
        //drop item if they are carrying one
        if (catstate == State.Carry) Drop();
        catstate = State.Distracted;
        Debug.Log("Distract state triggered");
        
    }

    private void Drop()
    {
        rb.linearVelocity = new Vector3(0, 0, 0);
        stopPathfind();
        if(matToCarry != null)matToCarry.GetComponent<MaterialBehaviour>().RemoveCats(gameObject);
        matToCarry = null;
    }

    public void StateCarry()
    {
        if (hearCommands && matToCarry !=null)
        {
            catstate = State.Carry;
            rb.linearVelocity = new Vector3(0, 0, 0);
            CarryObject();
        }
    }
    //scold and reward
    public void Scolded()
    {
        Debug.Log("Scolded");
        if (hearCommands && learningOn)
        {
            if (catstate == State.Distracted)
            {
                distractPercent -= disChange;
                catstate = State.Follow;
            }
            affinity -= affChange;
        }
           
    }

    public void Rewarded()
    {
        Debug.Log("Rewarded");
        if (hearCommands && learningOn)
        {
            if (catstate != State.Distracted)
            {
                distractPercent -= disChange;
            }
            else 
            {
                distractPercent += disChange;
            }
            affinity += affChange;
        }
    }

    //distracted couroutine
    IEnumerator Distract()
    {
        Debug.Log("Distract coroutine running");
        //coroutine is running
        coRuning = true;
        if (Random.value * 100 <= distractPercent)
        {
            StateDistracted();
            
        }
        yield return new WaitForSeconds(distractCheckTime);
        Debug.Log("Distract coroutine stopping");
        coRuning = false;
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player && other.gameObject.GetComponent<PlayerBehviour>().cats.Contains(gameObject) == false)
        {
            hearCommands = true;
            //Debug.Log(gameObject.name + "Can hear commands");
            other.gameObject.GetComponent<PlayerBehviour>().cats.Add(gameObject);
        }

        if (other.tag == "Material")
        {
            Debug.Log("Mat to Carry added");
            matToCarry = other.gameObject;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            hearCommands = false;
            //Debug.Log(gameObject.name + "Cant hear commands");
            other.gameObject.GetComponent<PlayerBehviour>().cats.Remove(gameObject);
        }
        if (other.gameObject.tag == "Material")
        {
            Debug.Log("Mat to Carry removed");
            if (catstate == State.Carry)
            {
                catstate = State.Idle;
                Drop();
            }
            matToCarry = null;
        }
    }

    private void CarryObject()
    {
        if (matToCarry != null && matToCarry.GetComponent<MaterialBehaviour>().GetCarrying() == false)
        {
            SeekObject();
            matToCarry.GetComponent<MaterialBehaviour>().AddCats(gameObject);
        }
    }

    public void pathfind(GameObject destination)
    {
        Debug.Log("Pathfind called");
        rb.linearVelocity = new Vector3(0, 0, 0);
        //agent.isStopped = false;
        testObject = destination;
        agent.SetDestination(destination.transform.position);
        //Debug.Log("Destination position" + destination.transform.position);
        pathfinding = true;
        
    }
    public void stopPathfind()
    {
        Debug.Log("stopPathfind called");
        agent.isStopped = true;
        pathfinding = false;
    }
}
