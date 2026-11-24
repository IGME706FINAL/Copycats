using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.AI;
public class MaterialBehaviour : MonoBehaviour
{
    //if max amounts of cats are on this object/if object is carried
    public bool carrying;
    //how many cats are on this object
    public int catsCarying;
    //how many cats are needed to lift this object
    public int weight;
    //where to carry to
    public GameObject toObstacle;
    //center of all carrying cats
    public Vector3 centerPoint;
    //cats carrying
    public List<GameObject> cats;
    private Rigidbody rb;
    //how fast it moves to center of object
    public float followSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cats = new List<GameObject>();
        rb = gameObject.GetComponent<Rigidbody>();
        centerPoint = gameObject.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (carrying == true)
        {
            
            seekCenter();
        }
        if (carrying == false)
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
        }
    }

    public bool GetCarrying()
    {
        return carrying;
    }

    public void AddCats(GameObject cat)
    {
        catsCarying++;
        cats.Add(cat);
        if (catsCarying >= weight) 
        { 
            carrying = true;
            catsSeek();
        }
    }

    public void RemoveCats(GameObject cat)
    {
        catsCarying--;
        cats.Remove(cat);
        if (catsCarying < weight) carrying = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Debug.Log("obstacle on collison");
            collision.gameObject.GetComponent<ObstacleBehavior>().matAdded();
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Debug.Log("obstacle on trigger");
            other.gameObject.GetComponent<ObstacleBehavior>().matAdded();
            foreach (GameObject cat in cats)
            {
                cat.GetComponent<CopycatBehaviour>().stopPathfind();
            }
            Destroy(gameObject);
        }
    }

    private void seekCenter()
    {
        float totalX = 0f;
        float totalZ = 0f;
        foreach (GameObject cat in cats)
        {
            totalX += cat.transform.position.x;
            totalZ += cat.transform.position.z;
        }
        float centerX = totalX / cats.Count;
        float centerZ = totalZ / cats.Count;
        centerPoint = new Vector3(centerX,0,centerZ);
        //move to object
        Vector3 direction = centerPoint - gameObject.transform.position;
        //reset vector
        rb.linearVelocity = new Vector3(0, 0, 0);
        direction.Normalize();
        direction *= followSpeed;
        rb.linearVelocity = direction - rb.linearVelocity;
    }

    private void catsSeek()
    {
        Debug.Log("CatSeek called");
        foreach (GameObject cat in cats)
        {
            cat.GetComponent<CopycatBehaviour>().pathfind(toObstacle);
           
        }
    }
}
