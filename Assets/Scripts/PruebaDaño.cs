using UnityEngine;

public class PruebaDaño : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Comprova si el que toca té el component Vida
        Vida vida = other.GetComponent<Vida>();
        if (vida != null)
        {
            vida.RecibirDanio(1);
            Debug.Log("Jugador ha recibido daño!");
        }
    }
}
