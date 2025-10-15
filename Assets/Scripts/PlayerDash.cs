using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDash : MonoBehaviour
{

    private Rigidbody2D _rb;
    private Player _player;
    private float _baseGravity;

    [Header("Dash")]
    [SerializeField] private float _dashForce = 20f;
    [SerializeField] private float _dashTime = 0.2f;
    [SerializeField] private float _timeCanDash = 1f;
    private bool _isDashing;
    private bool _canDash = true;
    public bool IsDashing => _isDashing;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
        _baseGravity = _rb.gravityScale;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {

        if (_player.Direction != 0 && _canDash)
        {
            _isDashing = true;
            _canDash = false;
            _rb.gravityScale = 0;
            _rb.linearVelocity = new Vector2(_player.Direction * _dashForce, 0f);
            yield return new WaitForSeconds(_dashTime);
            _isDashing = false;
            _rb.gravityScale = _baseGravity;
            yield return new WaitForSeconds(_timeCanDash);
            _canDash = true;   
        }
    }
}