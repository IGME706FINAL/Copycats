using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehviour : MonoBehaviour
{
    public float speed;
    public PlayerInput PlayerInput;
    private Vector2 moveInput;
    public InputAction Move;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
