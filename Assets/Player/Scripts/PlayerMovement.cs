using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float speed;
    public float jumpHeight;
    private float jumpVelocity;
    public float gravity;
    public float horizontalSpeed;
    private bool isMovingLeft;
    private bool isMovingRight;

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
                jumpVelocity = jumpHeight;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x < 1f && !isMovingRight)
            {
                isMovingRight = true;
                StartCoroutine(RightMove());
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > -1f && !isMovingLeft)
            {
                isMovingLeft = true;
                StartCoroutine(LeftMove());
            }
        }

        else
        {
            jumpVelocity -= gravity;
        }

        direction.y = jumpVelocity;

        controller.Move(direction * Time.deltaTime);
    }

    IEnumerator RightMove()
    {
        for (float i = 0; i < 10; i += 0.1f)
        {
            controller.Move(Vector3.right * horizontalSpeed * Time.deltaTime);
            yield return null;
        }

        isMovingRight = false;
    }

    IEnumerator LeftMove()
    {
        for (float i = 0; i < 10; i += 0.1f)
        {
            controller.Move(Vector3.left * horizontalSpeed * Time.deltaTime);
            yield return null;
        }

        isMovingLeft = false;
    }
}
