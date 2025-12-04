using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private CharacterController controller;
    private Transform myCamera;
    private Animator animator;
    [SerializeField] private Transform Foot;
    [SerializeField] private LayerMask colisionLayer;

    [Header("Variables")]
    public float velocity = 5f;
    private bool isGround;
    private float yForce;

    void Start()
    {

        controller = GetComponent<CharacterController>();
        myCamera = Camera.main.transform;
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();

    }

    public void Move()
    {
        // Debug.Log("Executando o movimento do personagem...");

        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;

        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical -= 1f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical += 1f;

        Vector3 movimento = new Vector3(horizontal, 0, vertical);

        movimento = Vector3.ClampMagnitude(movimento, 1f); // Normaliza a velocidade diagonal de movimento
        movimento = myCamera.TransformDirection(movimento);
        movimento.y = 0; // Mantém o movimento no plano horizontal

        controller.Move(movimento * velocity * Time.deltaTime); // Aplica o movimento ao CharacterController 
        if (movimento != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation, Quaternion.LookRotation(movimento),
                Time.deltaTime * 10f
             );
        }

        animator.SetBool("isRunning", movimento != Vector3.zero);
        isGround = Physics.CheckSphere(Foot.position, 0.3f, colisionLayer);
        //Botar parametro isGround no animator
        animator.SetBool("isGround", isGround);
    }

    public void Jump()
    {
        // Debug.Log("Estou no chão?" + isGround);
        // Debug.Log("Estou no chão?" + isGround);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGround)
        {
            yForce = 5f;
            animator.SetTrigger("Jump");

        }

        if (yForce > -9.81f)
        {
            yForce += -9 * Time.deltaTime;
        }

        controller.Move(new Vector3(0, yForce, 0) * Time.deltaTime); // Aplica a gravidade ao CharacterController

    }

}
