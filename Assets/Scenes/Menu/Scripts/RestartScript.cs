using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverDialogo : MonoBehaviour
{
    [Header("---- UI Components ----")]
    public Text textoPrincipal;
    public Text textoBotaoSim;
    public Text textoBotaoNao;

    [Header("---- Configuração de Cenas ----")]
    public string nomeCenaJogo = "MainScene";
    public string nomeCenaMenu = "MenuPrincipal";

    private bool estadoConfirmacaoMenu = false;

    void Start()
    {
        MostrarPrimeiraPergunta();
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

        textoBotaoSim.text = "SIM (S)";
        textoBotaoNao.text = "NÃO (N)";
    }

    void MostrarSegundaPergunta()
    {
        estadoConfirmacaoMenu = true;

        textoPrincipal.text = "Você deseja mesmo ir para o menu?\nSua pontuação não aumentará com essa decisão.";

        textoBotaoSim.text = "SIM (S)";
        textoBotaoNao.text = "NÃO (N)";
    }

    public void AcaoSim()
    {
        if (estadoConfirmacaoMenu == false)
        {
            SceneManager.LoadScene(nomeCenaJogo);
        }
        else
        {
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