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
}
