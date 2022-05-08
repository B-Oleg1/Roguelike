using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    [SerializeField] private float _cooldownAttack;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private GameObject _player;

    private float _currentCooldownAttack;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
    }

    private void Update()
    {
        if (_currentCooldownAttack > 0) 
        {
            _currentCooldownAttack -= Time.deltaTime;
        }

        if (_player != null && _health > 0)
        {
            if (transform.position.x > _player.transform.position.x)
            {
                _spriteRenderer.flipX = true;
            } 
            else
            {
                _spriteRenderer.flipX = false;
            }

            if (Vector2.Distance(transform.position, _player.transform.position) > 0.95f)
            {
                _navMeshAgent.destination = _player.transform.position;
            }
            else if (_currentCooldownAttack <= 0)
            {
                _navMeshAgent.destination = transform.position;

                Attack();
            }
        }
    }

    private void Attack()
    {
        _currentCooldownAttack = _cooldownAttack;

        _animator.SetTrigger("Attack");
        PlayerInfoScript.Instance.UpdateHeal(-_damage);
    }

    public void TakeHit(int damage)
    {
        if (damage >= _health)
        {
            _health = 0;
            _animator.SetTrigger("Death");

            GameManagerScript.Instance.SpawnOtherItems(OtherTypeItems.Coin, 3, transform.position);
            GameManagerScript.Instance.SpawnOtherItems(OtherTypeItems.Health, 1, transform.position);
            GameManagerScript.Instance.SpawnOtherItems(OtherTypeItems.Energy, 3, transform.position);

            Destroy(gameObject, 5);
        }
        else
        {
            _animator.SetTrigger("Hit");
            _health -= damage; 
        }
    }
}