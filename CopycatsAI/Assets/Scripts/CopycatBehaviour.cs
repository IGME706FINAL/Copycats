using UnityEngine;

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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // = State.Idle;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
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

    public void StateIdle()
    {
        Debug.Log("Idle called");
        if (hearCommands) 
        { 
            catstate = State.Idle; 
        }
    }

    public void StateFollow()
    {
        Debug.Log("Follow called");
        if (hearCommands)
        {
            catstate = State.Follow;
        }
    }

    public void StateDistracted()
    {
        if (hearCommands)
        {
            if (catstate == State.Carry) Drop();
            catstate = State.Distracted;
        }
    }

    private void Drop()
    { 
    
    }

    public void StateCarry()
    {
        if (hearCommands)
        {
            catstate = State.Carry;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            hearCommands = true;
            Debug.Log(gameObject.name + "Can hear commands");
            other.gameObject.GetComponent<PlayerBehviour>().cats.Add(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            hearCommands = false;
            Debug.Log(gameObject.name + "Cant hear commands");
            other.gameObject.GetComponent<PlayerBehviour>().cats.Remove(gameObject);
        }
    }
}
