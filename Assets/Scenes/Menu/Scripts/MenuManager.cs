using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
   
    [SerializeField] private string nomeDoLevelDeJogo = "Game";
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelSettings;
    [SerializeField] private GameObject painelCreditos;
    [SerializeField] private GameObject painelPause;



    [SerializeField] private string nomeCenaMenuPrincipal = "Menu"; // Defina o nome da sua cena de menu no Inspector

    private bool jogoPausado = false;


    void Update()
    {
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

    }


   
    public void VoltarAoMenuPrincipal()
    {
        
        Time.timeScale = 1f;

        // Carrega a cena do menu principal
        SceneManager.LoadScene(nomeCenaMenuPrincipal);
    }
 


    public void Jogar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeDoLevelDeJogo); // Usando a variável de string para Jogar
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
        SceneManager.LoadScene(nomeCenaMenuPrincipal);
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
        Debug.Log("Sair do Jogo");

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