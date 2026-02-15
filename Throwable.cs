using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Throwable : MonoBehaviour 
{   
    List<Vector3> trackingPos = new List<Vector3>();
    public float velocity = 1000f;

    bool pickekdUp = false;
    GameObject parentHand;
    Rigidbody rb;

    void start() 
    {

    }

    void Update()
    {
        if (pickedUp == true) 
        {
            rb.useGravity = false;

            transform.position = parentHand.transform.position; //match parent
            transform.rotation = parentHand.transform.rotation;

            if (trackingPos > 15) // delete the oldest tracked position
            {
                trackingPos.RemoveAt(0);
            }
            trackingPos.Add(transform.position) // add the current position to the top of the list 
        
            float triggerRight = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);

            if (triggerRight < 0.1f) // checks if trigger is released
            {
                pickedUp = false;
                Vector3 direction = trackingPos[trackingPos.Count - 1] - trackingPos[0];
                rb.AddForce(direction * velocity);
                rb.useGravity = true;
                rb.isKinematic = false;
                GetComponent<Collider>().isTrigger = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        float triggerRight = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);

        if (other.gameObject.tag == "hand" && trriggerRight > 0.9f) 
        {
            pickekdUp = true;
            parentHand = other.gemaObject;
        }
    }
}

