using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Energy")]
    public float playerEnergy = 100f;
    public float maxEnergy = 100f;
    private Image energyBar;

    [Header("Audio")]
    [SerializeField] private AudioClip failSFX;

    private Animator animator;
    private PlayerMovement playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        SetupEnergyBarReference();

        if (GameManager.canRestartFromCheckpoint)
        {
            playerEnergy = maxEnergy - 80f;
        }

        UpdateEnergyBar();
    }

    private void SetupEnergyBarReference()
    {
        Image[] images = FindObjectsOfType<Image>(true);

        foreach (Image img in images)
        {
            if (img.name.Contains("EnergyBarFill") && img.gameObject.activeInHierarchy)
            {
                energyBar = img;
                return;
            }
        }
    }

    void Update()
    {
        UpdateEnergyBar();
    }

    public void TakeDamage(float damage)
    {
        playerEnergy -= damage;

        if (playerEnergy <= 0)
        {
            if (AudioManager.Instance != null && failSFX != null)
            {
                AudioManager.Instance.PlaySFX(failSFX, .2f);
            }

            playerEnergy = 0;
            if (playerMovement != null) playerMovement.enabled = false;
            if (animator != null) animator.SetTrigger("fail");

            if (GameManager.canRestartFromCheckpoint)
            {
                Invoke("RestartGame", 2f);
            }
            else
            {
                Invoke("GameOverScene", 2f);
            }
        }

        UpdateEnergyBar();
    }

    public void Heal(float amount)
    {
        playerEnergy = Mathf.Min(playerEnergy + amount, maxEnergy);
        UpdateEnergyBar();
    }

    void RestartGame()
    {
        SceneManager.LoadScene("RestartGame");
    }

    void GameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            float normalizedEnergy = Mathf.Clamp01(playerEnergy / maxEnergy);
            energyBar.fillAmount = normalizedEnergy;
        }
    }
}