using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float speed;
    public float jumpHeight;
    private float jumpVelocity;
    public float gravity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 direction = Vector3.forward * speed;

        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Jump!");
                jumpVelocity = jumpHeight;
            }
        }

        else
        {
            jumpVelocity -= gravity;
        }

        direction.y = jumpVelocity;

        controller.Move(direction * Time.deltaTime);
    }
}
