using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab4 : MonoBehaviour
{
    CustomGrab otherHand = null;
    public List<Transform> nearObjects = new List<Transform>();
    public Transform grabbedObject = null;
    public InputActionReference action;
    bool grabbing = false;

    Vector3 lastPosition;
    Quaternion lastRotation;

    private void Start()
    {
        action.action.Enable();

        foreach (CustomGrab c in transform.parent.GetComponentsInChildren<CustomGrab>())
        {
            if (c != this)
                otherHand = c;
        }

        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    void Update()
    {
        grabbing = action.action.IsPressed();

        if (grabbing)
        {
            // Grab object if not already holding one
            if (!grabbedObject)
            {
                if (nearObjects.Count > 0)
                {
                    grabbedObject = nearObjects[0];
                }
                else if (otherHand && otherHand.grabbedObject)
                {
                    grabbedObject = otherHand.grabbedObject;
                    otherHand.grabbedObject = null;
                }

                // NEW: Notify Throwable when grabbed
                if (grabbedObject && grabbedObject.TryGetComponent<Throwable>(out Throwable tGrab))
                {
                    tGrab.OnGrab(transform);
                }
            }

            // Move grabbed object
            if (grabbedObject)
            {
                Vector3 deltaPosition = transform.position - lastPosition;
                Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(lastRotation);

                // NEW: If throwable, let it handle movement
                if (grabbedObject.TryGetComponent<Throwable>(out Throwable tHeld))
                {
                    tHeld.OnHeldUpdate(transform);
                }
                else
                {
                    // Original movement for non-throwables
                    grabbedObject.position += deltaPosition;
                    grabbedObject.rotation = deltaRotation * grabbedObject.rotation;
                }
            }
        }
        else if (grabbedObject)
        {
            // NEW: Notify Throwable when released
            if (grabbedObject.TryGetComponent<Throwable>(out Throwable tRelease))
            {
                tRelease.OnRelease();
            }

            grabbedObject = null;
        }

        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform t = other.transform;
        if (t && t.tag.ToLower() == "grabbable")
            nearObjects.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if (t && t.tag.ToLower() == "grabbable")
            nearObjects.Remove(t);
    }
}
