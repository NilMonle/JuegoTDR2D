using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _baseGravity;

    [Header("Dash")]
    [SerializeField] private float _dashingTime = 0.2f;     // tiempo que dura el dash
    [SerializeField] private float _dashForce   = 20f;      // fuerza/velocidad del dash
    [SerializeField] private float _timeCanDash = 1f;       // cooldown entre dashes

    private bool _isDashing;
    private bool _canDash = true;
    private bool IsDashing => _isDashing;

    // Dirección horizontal actual: -1 izquierda, 1 derecha
    private int _facing = 1;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _baseGravity = _rb.gravityScale;
    }

    private void Update()
    {
        // Actualiza dirección según input horizontal
        float h = Input.GetAxisRaw("Horizontal");
        if (h != 0) _facing = h > 0 ? 1 : -1;

        // Respeta cooldown
        if (!_isDashing && _canDash && Input.GetKeyDown(KeyCode.F))
            {
                 StartCoroutine(Dash());
            }

    }

    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;

        // Desactiva gravedad y aplica impulso horizontal
        float originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;

        // En versiones actuales usa 'velocity' (no existe 'linearVelocity' en la mayoría de builds)
        _rb.linearVelocity = new Vector2(_facing * _dashForce, 0f);

        yield return new WaitForSeconds(_dashingTime);

        // Restaura estado
        _rb.gravityScale = originalGravity;
        _isDashing = false;

        // Cooldown antes de poder volver a dasear
        yield return new WaitForSeconds(_timeCanDash);
        _canDash = true;
    }
}
