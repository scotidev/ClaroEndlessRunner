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

    [Header("UI Panes (Filled Dynamically)")]
    private GameObject painelPause;
    private GameObject painelTutorial;
    private bool jogoPausado = false;

    [Header("Audio")]
    [SerializeField] private AudioClip extraLifeSFX;

    [Header("Score")]
    public float score;
    public int scoreCoin;
    private Text scoreText;
    private Text scoreCoinText;

    [Header("Game Speed Settings")]
    [SerializeField] private float speedIncreasePerDistance = 2f;
    [SerializeField] private int distanceInterval = 100;
    private int nextSpeedIncreaseScore;

    private PlayerMovement player;

    void Start()
    {
        SetupDynamicUIReferences();

        if (Time.timeScale == 0f)
        {
            RetomarJogo();
        }

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerMovement>();
        }

        nextSpeedIncreaseScore = distanceInterval;
        currentCoinTarget = nextCoinTarget;

        CheckForRestartData();

        if (canRestartFromCheckpoint && player != null)
        {
            player.SetExtraLifeEffectState(true);
        }

        if (!GameManager.canRestartFromCheckpoint)
        {
            PausarParaTutorial();
        }

        UpdateScoreDisplays();
    }

    private void SetupDynamicUIReferences()
    {
        GameObject canvasRoot = GameObject.FindObjectOfType<Canvas>()?.gameObject;
        if (canvasRoot == null) return;

        Transform activeUIParent = null;

        foreach (Transform child in canvasRoot.transform)
        {
            if (child.gameObject.activeInHierarchy && (child.name == "Desktop" || child.name == "Mobile"))
            {
                activeUIParent = child;
                break;
            }
        }

        if (activeUIParent == null)
        {
            return;
        }

        Transform[] uiChildren = activeUIParent.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in uiChildren)
        {
            if (child.name.Contains("HUD"))
            {
                if (child.gameObject.activeInHierarchy)
                {
                    if (scoreText == null) scoreText = FindTextComponentByName(child, "ScoreText");
                    if (scoreCoinText == null) scoreCoinText = FindTextComponentByName(child, "CoinText");
                }
            }
            else if (child.name.Contains("Pause") && painelPause == null)
            {
                if (child.parent == activeUIParent)
                {
                    painelPause = child.gameObject;
                }
            }
            else if (child.name.Contains("Tutorial") && painelTutorial == null)
            {
                if (child.parent == activeUIParent)
                {
                    painelTutorial = child.gameObject;
                }
            }

            if (scoreText != null && scoreCoinText != null && painelPause != null && painelTutorial != null) break;
        }
    }

    private Text FindTextComponentByName(Transform parent, string name)
    {
        foreach (Transform t in parent.GetComponentsInChildren<Transform>(true))
        {
            if (t.name.Contains(name) && t.gameObject.activeInHierarchy)
            {
                Text txt = t.GetComponent<Text>();
                if (txt != null) return txt;
            }
        }
        return null;
    }

    void Update()
    {
        if (painelTutorial != null && painelTutorial.activeSelf)
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
            UpdateScoreDisplays();
            CheckForSpeedIncrease();
        }
    }

    private void UpdateScoreDisplays()
    {
        if (scoreText != null)
        {
            scoreText.text = Mathf.Round(score).ToString();
        }
        if (scoreCoinText != null)
        {
            scoreCoinText.text = scoreCoin.ToString();
        }
    }

    private void CheckForSpeedIncrease()
    {
        if (score >= nextSpeedIncreaseScore)
        {
            if (player != null)
            {
                player.IncreaseSpeed(speedIncreasePerDistance);
            }
            nextSpeedIncreaseScore += distanceInterval;
        }
    }

    private void CheckpointLogic()
    {
        if (scoreCoin >= currentCoinTarget)
        {
            canRestartFromCheckpoint = true;
            if (player != null) player.SetExtraLifeEffectState(true);
            savedCoinScore = scoreCoin;
            savedDistanceScore = score;
            currentCoinTarget += coinIntervalForCheckpoint;
            nextCoinTarget = currentCoinTarget;

            if (AudioManager.Instance != null && extraLifeSFX != null)
            {
                AudioManager.Instance.PlaySFX(extraLifeSFX, 1.3f);
            }
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
        UpdateScoreDisplays();
        CheckpointLogic();
    }

    public void PausarParaTutorial()
    {
        Time.timeScale = 0f;
        if (painelPause != null) painelPause.SetActive(false);

        if (painelTutorial != null)
        {
            painelTutorial.SetActive(true);
        }

        jogoPausado = true;
    }

    public void PausarJogo()
    {
        Time.timeScale = 0f;
        if (painelPause != null) painelPause.SetActive(true);
        jogoPausado = true;
    }

    public void RetomarJogo()
    {
        Time.timeScale = 1f;
        if (painelPause != null) painelPause.SetActive(false);
        if (painelTutorial != null) painelTutorial.SetActive(false);
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