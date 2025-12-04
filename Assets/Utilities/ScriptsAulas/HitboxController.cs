using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public float dano = 10f; // quanto o NPC vai tirar por golpe

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager pm = other.GetComponent<PlayerManager>();

            if (pm != null)
            {
                pm.ReceberDano(dano);
            }
        }
    }
}
