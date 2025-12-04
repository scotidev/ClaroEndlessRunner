using UnityEngine;

public class CollectibleMovement : MonoBehaviour
{
    // --- Configurações de Rotação (Girar) ---
    [Header("Rotação")]
    // Velocidade angular (graus por segundo)
    public float rotationSpeed = 90f;

    // --- Configurações de Flutuação (Subir/Descer) ---
    [Header("Flutuação")]
    // A altura máxima que o objeto irá subir e descer (Amplitude)
    public float floatAmplitude = 0.5f;
    // A velocidade da flutuação (Frequência)
    public float floatSpeed = 1f;

    // Variável para armazenar a posição Y inicial
    private float startY;

    void Start()
    {
        // Armazena a posição Y inicial do objeto quando o jogo começa
        startY = transform.position.y;
    }

    void Update()
    {
        // 1. Gira o objeto
        RotateObject();

        // 2. Faz o objeto flutuar (subir e descer)
        FloatObject();
    }

    private void RotateObject()
    {
        // Rotação: Gira o objeto em torno do eixo Y local (para cima/baixo).
        // Time.deltaTime garante que a rotação é independente da taxa de quadros (FPS).
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void FloatObject()
    {
        // Flutuação: Usa a função Seno (sin) para criar um movimento suave e repetitivo.
        // Time.time é o tempo total desde o início do jogo.
        // O resultado varia suavemente entre -1 e 1.
        float newY = startY + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Aplica a nova posição Y, mantendo X e Z originais.
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}