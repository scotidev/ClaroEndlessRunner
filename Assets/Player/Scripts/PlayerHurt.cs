using System.Collections;
using UnityEngine;

public class PlayerHurt : MonoBehaviour
{
    [Header("iFrames Settings")]
    [SerializeField] private float iFramesDuration = 2f;
    [SerializeField] private int numberOfFlashes = 5;
    [SerializeField] private int[] targetLayers;
    [SerializeField] private Color damageFlashColor = new Color(0.8f, 0, 0, 0.5f);

    [Header("Boost Settings")]
    public float boostDuration = 4f;
    public GameObject boostParticleEffect;

    private SkinnedMeshRenderer meshRenderer;
    private Color originalColor;
    public bool isInvulnerable = false;

    void Awake()
    {
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        if (meshRenderer != null)
        {
            originalColor = meshRenderer.material.color;
        }

        if (boostParticleEffect != null)
        {
            boostParticleEffect.SetActive(false);
        }
    }

    void Start()
    {
        IgnoreAllLayersCollision(false);
    }

    public void ActivateInvulnerability()
    {
        if (isInvulnerable) return;
        StartCoroutine(Invulnerability(iFramesDuration, damageFlashColor, false));
    }

    public void ActivateBoostInvulnerability()
    {
        if (isInvulnerable) return;

        if (boostParticleEffect != null)
        {
            boostParticleEffect.SetActive(true);
        }

        StartCoroutine(Invulnerability(boostDuration, Color.clear, true));
    }

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
        IgnoreAllLayersCollision(true);

        if (!isBoost)
        {
            float flashDuration = duration / (numberOfFlashes * 2);

            for (int i = 0; i < numberOfFlashes; i++)
            {
                SetModelColor(flashColor);
                yield return new WaitForSeconds(flashDuration);

                SetModelColor(originalColor);
                yield return new WaitForSeconds(flashDuration);
            }
        }

        SetModelColor(originalColor);

        float remainingTime = duration;

        if (!isBoost)
        {
            float timeSpentFlashing = (iFramesDuration / (numberOfFlashes * 2)) * numberOfFlashes * 2;
            remainingTime = duration - timeSpentFlashing;
        }

        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        IgnoreAllLayersCollision(false);
        isInvulnerable = false;

        if (isBoost && boostParticleEffect != null)
        {
            boostParticleEffect.SetActive(false);
        }
    }

    private void SetModelColor(Color color)
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = color;
        }
    }
}