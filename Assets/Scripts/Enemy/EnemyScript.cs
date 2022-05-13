using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] public int _health;
    [SerializeField] private int _damage;
    [SerializeField] private float _cooldownAttack;

    public GameObject player;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    private float _currentCooldownAttack;

    private void Start()
    {
        print(gameObject.name);

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_currentCooldownAttack > 0) 
        {
            _currentCooldownAttack -= Time.deltaTime;
        }

        if (player != null && _health > 0)
        {
            Flip();

            if (Vector2.Distance(transform.position, player.transform.position) > 0.95f)
            {
                _navMeshAgent.destination = player.transform.position;
            }
            else if (_currentCooldownAttack <= 0)
            {
                _navMeshAgent.destination = transform.position;

                Attack();
            }
        }
    }

    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
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
            _navMeshAgent.destination = transform.position;
            Destroy(GetComponent<BoxCollider2D>());

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