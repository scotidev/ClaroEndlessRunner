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

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (currentLane < 2)
                currentLane++;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (currentLane > 0)
                currentLane--;
        }

        if (controller.isGrounded)
        {
            if (jumpVelocity < 0)
                jumpVelocity = -1f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpVelocity = jumpHeight;
                animator.SetTrigger("Jump");
            }
        }
        else
        {
            jumpVelocity -= gravity * Time.deltaTime;
        }

        float moveZ = speed;
        float moveY = jumpVelocity;
        float targetX = lanes[currentLane];
        float moveX = (targetX - transform.position.x) * laneSmooth;

        Vector3 finalVelocity = new Vector3(moveX, moveY, moveZ);

        controller.Move(finalVelocity * Time.deltaTime);
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