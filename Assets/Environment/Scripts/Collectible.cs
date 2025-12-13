using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip collectSFX;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                gameManager.AddCoin();
            }

            if (AudioManager.Instance != null && collectSFX != null)
            {
                AudioManager.Instance.PlaySFX(collectSFX, 1f);
            }

            Destroy(gameObject);
        }
    }
}
