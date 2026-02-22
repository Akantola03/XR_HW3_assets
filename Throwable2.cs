using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Throwable2 : MonoBehaviour
{
    Rigidbody rb;

    // These are set by the grab script
    public bool isHeld = false;
    public Vector3 lastHandPos;
    public Vector3 handVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isHeld)
        {
            // While held, disable physics
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        else
        {
            // When released, physics takes over
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    // Called by CustomGrab when the object is grabbed
    public void OnGrab(Transform hand)
    {
        isHeld = true;
        lastHandPos = hand.position;
    }

    // Called every frame while held
    public void OnHeldUpdate(Transform hand)
    {
        // Compute hand velocity
        handVelocity = (hand.position - lastHandPos) / Time.deltaTime;
        lastHandPos = hand.position;

        // Move object with hand
        transform.position = hand.position;
        transform.rotation = hand.rotation;
    }

    // Called by CustomGrab when released
    public void OnRelease()
    {
        isHeld = false;

        // Apply throw force
        rb.velocity = handVelocity;
    }
}
