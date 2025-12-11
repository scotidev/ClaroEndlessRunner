using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private GameObject painelPause;
    private bool jogoPausado = false;

    [Header("Score")]
    public float score;
    public int scoreCoin;
    public Text scoreText;
    public Text scoreCoinText;

    [Header("Game Speed Settings")]
    [SerializeField] private float speedIncreasePerDistance = 2f;
    [SerializeField] private int distanceInterval = 100;
    private int nextSpeedIncreaseScore;

    private PlayerMovement player;

    void Start()
    {
        if (Time.timeScale == 0f) RetomarJogo();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        nextSpeedIncreaseScore = distanceInterval;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (jogoPausado)
                RetomarJogo();
            else
                PausarJogo();
        }

        if (!player.isStop)
        {
            score += Time.deltaTime * 10f;
            scoreText.text = Mathf.Round(score).ToString();

            CheckForSpeedIncrease();
        }
    }

    private void CheckForSpeedIncrease()
    {
        if (score >= nextSpeedIncreaseScore)
        {
            player.IncreaseSpeed(speedIncreasePerDistance);

            nextSpeedIncreaseScore += distanceInterval;

            Debug.Log($"Velocidade aumentada! Nova Velocidade Base: {player.speed}");
        }
    }

    public void AddCoin()
    {
        scoreCoin++;
        scoreCoinText.text = scoreCoin.ToString();
    }

    public void PausarJogo()
    {
        Time.timeScale = 0f;
        painelPause.SetActive(true);
        jogoPausado = true;
    }

    public void RetomarJogo()
    {
        Time.timeScale = 1f;
        painelPause.SetActive(false);
        jogoPausado = false;
    }

    public void IrParaMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void SairDoJogo()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        UnityEngine.Application.OpenURL("about:blank");
#else
        Application.Quit();
#endif
    }
}
