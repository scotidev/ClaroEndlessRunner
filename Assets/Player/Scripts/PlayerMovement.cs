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

    [Header("Slow Effect")]
    public float slowDuration = 2f;
    public float slowAmount = 5f;
    private float originalSpeed;

    //[Header("Raycast")]
    //public float rayRadius;
    public LayerMask layer;
    private CharacterController controller;

    private PlayerHurt hurt;
    private PlayerManager playerManager;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        hurt = GetComponent<PlayerHurt>();
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        originalSpeed = speed;
    }

    void Update()
    {
        Vector3 direction = Vector3.forward * speed;

        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpVelocity = jumpHeight;
                animator.SetTrigger("Jump");
            }

            if ((Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x < 1f && !isMovingRight) ||
                (Input.GetKeyDown(KeyCode.D) && transform.position.x < 1f && !isMovingRight))
            {
                isMovingRight = true;
                StartCoroutine(RightMove());
            }

            if ((Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > -1f && !isMovingLeft) ||
                (Input.GetKeyDown(KeyCode.A) && transform.position.x > -1f && !isMovingLeft))
            {
                isMovingLeft = true;
                StartCoroutine(LeftMove());
            }
        }

        else
        {
            jumpVelocity -= gravity;
        }
        //OnCollision();

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

    //void OnCollision()
    //{
    //    RaycastHit hit;

    //    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayRadius, layer) && !hurt.isInvulnerable)
    //    {
    //        playerManager.TakeDamage(40f);
    //        hurt.ActivateInvulnerability();
    //        StartCoroutine(ApplySlowForDuration(slowDuration));
    //    }
    //}

    // Novo método usando ControllerColliderHit
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        bool isObstacle = ((1 << hit.gameObject.layer) & layer) != 0;

        if (isObstacle && !hurt.isInvulnerable)
        {
            if (Vector3.Dot(Vector3.up, hit.normal) < 0.1f)
            {
                playerManager.TakeDamage(40f);
                hurt.ActivateInvulnerability();
                StartCoroutine(ApplySlowForDuration(slowDuration));
            }
        }
    }

    private IEnumerator ApplySlowForDuration(float duration)
    {
        StopCoroutine(nameof(ApplySlowForDuration));

        speed = originalSpeed - slowAmount;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
    }
}