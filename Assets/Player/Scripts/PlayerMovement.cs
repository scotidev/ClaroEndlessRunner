using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float jumpHeight;
    private float jumpVelocity;
    public float gravity;
    public float horizontalSpeed;
    private bool isMovingLeft;
    private bool isMovingRight;
    public float speed;
    private float baseSpeed;
    private float speedBeforeSlow;

    [Header("Slow Effect")]
    public float slowDuration = 2f;
    public float slowPercentage = 0.9f;

    [Header("Collectibles")]
    public bool isStop;
    public LayerMask collectiblesLayer;

    [Header("Obstacles")]
    public LayerMask obstaclesLayer;

    private CharacterController controller;
    private PlayerHurt hurt;
    private PlayerManager playerManager;
    private Animator animator;
    private GameManager gameManager;
    private Coroutine slowCoroutine;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        hurt = GetComponent<PlayerHurt>();
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        baseSpeed = speed;
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

    public void IncreaseSpeed(float amount)
    {
        baseSpeed += amount;

        if (slowCoroutine == null)
        {
            speed = baseSpeed;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        bool isObstacle = ((1 << hit.gameObject.layer) & obstaclesLayer) != 0;

        if (isObstacle && !hurt.isInvulnerable)
        {
            if (Vector3.Dot(Vector3.up, hit.normal) < 0.1f)
            {
                playerManager.TakeDamage(20f);
                hurt.ActivateInvulnerability();
                ApplySlow();
            }
        }
    }

    private void ApplySlow()
    {
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }
        speedBeforeSlow = baseSpeed;

        slowCoroutine = StartCoroutine(ApplySlowForDuration(slowDuration));
    }

    private IEnumerator ApplySlowForDuration(float duration)
    {
        speed = speedBeforeSlow * (1 - slowPercentage);

        speed = Mathf.Max(speed, 1f);

        yield return new WaitForSeconds(duration);

        speed = baseSpeed;

        slowCoroutine = null;
    }
}