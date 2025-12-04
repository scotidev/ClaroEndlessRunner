using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public float delay = 5f;

    void Start()
    {
        StartCoroutine(LoadAfterSeconds());
    }

    IEnumerator LoadAfterSeconds()
    {
        // espera o tempo definido
        yield return new WaitForSeconds(delay);

        // volta para o Menu (ID 0)
        SceneManager.LoadScene(0);
    }
}
