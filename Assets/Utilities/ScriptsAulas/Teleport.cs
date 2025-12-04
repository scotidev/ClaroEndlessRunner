using UnityEngine;
public class Teleport : MonoBehaviour
{

    public Transform player, destino;
    public GameObject playerObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerObj.SetActive(false);
            player.position = destino.position;
            playerObj.SetActive(true);
        }
    }
}
