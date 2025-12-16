using UnityEngine;

public class ClaroBoost : MonoBehaviour
{
    [Header("Boost Effect")]
    public float healAmount = 20f;

    [Header("Audio")]
    [SerializeField] private AudioClip boostSFX;

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

                if (AudioManager.Instance != null && boostSFX != null)
                {
                    AudioManager.Instance.PlaySFX(boostSFX, .7f);
                }
            }

            Destroy(gameObject);
        }
    }
}
