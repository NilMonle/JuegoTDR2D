using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int vidaMax = 10;
    public int vidaActual;

    public Rigidbody2D rb; // Rigidbody del enemigo
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;

    float knockbackEndTime = -1f;

    public bool IsRecoiling => Time.time < knockbackEndTime;

    void Awake()
    {
        vidaActual = vidaMax;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage, Vector2 knockbackDir)
    {
        // Restar vida
        vidaActual -= damage;

        // Revisar muerte
        if (vidaActual <= 0)
        {
            // 1) Miramos si este enemigo es un BOSS
            BossController boss = GetComponent<BossController>();

            if (boss != null)
            {
                // Si es boss, dejamos que el BossController gestione la animación de muerte
                boss.Morir();
            }
            else
            {
                // Si NO es boss, simplemente destruimos el objeto
                Destroy(gameObject);
            }

            // Importante: salimos para no aplicar knockback a un muerto
            return;
        }

        // Aplicar knockback
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // resetear velocidad antes del golpe
            rb.AddForce(knockbackDir.normalized * knockbackForce, ForceMode2D.Impulse);
            knockbackEndTime = Time.time + knockbackDuration;
        }
    }
}
