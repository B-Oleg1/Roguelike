using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GolemBossScript : MonoBehaviour
{
    [SerializeField] private Sprite _secondPhase;

    private EnemyScript _enemyScript;
    private SpriteRenderer _spriteRenderer;

    private int _maxHealth;
    private int _currentPhase;

    private void Start()
    {
        _enemyScript = GetComponent<EnemyScript>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _maxHealth = _enemyScript._health;
    }

    private void Update()
    {
        print(_maxHealth);
        if (_currentPhase < 1 && _enemyScript._health < _maxHealth / 2)
        {
            ChangePhase();
        }
    }

    private void ChangePhase()
    {
        _currentPhase++;

        StartCoroutine(Regeneration());
    }

    private IEnumerator Regeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (_enemyScript._health > 0 && _enemyScript._health < _maxHealth)
            {
                _enemyScript._health += 5;

                Instantiate(Resources.Load("Particles/HealthParticleSystem"), transform.position, Quaternion.identity);
            }
        }
    }
}