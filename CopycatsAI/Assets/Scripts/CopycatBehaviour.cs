using UnityEngine;

public class CopycatBehaviour : MonoBehaviour
{
    public enum State 
    { 
        Idle,
        Follow,
        Carry,
        Distracted,
        Breaking,
        Dead
    };
    public float distractPercent;
    public float speed;
    public GameObject Player;
    public float seperation;
    public State catstate;
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
        
        
        if (catstate == State.Follow)
        {
            SeekPlayer();
        }
    }

    void SeekPlayer()
    {
        Vector3 direction = Player.transform.position - gameObject.transform.position;
        rb.linearVelocity = direction;
        rb.linearVelocity.Normalize();
        rb.linearVelocity *= speed;
    }
}
