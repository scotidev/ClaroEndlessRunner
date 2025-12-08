using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float jumpHeight;
    private float jumpVelocity;
    public float gravity;
    public float horizontalSpeed;
    private bool isMovingLeft;
    private bool isMovingRight;

    [Header("Raycast")]
    public float rayRadius;
    public LayerMask layer;

    private CharacterController controller;

    private PlayerHurt hurt;
    private PlayerManager playerManager;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        hurt = GetComponent<PlayerHurt>();
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
        OnCollision();

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

    void OnCollision()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayRadius, layer) && !hurt.isInvulnerable)
        {
            Debug.Log("colidiu");
            hurt.ActivateInvulnerability();
        }
    }
}
