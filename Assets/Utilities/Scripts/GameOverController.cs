using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public void VoltarAoMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
