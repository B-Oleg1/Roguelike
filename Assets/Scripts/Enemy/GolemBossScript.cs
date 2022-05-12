using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBossScript : MonoBehaviour
{
    [SerializeField] private Sprite _secondPhase;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _damage;
    [SerializeField] private float _cooldownAttack;

    public GameObject player;

    private GameObject[] _allEnemy;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    private int _currentPhase;
    private int _currentHealth;
    private float _currentCooldownAttack;
    private bool _regenerationActive;

    private void Start()
    {
        _allEnemy = 
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        if (_currentCooldownAttack > 0)
        {
            _currentCooldownAttack -= Time.deltaTime;
        }

        if (player != null && _currentHealth > 0)
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
        if (damage >= _currentHealth)
        {
            _currentHealth = 0;
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
            _currentHealth -= damage;

            if (_currentHealth <= _maxHealth / 2 && _currentPhase < 1)
            {
                ChangePhase();
            }
        }
    }

    private void ChangePhase()
    {
        _currentPhase++;
        _spriteRenderer.sprite = _secondPhase;
        _regenerationActive = true;

        StartCoroutine(Regeneration());
    }

    private IEnumerator Regeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (_currentHealth < _maxHealth)
            {
                _currentHealth += 5;
            }
        }
    }
}