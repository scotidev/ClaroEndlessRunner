using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public float dano = 10f; // quanto o NPC vai tirar por golpe

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManagerAula pm = other.GetComponent<PlayerManagerAula>();

            if (pm != null)
            {
                pm.ReceberDano(dano);
            }
        }
    }
}
