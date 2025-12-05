using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Text HStext;

    void Start()
    {
        HStext.text = "H I G H S C O R E: " + PlayerPrefs.GetInt("highscore");
    }
    public void StartButton()
    {
        SceneManager.LoadScene("MainScene");
    }
}
