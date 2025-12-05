using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScore : MonoBehaviour
{
    [HideInInspector]
    public int GameScore = 0;
    public Text ScoreText;
    public Text ShadowScoreText;

    void Start()
    {
        GameScore = 0;
    }

    void Update()
    {
        ScoreText.text = "S C O R E: " + GameScore;
        ShadowScoreText.text = "S C O R E: " + GameScore;
    }

    public void ReturnButton()
    {
        if (GameScore > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", GameScore);
        }
        SceneManager.LoadScene("Menu");
    }
}