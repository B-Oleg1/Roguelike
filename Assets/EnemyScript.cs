using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _damage;

    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private GameObject _player;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_player != null && Vector2.Distance(transform.position, _player.transform.position) < 3f)
        {
            _animator.SetTrigger("Attack");
            PlayerInfoScript.Instance.UpdateHeal(_damage);
        }
        else
        {
            _navMeshAgent.destination = _player.transform.position;
        }
    }

    public void TakeHit(int damage)
    {
        if (damage >= _health)
        {
            _animator.SetTrigger("Dead");
            Destroy(gameObject);
        }
        else
        {
            _animator.SetTrigger("Hit");
            _health -= damage;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _player = collision.gameObject;
        }
    }
}