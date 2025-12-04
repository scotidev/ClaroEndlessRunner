using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame() // Method to start the game
    {
        SceneManager.LoadScene(1);
    }
    public void ExitGame() // Method to exit the game
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
#elif UNITY_WEBGL
UnityEngine.Application.OpenURL("about:blank"); // Redirect to a blank page in WebGL builds

#else
        Application.Quit(); 

#endif
    }
}
