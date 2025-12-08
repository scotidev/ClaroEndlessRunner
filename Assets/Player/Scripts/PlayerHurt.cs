using System.Collections;
using UnityEngine;
public class PlayerHurt : MonoBehaviour
{
    [Header("iFrames Settings")]
    [SerializeField] private float iFramesDuration = 2f;
    [SerializeField] private int numberOfFlashes = 5;
    [SerializeField] private int[] targetLayers;

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
    }

    void Start()
    {
        IgnoreAllLayersCollision(false);
    }

    public void ActivateInvulnerability()
    {
        if (isInvulnerable) return;
        StartCoroutine(Invulnerability());
    }

    private void IgnoreAllLayersCollision(bool isIgnored)
    {
        foreach (int layerNum in targetLayers)
        {
            Physics.IgnoreLayerCollision(gameObject.layer, layerNum, isIgnored);
        }
    }

    private IEnumerator Invulnerability()
    {
        isInvulnerable = true;
        IgnoreAllLayersCollision(true);
        float flashDuration = iFramesDuration / (numberOfFlashes * 2);

        for (int i = 0; i < numberOfFlashes; i++)
        {
            SetModelColor(new Color(0.5f, 0, 0, 0.5f));
            yield return new WaitForSeconds(flashDuration);

            SetModelColor(originalColor);
            yield return new WaitForSeconds(flashDuration);
        }

        IgnoreAllLayersCollision(false);
        isInvulnerable = false;
    }

    private void SetModelColor(Color color)
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = color;
        }
    }
}
