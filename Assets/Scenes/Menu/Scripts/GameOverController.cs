using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class GameOverController : MonoBehaviour
{
    
    [Tooltip("O nome exato da cena do Menu Principal (ex: 'MainMenuScene').")]
    public string nomeCenaMenuPrincipal = "MenuPrincipal";

    
    public void VoltarAoMenuPrincipal()
    {
        
        Time.timeScale = 1f;

        
        SceneManager.LoadScene(nomeCenaMenuPrincipal);
    }

 
    private void Start()
    {
      
        Cursor.visible = true;

       
        Cursor.lockState = CursorLockMode.None;
    }

   
    public void SairDoJogo()
    {
        Debug.Log("Saindo do Jogo...");
        Application.Quit();
    }
}
