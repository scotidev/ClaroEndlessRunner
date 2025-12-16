using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverDialogo : MonoBehaviour
{
    private Text textoPrincipal;

    [Header("Configuração de Cenas")]
    public string nomeCenaJogo = "MainScene";
    public string nomeCenaMenu = "MenuPrincipal";

    private const string TEXT_TAG = "DialogContentText";

    private bool estadoConfirmacaoMenu = false;

    void Awake()
    {
        GameObject contentObject = GameObject.FindWithTag(TEXT_TAG);

        if (contentObject != null)
        {
            textoPrincipal = contentObject.GetComponent<Text>();
        }
    }

    void Start()
    {
        if (textoPrincipal != null)
        {
            MostrarPrimeiraPergunta();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            AcaoSim();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            AcaoNao();
        }
    }

    void MostrarPrimeiraPergunta()
    {
        estadoConfirmacaoMenu = false;
        textoPrincipal.text = "Sabemos que a jornada é difícil, mas você vai conseguir.\nGostaria de tentar mais uma vez?";
    }

    void MostrarSegundaPergunta()
    {
        estadoConfirmacaoMenu = true;
        textoPrincipal.text = "Você deseja mesmo ir para o menu?\nSua pontuação não aumentará com essa decisão.";
    }

    public void AcaoSim()
    {
        if (estadoConfirmacaoMenu == false)
        {
            SceneManager.LoadScene(nomeCenaJogo);
        }
        else
        {
            if (GameManager.canRestartFromCheckpoint)
            {
                GameManager.ResetGameStatics();
            }
            SceneManager.LoadScene(nomeCenaMenu);
        }
    }

    public void AcaoNao()
    {
        if (estadoConfirmacaoMenu == false)
        {
            MostrarSegundaPergunta();
        }
        else
        {
            MostrarPrimeiraPergunta();
        }
    }
}