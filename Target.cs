using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Target : MonoBehaviour 
{   
    public float maxHeight = 3.0f;
    public float velocity = 1.0f;
    
    public GameObject particles;

    void start() 
    {
        startHeight = transform.position.y;
        maxHeight = maxHeight + startHeight;
        velocity -= Random.Range(-0.5f, 0.5f);
    }

    void Update()
    {
       Vector3 temp = transform.position;
       temp.y -= velocity * Time.deltaTime;
       if (temp.y < startHeight || temp.y > maxHeight)
       {
            velocity *= -1;
       }
       transfomr.posisition = temp;
    }

    private void OnCollisionEnter(Collision collision) 
    {
        GameObject clone = Instantiate(particles, transform.position, transform.rotation)
        Destroy(clone.gameObject, 2);

        Destroy(gameObject);
        Destroy(collision.gameObject);
    }
}
