using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject painelPause;

    private bool jogoPausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (jogoPausado)
                RetomarJogo();
            else
                PausarJogo();
        }
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

