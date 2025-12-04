using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    [Header("Variables")]
    public string[] speetchText;
    public string actorName;
    private DialogueControl dc;
    private bool onRadious;
    private bool isDialogueActive = false;
    public LayerMask playerLayer;
    public float radious;

    void Start()
    {
        dc = FindObjectOfType<DialogueControl>();
    }

    private void FixedUpdate()
    {
        Interact();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame && onRadious && !isDialogueActive)
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        dc.Speetch(speetchText, actorName);
        Debug.Log("Dialogo Iniciado com " + actorName);
    }
    private void EndDialogue()
    {
        isDialogueActive = false;
        dc.HidePanel();
        Debug.Log("Dialogo Encerrado");
    }

    /*  private void OnDrawGizmos() //método opcional
      {
          Gizmos.color = Color.green;
          Gizmos.DrawSphere(transform.position, radious);
      }*/

    public void Interact()
    {
        Vector3 point1 = transform.position + Vector3.up * radious;
        Vector3 point2 = transform.position - Vector3.up * radious;
        Collider[] hits = Physics.OverlapCapsule(point1, point2, radious, playerLayer);
        if (hits.Length > 0)
        {
            if (!onRadious)
            {
                Debug.Log("Jogador entrou no raio de interação com o NPC");
            }
            onRadious = true;
        }
        else
        {
            if (!onRadious)
            {
                Debug.Log("Jogador saiu no raio de interação com o NPC");
            }
            onRadious = false;

            if (isDialogueActive)
            {
                EndDialogue();
            }
        }
    }

}
