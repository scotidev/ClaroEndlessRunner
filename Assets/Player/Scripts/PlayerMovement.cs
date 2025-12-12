using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float jumpHeight;
    private float jumpVelocity;
    public float gravity;
    public float speed;
    private float baseSpeed;

    [Header("Lane Movement")]
    // AQUI ESTA A MUDANÇA: Aumentei de 1 para 3.5. 
    // Se ainda achar pouco, mude o 3.5f para 4f ou 5f.
    private float[] lanes = new float[] { -3.5f, 0f, 3.5f };
    private int currentLane = 1;
    public float laneSmooth = 10f;

    [Header("Slow Effect")]
    public float slowDuration = 2f;
    public float slowPercentage = 0.75f;
    private float speedBeforeSlow;
    private Coroutine slowCoroutine;

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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        hurt = GetComponent<PlayerHurt>();
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();

        baseSpeed = speed;
        SetExtraLifeEffectState(GameManager.canRestartFromCheckpoint);
    }

    void Update()
    {
        // 1. INPUTS DE MOVIMENTO LATERAL
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

        // 2. CÁLCULO DE PULO E GRAVIDADE
        if (controller.isGrounded)
        {
            // Resetamos a gravidade
            if (jumpVelocity < 0)
                jumpVelocity = -2f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpVelocity = jumpHeight;
                animator.SetTrigger("Jump");
            }
        }
        else
        {
            // Aplica a gravidade se estiver no ar
            jumpVelocity -= gravity * Time.deltaTime;
        }

        // 3. CÁLCULO FINAL DOS VETORES DE MOVIMENTO

        // EIXO Z (Frente)
        float moveZ = speed;

        // EIXO Y (Pulo/Gravidade)
        float moveY = jumpVelocity;

        // EIXO X (Lateral)
        float targetX = lanes[currentLane];
        // O laneSmooth define a velocidade da troca de faixa
        float moveX = (targetX - transform.position.x) * laneSmooth;

        // 4. APLICAÇÃO NO CONTROLE
        Vector3 finalVelocity = new Vector3(moveX, moveY, moveZ);

        controller.Move(finalVelocity * Time.deltaTime);
    }

    // ---------- SPEED BOOST ----------
    public void IncreaseSpeed(float amount)
    {
        baseSpeed += amount;

        if (slowCoroutine == null)
            speed = baseSpeed;
    }

    // ---------- OBSTACLE COLLISION ----------
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
            extraLifeEffect.SetActive(active);
    }

    // ----------- SLOW EFFECT -----------
    private void ApplySlow()
    {
        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        speedBeforeSlow = baseSpeed;
        slowCoroutine = StartCoroutine(ApplySlowForDuration(slowDuration));
    }

    private IEnumerator ApplySlowForDuration(float duration)
    {
        speed = baseSpeed * slowPercentage;

        yield return new WaitForSeconds(duration);

        speed = speedBeforeSlow;
        slowCoroutine = null;
    }
}