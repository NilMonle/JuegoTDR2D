using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossProjectile : MonoBehaviour
{
    public float lifetime = 6f;
    public AnimationCurve speedCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

    Rigidbody2D rb;
    int damage;
    float speed;
    float spawnTime;
    LayerMask damageLayers;
    Vida cachedPlayerVida;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnTime = Time.time;
    }

    public void Lanzar(Vector2 direction, float baseSpeed, int dano, LayerMask capasDanio)
    {
        speed = Mathf.Max(0.5f, baseSpeed);
        damage = Mathf.Max(1, dano);
        damageLayers = capasDanio;

        rb.linearVelocity = direction.normalized * speed;
        spawnTime = Time.time;
    }

    void Update()
    {
        if (Time.time - spawnTime >= lifetime)
            Destroy(gameObject);

        float t = Mathf.Clamp01((Time.time - spawnTime) / lifetime);
        float factor = speedCurve.Evaluate(t);
        rb.linearVelocity = rb.linearVelocity.normalized * speed * factor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & damageLayers) == 0)
            return;

        var vida = ObtenerVida(other);
        if (vida != null)
            vida.RecibirDanio(damage, other);

        Destroy(gameObject);
    }

    Vida ObtenerVida(Collider2D col)
    {
        if (col == null) return null;

        if (cachedPlayerVida != null && col.transform == cachedPlayerVida.transform)
            return cachedPlayerVida;

        var vida = col.GetComponentInParent<Vida>();
        if (vida == null && col.attachedRigidbody != null)
            vida = col.attachedRigidbody.GetComponent<Vida>();

        if (vida != null && cachedPlayerVida == null)
            cachedPlayerVida = vida;

        return vida;
    }
}