using UnityEngine;
using UnityEngine.InputSystem;

public class ManagerBehaviour : MonoBehaviour
{

    public InputAction quit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true;
        quit.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (quit.triggered)
        {
            Debug.Log("quit");
        }
        
    }

    
}
