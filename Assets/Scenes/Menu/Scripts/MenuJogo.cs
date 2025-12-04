using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuJogo : MonoBehaviour
{
    [SerializeField] private string nomeCenaMenu = "Menu";

    public void IrParaMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
