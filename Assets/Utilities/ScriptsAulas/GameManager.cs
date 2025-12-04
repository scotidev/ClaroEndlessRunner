using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject img;

    void Start()
    {
        if (Time.timeScale == 0f) Resume();
    }

    void Update()
    {
        ReturnGame();
        CallPauseResume();
    }

    public void ReturnGame()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void CallPauseResume()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        img.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        img.SetActive(false);
    }
}
