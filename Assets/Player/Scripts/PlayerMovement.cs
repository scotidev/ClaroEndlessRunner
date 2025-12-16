using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float jumpHeight = 15f;
    private float jumpVelocity;
    public float gravity = 30f;
    public float speed = 10f;
    private float baseSpeed;
    private float speedBeforeSlow;
    public float maxSpeed = 50f;

    [Header("Lane Movement")]
    private float[] lanes = new float[] { -2.5f, 0f, 2.5f };
    private int currentLane = 1;
    public float laneSmooth = 10f;

    [Header("Slow Effect")]
    public float slowDuration = 2f;
    public float slowPercentage = 0.9f;

    [Header("Extra Life Visual")]
    public GameObject extraLifeEffect;

    [Header("Collectibles")]
    public bool isStop;
    public LayerMask collectiblesLayer;

    [Header("Obstacles")]
    public LayerMask obstaclesLayer;

    [Header("Audio")]
    [SerializeField] private AudioClip damageSFX;
    [SerializeField] private AudioClip jumpSFX;

    [Header("Mobile Input")]
    [SerializeField] private float swipeThreshold = 1f;
    private Vector2 touchStartPos;
    private bool touchMoving;

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
        SetExtraLifeEffectState(GameManager.canRestartFromCheckpoint);

        jumpVelocity = 0f;
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            if (jumpVelocity != 0f)
            {
                jumpVelocity = 0f;
            }
            return;
        }

        HandleKeyboardInput();

        HandleMobileInput();

        if (!controller.isGrounded)
        {
            jumpVelocity -= gravity * Time.deltaTime;
        }
        else
        {
            if (jumpVelocity < 0)
                jumpVelocity = -1f;
        }

        float moveZ = speed;
        float moveY = jumpVelocity;
        float targetX = lanes[currentLane];
        float moveX = (targetX - transform.position.x) * laneSmooth;

        Vector3 finalVelocity = new Vector3(moveX, moveY, moveZ);

        controller.Move(finalVelocity * Time.deltaTime);
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoveToLane(1);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoveToLane(-1);
        }

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void HandleMobileInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                touchMoving = true;
            }
            else if (touch.phase == TouchPhase.Ended && touchMoving)
            {
                Vector2 touchEndPos = touch.position;
                Vector2 swipeDelta = touchEndPos - touchStartPos;

                if (swipeDelta.magnitude > swipeThreshold)
                {
                    if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                    {
                        if (swipeDelta.x > 0)
                        {
                            MoveToLane(1);
                        }
                        else
                        {
                            MoveToLane(-1);
                        }
                    }
                    else
                    {
                        if (swipeDelta.y > 0)
                        {
                            if (controller.isGrounded)
                            {
                                Jump();
                            }
                        }
                    }
                }

                touchMoving = false;
            }
        }
    }

    private void MoveToLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, 0, 2);
    }

    private void Jump()
    {
        jumpVelocity = jumpHeight;
        animator.SetTrigger("Jump");

        if (AudioManager.Instance != null && jumpSFX != null)
        {
            AudioManager.Instance.PlaySFX(jumpSFX, 0.6f);
        }
    }

    public void IncreaseSpeed(float amount)
    {
        baseSpeed += amount;

        baseSpeed = Mathf.Min(baseSpeed, maxSpeed);

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
                if (AudioManager.Instance != null && damageSFX != null)
                {
                    AudioManager.Instance.PlaySFX(damageSFX, 0.6f);
                }
            }
        }
    }

    public void SetExtraLifeEffectState(bool active)
    {
        if (extraLifeEffect != null)
        {
            extraLifeEffect.SetActive(active);
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