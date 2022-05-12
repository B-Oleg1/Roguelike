using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementScript : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private float _moveX = 0;
    private float _moveY = 0;

    private float _speed = 3.5f;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _moveX = Input.GetAxis("Horizontal");
        _moveY = Input.GetAxis("Vertical");

        if (_rigidbody2D.velocity.magnitude > 0.1f)
        {
            _animator.SetBool("Run", true);
        }
        else
        {
            _animator.SetBool("Run", false);
        }

        if (Mathf.Abs(_moveX) > 0.75f && Mathf.Abs(_moveY) > 0.75f)
        {
            _moveX = 0.75f * (_moveX / Mathf.Abs(_moveX));
            _moveY = 0.75f * (_moveY / Mathf.Abs(_moveY));
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = new Vector2(_moveX, _moveY) * _speed;
    }
}