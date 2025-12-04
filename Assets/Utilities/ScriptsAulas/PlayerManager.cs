using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Vida")]
    public float vidaJogador = 100f;
    public float vidaMaxima = 100f;
    public Image barraDeVida;

    [Header("Sistema de Salvamento")]
    private bool podeSalvar = false;
    private Vector3 ultimaPosicaoSalva;
    private float ultimaVidaSalva;
    private bool temDadosSalvos = false;

    void Start()
    {
        CarregarDadosJogador();
        AtualizarBarraVida();
    }

    void Update()
    {
        AtualizarBarraVida();
        TesteDeDano();

        if (podeSalvar && Keyboard.current.bKey.wasPressedThisFrame)
        {
            SalvarDadosJogador();
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            LimparDadosJogador();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PontodeSalvamento"))
        {
            Debug.Log("Colidiu com ponto de salvamento! Pressione B para salvar.");
            podeSalvar = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PontodeSalvamento"))
        {
            Debug.Log("Saiu do ponto de salvamento.");
            podeSalvar = false;
        }
    }

    // -----------------------------
    // SISTEMA DE SALVAMENTO
    // -----------------------------

    void SalvarDadosJogador()
    {
        ultimaPosicaoSalva = transform.position;
        ultimaVidaSalva = vidaJogador;

        PlayerPrefs.SetFloat("PosX", ultimaPosicaoSalva.x);
        PlayerPrefs.SetFloat("PosY", ultimaPosicaoSalva.y);
        PlayerPrefs.SetFloat("PosZ", ultimaPosicaoSalva.z);

        PlayerPrefs.SetFloat("Vida", ultimaVidaSalva);

        PlayerPrefs.SetInt("temDadosSalvos", 1);

        Debug.Log("Dados salvos com sucesso!");
        podeSalvar = false;
    }

    void LimparDadosJogador()
    {
        PlayerPrefs.DeleteKey("PosX");
        PlayerPrefs.DeleteKey("PosY");
        PlayerPrefs.DeleteKey("PosZ");
        PlayerPrefs.DeleteKey("Vida");
        PlayerPrefs.DeleteKey("temDadosSalvos");

        ultimaPosicaoSalva = Vector3.zero;
        ultimaVidaSalva = 0f;
        temDadosSalvos = false;

        vidaJogador = vidaMaxima;
        AtualizarBarraVida();

        Debug.Log("Dados apagados com sucesso!");
    }

    void CarregarDadosJogador()
    {
        if (PlayerPrefs.GetInt("temDadosSalvos") == 1)
        {
            float x = PlayerPrefs.GetFloat("PosX");
            float y = PlayerPrefs.GetFloat("PosY");
            float z = PlayerPrefs.GetFloat("PosZ");

            ultimaPosicaoSalva = new Vector3(x, y, z);
            ultimaVidaSalva = PlayerPrefs.GetFloat("Vida");

            transform.position = ultimaPosicaoSalva;
            vidaJogador = ultimaVidaSalva;

            temDadosSalvos = true;
            Debug.Log("Dados carregados com sucesso!");
        }
        else
        {
            Debug.Log("Nenhum dado salvo encontrado.");
        }
    }

    // -----------------------------
    // VIDA / DANO
    // -----------------------------

    public void ReceberDano(float quantidade)
    {
        vidaJogador -= quantidade;
        AtualizarBarraVida();

        if (vidaJogador <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }

    // Função usada somente para teste com tecla H
    void TesteDeDano()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            ReceberDano(10f);
        }
    }

    void AtualizarBarraVida()
    {
        if (barraDeVida == null) return;

        float vidaNormalizada = Mathf.Clamp01(vidaJogador / vidaMaxima);
        barraDeVida.fillAmount = vidaNormalizada;
    }
}
