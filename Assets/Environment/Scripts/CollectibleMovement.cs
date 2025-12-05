using UnityEngine;

public class CollectibleMovement : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 90f;

    [Header("Floating")]
    public float floatAmplitude = 0.5f;
    public float floatSpeed = 1f;
    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        RotateObject();
        FloatObject();
    }

    private void RotateObject()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void FloatObject()
    {
        float newY = startY + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}