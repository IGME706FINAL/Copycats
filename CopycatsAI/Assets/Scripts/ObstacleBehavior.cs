using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    //materials needed to destory this object
    public int matNeeded;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //destroy if matNeeded = 0
        if (matNeeded <= 0) Destroy(gameObject);
    }

    public void matAdded()
    {
        Debug.Log("MatAdded");
        matNeeded--;
    }
}
