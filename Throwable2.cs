using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ThrowableBall : MonoBehaviour
{
    Rigidbody rb;

    // These are set by the grab script
    public bool isHeld = false;
    public bool pendingThrow = false;

    public Vector3 lastHandPos;
    public Vector3 handVelocity;

    Transform currentHand;

    public float throwMultiplier = 1.5f;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
            
    }

    void FixedUpdate()
    {
        if (isHeld)
        {
            Vector3 newPos = currentHand.position; 
            handVelocity = (newPos - lastHandPos) / Time.fixedDeltaTime; 
            lastHandPos = newPos;

            // While held, disable physics
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.MovePosition(newPos); 
            rb.MoveRotation(currentHand.rotation);

        }
        else
        {
            // When released, physics takes over
            rb.isKinematic = false;
            rb.useGravity = true;

            if (pendingThrow) 
            { 
                rb.linearVelocity = handVelocity;
                pendingThrow = false;
            }
        }
    }

    // Called by CustomGrab when the object is grabbed
    public void OnGrab(Transform hand)
    {
        isHeld = true;
        currentHand = hand;
        lastHandPos = hand.position;
    }

    // Called every frame while held
    public void OnHeldUpdate(Transform hand)
    {
        currentHand = hand;
    }

    // Called by CustomGrab when released
    public void OnRelease()
    {
        isHeld = false;

        // Apply throw force
        //rb.linearVelocity = handVelocity;
        pendingThrow = true;
        
    }
}
