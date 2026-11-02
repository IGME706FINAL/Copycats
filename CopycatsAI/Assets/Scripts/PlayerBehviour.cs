using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehviour : MonoBehaviour
{
    public float speed;
    public PlayerInput PlayerInput;
    private Vector3 moveInput;
    public InputAction Move;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(rb.position + moveInput * speed * Time.deltaTime);
    }

    

    void OnMove(InputValue value)
    {
        Vector2 valueVec = value.Get<Vector2>();
        moveInput = new Vector3(valueVec.x, 0, valueVec.y);
    }
}
