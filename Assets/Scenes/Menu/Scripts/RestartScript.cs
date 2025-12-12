using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverDialogo : MonoBehaviour
{
    [Header("---- UI Components ----")]
    public TextMeshProUGUI textoPrincipal; // O texto grande do meio
    public TextMeshProUGUI textoBotaoSim;  // O texto dentro do botão vermelho esquerdo
    public TextMeshProUGUI textoBotaoNao;  // O texto dentro do botão vermelho direito

    [Header("---- Configuração de Cenas ----")]
    public string nomeCenaJogo = "MainScene";       // Nome da sua cena de jogo
    public string nomeCenaMenu = "MenuPrincipal";   // Nome da sua cena de menu

    // Variável para saber em qual pergunta estamos
    // false = Primeira pergunta (Quer tentar de novo?)
    // true = Segunda pergunta (Quer mesmo sair?)
    private bool estadoConfirmacaoMenu = false;

    void Start()
    {
        // Começa com a primeira pergunta
        MostrarPrimeiraPergunta();
    }

    void Update()
    {
        // Adiciona suporte ao teclado (S e N)
        if (Input.GetKeyDown(KeyCode.S))
        {
            AcaoSim();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            AcaoNao();
        }
    }

    // --- Lógica Visual ---

    void MostrarPrimeiraPergunta()
    {
        estadoConfirmacaoMenu = false;

        textoPrincipal.text = "Sabemos que a jornada é difícil, mas você vai conseguir.\nGostaria de tentar mais uma vez?";

        // Atualiza o texto dos botões para ficar claro
        textoBotaoSim.text = "SIM (S)";
        textoBotaoNao.text = "NÃO (N)";
    }

    void MostrarSegundaPergunta()
    {
        estadoConfirmacaoMenu = true;

        textoPrincipal.text = "Você deseja mesmo ir para o menu?\nSua pontuação não aumentará com essa decisão.";

        // Inverte a lógica visual para forçar o jogador a ler
        textoBotaoSim.text = "SIM (S)";
        textoBotaoNao.text = "NÃO (N)";
    }

    // --- Ações dos Botões ---

    // Ligue essa função no Botão da Esquerda (SIM)
    public void AcaoSim()
    {
        if (estadoConfirmacaoMenu == false)
        {
            // Pergunta 1: "Quer tentar de novo?" -> SIM
            Debug.Log("Reiniciando o Jogo...");
            SceneManager.LoadScene(nomeCenaJogo);
        }
        else
        {
            // Pergunta 2: "Quer mesmo ir pro menu?" -> SIM
            Debug.Log("Indo para o Menu...");
            SceneManager.LoadScene(nomeCenaMenu);
        }
    }

    // Ligue essa função no Botão da Direita (NÃO)
    public void AcaoNao()
    {
        if (estadoConfirmacaoMenu == false)
        {
            // Pergunta 1: "Quer tentar de novo?" -> NÃO
            // Leva para a tela de confirmação (Segunda chance)
            MostrarSegundaPergunta();
        }
        else
        {
            // Pergunta 2: "Quer mesmo ir pro menu?" -> NÃO
            // O jogador se arrependeu de sair, volta para a pergunta inicial
            MostrarPrimeiraPergunta();
        }
    }
}