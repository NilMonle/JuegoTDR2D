using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerDash _playerDash;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private Transform _checkGround;
    [SerializeField] private float _raycastLength;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Movement")]
    [SerializeField] private float _speed = 4f;
    private float direction;
    public float Direction => direction;

    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerDash = GetComponent<PlayerDash>();
    }

    void Start()
    {

    }

    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");
        if (!_playerDash.IsDashing)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (!_playerDash.IsDashing)
        {
            Move();
        }
    }

    private void Move()
    {
        _rb.linearVelocity = new Vector2(Direction * _speed, _rb.linearVelocity.y);
    }
    
    private void Jump()
    {
        _isGrounded = Physics2D.Raycast(_checkGround.position, Vector2.down, _raycastLength, _groundLayer);
        print(_isGrounded);

        if (Input.GetButtonDown("Jump") && _isGrounded == true)
        {
            _rb.linearVelocity = Vector2.up * _jumpForce;
        }
    }
}