using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public float minHeight = 2f;
    public float maxHeight = 5f;
    public float speed = 2f;

    private float startHeight;

    void Start()
    {
        startHeight = transform.position.y;
    }

    void Update()
    {
        float range = maxHeight - minHeight;
        float newY = minHeight + Mathf.PingPong(Time.time * speed, range);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
