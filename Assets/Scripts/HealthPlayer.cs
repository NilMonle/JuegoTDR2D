using UnityEngine;
public class HealthPlayer : MonoBehaviour 
{
    [SerializedField] private int vidaMaxima;
    [SerializedField] private int vidaActual;

    private void Awake()
    {
        vidaActual = vidaMaxima;
    }

    public void TomarDaño(int daño)
    {
        int vidaTemporal = vidaActual - daño;
        vidaTemporal = Mathf.Clamp(vidaTemporal, 0, vidaMaxima);
        vidaActual = vidaTemporal;
        if (vidaActual <= 0)
        {
            DestruirJugador()
        }

    }

    private void DestruirJugador()
    {
        Destroy(gameObject)
    }

    public void CurarVida(int curacion)
    {
        int vidaTemporal = vidaActual + curacion;
        vidaTemporal = Mathf.Clamp(vidaTemporal, 0, vidaMaxima);
        vidaActual = vidaTemporal;
    }

    public int GetVidaMaxima() => vidaMaxima;
    public int GetVidaActual() => vidaActual;

}
