using System.Collections;
using UnityEngine;

public class PlayerHurt : MonoBehaviour
{
    [Header("iFrames Settings")]
    [SerializeField] private float iFramesDuration = 2f; // Duração total da invulnerabilidade após o dano
    [SerializeField] private int numberOfFlashes = 5;      // Quantidade de vezes que o modelo piscará
    [SerializeField] private int[] targetLayers;           // Layers que devem ser ignoradas durante os iFrames (ex: Obstacle)
    [SerializeField] private Color damageFlashColor = new Color(0.8f, 0, 0, 0.5f); // Cor do flash de dano

    [Header("Boost Settings")]
    public float boostDuration = 4f;                      // Duração da invulnerabilidade por boost
    public GameObject boostParticleEffect;                 // Efeito de partícula para o boost

    private SkinnedMeshRenderer meshRenderer;             // Referência ao renderizador do modelo (para mudar a cor)
    private Color originalColor;                          // Cor original do modelo
    public bool isInvulnerable = false;                   // Flag de invulnerabilidade

    void Awake()
    {
        // Obtém o renderizador do modelo (supondo que esteja em um objeto filho)
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        if (meshRenderer != null)
        {
            // Armazena a cor original do material
            originalColor = meshRenderer.material.color;
        }

        if (boostParticleEffect != null)
        {
            boostParticleEffect.SetActive(false);
        }
    }

    void Start()
    {
        // Garante que a colisão está ativada no início
        IgnoreAllLayersCollision(false);
    }

    // Ativa iFrames após tomar dano
    public void ActivateInvulnerability()
    {
        if (isInvulnerable) return; // Se já estiver invulnerável, ignora
        StartCoroutine(Invulnerability(iFramesDuration, damageFlashColor, false));
    }

    // Ativa invulnerabilidade de Boost
    public void ActivateBoostInvulnerability()
    {
        if (isInvulnerable) return;

        if (boostParticleEffect != null)
        {
            boostParticleEffect.SetActive(true);
        }

        StartCoroutine(Invulnerability(boostDuration, Color.clear, true));
    }

    // Ignora ou reativa a colisão com as Target Layers
    private void IgnoreAllLayersCollision(bool isIgnored)
    {
        foreach (int layerNum in targetLayers)
        {
            Physics.IgnoreLayerCollision(gameObject.layer, layerNum, isIgnored);
        }
    }

    private IEnumerator Invulnerability(float duration, Color flashColor, bool isBoost)
    {
        isInvulnerable = true;
        IgnoreAllLayersCollision(true); // Desativa colisão

        if (!isBoost)
        {
            // Cálculo da duração de cada flash (duração total / (nº de flashes * 2, pois é pisca/volta))
            float flashDuration = duration / (numberOfFlashes * 2);

            for (int i = 0; i < numberOfFlashes; i++)
            {
                // Pisca (Muda para a cor de dano)
                SetModelColor(flashColor);
                yield return new WaitForSeconds(flashDuration);

                // Volta (Muda para a cor original)
                SetModelColor(originalColor);
                yield return new WaitForSeconds(flashDuration);
            }
        }

        // Garante que o modelo está na cor original
        SetModelColor(originalColor);

        float remainingTime = duration;

        if (!isBoost)
        {
            // Calcula o tempo que foi gasto na animação de piscar para subtrair do tempo de espera final
            float timeSpentFlashing = (iFramesDuration / (numberOfFlashes * 2)) * numberOfFlashes * 2;
            remainingTime = duration - timeSpentFlashing;
        }

        // Espera o tempo restante de invulnerabilidade
        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        IgnoreAllLayersCollision(false); // Reativa colisão
        isInvulnerable = false;

        if (isBoost && boostParticleEffect != null)
        {
            boostParticleEffect.SetActive(false);
        }
    }

    // Altera a cor do material do modelo
    private void SetModelColor(Color color)
    {
        if (meshRenderer != null)
        {
            // Nota: Se o modelo piscar de forma estranha, verifique o modo de renderização do material.
            meshRenderer.material.color = color;
        }
    }
}