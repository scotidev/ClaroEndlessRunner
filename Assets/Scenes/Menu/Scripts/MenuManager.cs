using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelSettings;
    [SerializeField] private GameObject painelCreditos;

    public void Jogar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    public void AbrirOpcoes()
    {
        painelMenuInicial.SetActive(false);
        painelSettings.SetActive(true);
    }

    public void FecharOpcoes()
    {
        painelMenuInicial.SetActive(true);
        painelSettings.SetActive(false);
    }

    public void AbrirCreditos()
    {
        painelMenuInicial.SetActive(false);
        painelCreditos.SetActive(true);
    }

    public void FecharCreditos()
    {
        painelMenuInicial.SetActive(true);
        painelCreditos.SetActive(false);
    }

    public void SairJogo()
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