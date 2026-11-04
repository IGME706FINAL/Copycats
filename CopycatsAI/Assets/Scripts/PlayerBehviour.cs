using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehviour : MonoBehaviour
{
    //speed
    public float speed;
    //Playerinput action map
    public PlayerInput PlayerInput;
    //Vector for movement
    private Vector3 moveInput;
    public InputAction Move;
    private Rigidbody rb;
    //Copycats
    GameObject[] cats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector3(0, 0, 0);
        //move based on input
        rb.MovePosition(rb.position + moveInput * speed * Time.deltaTime);
    }

    
    //input manager script
    void OnMove(InputValue value)
    {
       
        Vector2 valueVec = value.Get<Vector2>();
        moveInput = new Vector3(valueVec.x, 0, valueVec.y);
    }
}
