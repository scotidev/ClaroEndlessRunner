using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Energy")]
    public float playerEnergy = 100f;
    public float maxEnergy = 100f;
    public Image energyBar;

    private Animator animator;
    private PlayerMovement playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        if (GameManager.canRestartFromCheckpoint)
        {
            playerEnergy = maxEnergy - 80f;
        }

        UpdateEnergyBar();
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
            playerEnergy = 0;
            playerMovement.enabled = false;
            animator.SetTrigger("fail");

            if (GameManager.canRestartFromCheckpoint)
            {
                Invoke("RestartGame", 2f);
            }
            else
            {
                Invoke("GameOverScene", 2f);
            }
        }
    }

    public void Heal(float amount)
    {
        playerEnergy = Mathf.Min(playerEnergy + amount, maxEnergy);
        UpdateEnergyBar();
    }

    void RestartGame()
    {
        SceneManager.LoadScene("SegundaChanceDesktop");
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