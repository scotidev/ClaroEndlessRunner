using UnityEngine;

public class ClaroBoost : MonoBehaviour
{
    [Header("Boost Effect")]
    public float healAmount = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            PlayerHurt playerHurt = other.GetComponent<PlayerHurt>();

            if (playerManager != null && playerHurt != null)
            {
                playerHurt.ActivateBoostInvulnerability();

                playerManager.Heal(healAmount);
            }

            Destroy(gameObject);
        }
    }
}
