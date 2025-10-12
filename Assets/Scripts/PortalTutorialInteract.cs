using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalInteract : MonoBehaviour
{
    public string Game; // Nombre de la escena a cargar
    private bool playerInRange = false; // Si el jugador está cerca del portal

    void Update()
    {
        // Si el jugador está en rango y presiona la tecla X
        if (playerInRange && Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene(Game);
        }
    }

    // Detectar cuando el jugador entra en el rango del portal
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Jugador cerca del portal");
        }
    }

    // Detectar cuando el jugador sale del rango
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
