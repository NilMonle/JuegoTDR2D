using UnityEngine;
using UnityEngine.Events;

public class Vida : MonoBehaviour
{
    [Header("Ajustes de vida")]
    public int vidaMax = 3;
    public int vidaActual;

    [Header("Invulnerabilidad opcional tras daño")]
    public float tiempoInvulnerable = 0.4f;

    [Header("Knockback")]
    [Tooltip("Fuerza de empuje aplicada al jugador al recibir daño.")]
    public float knockbackForce = 6f;

    [Header("Eventos")]
    // (vidaActual, vidaMax)
    public UnityEvent<int, int> onVidaCambia;
    public UnityEvent onMuerte;
    public UnityEvent<int> onDanio;
    public UnityEvent<int> onCura;

    public bool EstaMuerto { get; private set; }
    bool invulnerable;

    void Awake()
    {
        vidaActual = Mathf.Clamp(vidaMax, 1, int.MaxValue);
        NotificarVida();
    }

    // Método original: sin fuente, igual que antes
    public void RecibirDanio(int cantidad) => RecibirDanio(cantidad, Vector2.zero, 0f, false);

    public void RecibirDanio(int cantidad, Collider2D fuente)
    {
        if (fuente == null || !fuente.CompareTag("Enemy")) return;

        Vector2 knockbackDir = (transform.position - fuente.transform.position).normalized;
        RecibirDanio(cantidad, knockbackDir, knockbackForce);
    }
    public void RecibirDanio(int cantidad, Vector2 knockbackDir, float fuerzaKnockback)
    {
        RecibirDanio(cantidad, knockbackDir, fuerzaKnockback, true);
    }

    // Nueva versión filtrando por tag
    bool RecibirDanio(int cantidad, Vector2 knockbackDir, float fuerzaKnockback, bool aplicarKnockback)
    {
        if (EstaMuerto || invulnerable || cantidad <= 0) return false;

        vidaActual = Mathf.Max(vidaActual - cantidad, 0);
        onDanio?.Invoke(cantidad);
        NotificarVida();

        if (vidaActual <= 0)
        {
            EstaMuerto = true;
            onMuerte?.Invoke();
        }
        else if (tiempoInvulnerable > 0f)
        {
            if (!gameObject.activeInHierarchy) return false;
            StopAllCoroutines();
            StartCoroutine(InvulnerablePor(tiempoInvulnerable));
        }

        if (aplicarKnockback)
            AplicarKnockback(knockbackDir, fuerzaKnockback);

        return true;
    }


    public void Curar(int cantidad)
    {
        if (EstaMuerto || cantidad <= 0) return;
        int antes = vidaActual;
        vidaActual = Mathf.Min(vidaActual + cantidad, vidaMax);
        int curado = vidaActual - antes;
        if (curado > 0) onCura?.Invoke(curado);
        NotificarVida();
    }

    public void Revivir(int vidaAlRevivir = -1)
    {
        EstaMuerto = false;
        vidaActual = (vidaAlRevivir > 0) ? Mathf.Min(vidaAlRevivir, vidaMax) : vidaMax;
        invulnerable = false;
        NotificarVida();
    }

    void NotificarVida() => onVidaCambia?.Invoke(vidaActual, vidaMax);

    System.Collections.IEnumerator InvulnerablePor(float t)
    {
        invulnerable = true;
        yield return new WaitForSeconds(t);
        invulnerable = false;
    }

    void AplicarKnockback(Vector2 knockbackDir, float fuerza)
    {
        if (fuerza <= 0f) return;

        var movement = GetComponent<PlayerMovement>();
        if (movement == null) return;

        if (knockbackDir == Vector2.zero)
            knockbackDir = Vector2.right * (movement.facingRight ? -1f : 1f);

        movement.ApplyKnockback(knockbackDir.normalized, fuerza);
    }
}
