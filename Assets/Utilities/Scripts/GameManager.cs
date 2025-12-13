using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Checkpoint System")]
    public static int nextCoinTarget = 50;
    public static int savedCoinScore = 0;
    public static float savedDistanceScore = 0f;
    public static bool canRestartFromCheckpoint = false;

    [Header("Checkpoint Settings")]
    [SerializeField] private int coinIntervalForCheckpoint = 50;
    private int currentCoinTarget;

    [Header("Pause")]
    [SerializeField] private GameObject painelPause;
    private bool jogoPausado = false;

    [Header("Tutorial")]
    [SerializeField] private GameObject painelTutorial;

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
        currentCoinTarget = nextCoinTarget;

        CheckForRestartData();

        if (canRestartFromCheckpoint)
        {
            player.SetExtraLifeEffectState(true);
        }

        if (!GameManager.canRestartFromCheckpoint)
        {
            PausarParaTutorial();
        }
    }

    void Update()
    {
        if (painelTutorial.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RetomarJogo();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (jogoPausado)
                RetomarJogo();
            else
                PausarJogo();
        }

        if (player != null && !player.isStop)
        {
            score += Time.deltaTime * player.speed;
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
        }
    }

    private void CheckpointLogic()
    {
        if (scoreCoin >= currentCoinTarget)
        {
            canRestartFromCheckpoint = true;
            player.SetExtraLifeEffectState(true);
            savedCoinScore = scoreCoin;
            savedDistanceScore = score;
            currentCoinTarget += coinIntervalForCheckpoint;
            nextCoinTarget = currentCoinTarget;
        }
    }

    public static void ResetGameStatics()
    {
        nextCoinTarget = 50;
        savedCoinScore = 0;
        savedDistanceScore = 0f;
        canRestartFromCheckpoint = false;
    }

    private void CheckForRestartData()
    {
        if (canRestartFromCheckpoint == true)
        {
            score = savedDistanceScore;
            scoreCoin = savedCoinScore;
            scoreText.text = Mathf.Round(score).ToString();
            scoreCoinText.text = scoreCoin.ToString();

            currentCoinTarget = nextCoinTarget;

            ResetGameStatics();

            if (player != null)
            {
                player.SetExtraLifeEffectState(false);
            }
        }
    }

    public void AddCoin()
    {
        scoreCoin++;
        scoreCoinText.text = scoreCoin.ToString();
        CheckpointLogic();
    }

    public void PausarParaTutorial()
    {
        Time.timeScale = 0f;
        if (painelPause != null) painelPause.SetActive(false);
        if (painelTutorial != null) painelTutorial.SetActive(true);
        jogoPausado = true;
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
        painelTutorial.SetActive(false);
        jogoPausado = false;
    }

    public void IrParaMenu()
    {
        Time.timeScale = 1f;
        GameManager.ResetGameStatics();
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