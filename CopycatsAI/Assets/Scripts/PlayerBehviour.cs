using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

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
    public List<GameObject> cats = new List<GameObject>();

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
    public void OnMove(InputValue value)
    {
       
        Vector2 valueVec = value.Get<Vector2>();
        moveInput = new Vector3(valueVec.x, 0, valueVec.y);
    }
    //ask all cats to follow
    public void CatsFollow()
    {
        foreach (GameObject cat in cats)
        {
            cat.GetComponent<CopycatBehaviour>().StateFollow();
        }
    }
    //ask cats to stay
    public void CatsStay()
    {
        foreach (GameObject cat in cats)
        {
            cat.GetComponent<CopycatBehaviour>().StateIdle();
        }
    }

    public void CatsCarry()
    {
        foreach (GameObject cat in cats)
        {
            cat.GetComponent<CopycatBehaviour>().StateCarry();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("cat")) 
        {
            //cats.Add(other.gameObject);
            Debug.Log("Cat added");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("cat"))
        {
            //cats.Remove(other.gameObject);
            Debug.Log("Cat removed");
        }
    }
}
