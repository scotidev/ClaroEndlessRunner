using UnityEngine;

public class ForceAnimatorStart : MonoBehaviour
{
    private Animator anim;

    // Use o mesmo nome da animação
    private const string ANIMATION_NAME = "MoveLeft";

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim != null)
        {
            // Pega o ID hash da string
            int animationHash = Animator.StringToHash(ANIMATION_NAME);

            // Tenta reproduzir usando o ID hash (mais robusto)
            anim.Play(animationHash);
            Debug.Log($"LOG ANIM: Tentando forçar o início da animação por Hash: {ANIMATION_NAME}");
        }
        else
        {
            Debug.LogError("LOG ANIM ERRO: Animator não encontrado no objeto.");
        }
    }
}