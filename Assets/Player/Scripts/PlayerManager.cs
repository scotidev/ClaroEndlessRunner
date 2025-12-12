using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Energy")]
    public float playerEnergy = 100f; // Energia atual do jogador
    public float maxEnergy = 100f;    // Energia máxima
    public Image energyBar;           // Referência à barra de vida (UI Image)

    private Animator animator;
    private PlayerMovement playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        // Lógica para reiniciar de um checkpoint
        if (GameManager.canRestartFromCheckpoint)
        {
            // Diminui a energia inicial (Start com 20% de vida)
            playerEnergy = maxEnergy - 80f;
        }

        UpdateEnergyBar();
    }

    void Update()
    {
        // Atualiza a barra de energia a cada frame
        UpdateEnergyBar();
    }

    // Aplica dano ao jogador
    public void TakeDamage(float damage)
    {
        playerEnergy -= damage;

        if (playerEnergy <= 0)
        {
            playerEnergy = 0;
            // Desativa o movimento do jogador
            playerMovement.enabled = false;
            // Dispara a animação de falha
            animator.SetTrigger("fail");

            // Define se deve reiniciar ou ir para a tela de Game Over
            if (GameManager.canRestartFromCheckpoint)
            {
                Invoke("RestartGame", 2f); // Chama RestartGame após 2 segundos
            }
            else
            {
                Invoke("GameOverScene", 2f); // Chama GameOverScene após 2 segundos
            }
        }
    }

    // Cura o jogador
    public void Heal(float amount)
    {
        playerEnergy = Mathf.Min(playerEnergy + amount, maxEnergy);
        UpdateEnergyBar();
    }

    void RestartGame()
    {
        // Carrega a cena de "Segunda Chance" (usada para checkpoint)
        SceneManager.LoadScene("SegundaChanceDesktop");
    }

    void GameOverScene()
    {
        // Carrega a cena de "Game Over"
        SceneManager.LoadScene("GameOver");
    }

    // Atualiza a barra de energia (visual na UI)
    void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            // Calcula a porcentagem de vida e aplica ao fillAmount da imagem
            float normalizedEnergy = Mathf.Clamp01(playerEnergy / maxEnergy);
            energyBar.fillAmount = normalizedEnergy;
        }
    }
}