using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Energy")]
    public float playerEnergy = 100f;
    public float maxEnergy = 100f;
    public Image energyBar;

    void Start()
    {
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
            SceneManager.LoadScene("RestartCutscene");
        }
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
