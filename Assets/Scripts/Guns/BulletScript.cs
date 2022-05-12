using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int Damage { get; set; }
    public float Speed { get; set; }
    public float LifeTime { get; set; }

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, LifeTime);
    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.right * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyScript enemyScript))
        {
            enemyScript.TakeHit(Damage);
            Destroy(Instantiate(Resources.Load<GameObject>("Particles/BloodParticleSystem"), collision.transform.position, Quaternion.identity), 0.75f);
            gameObject.SetActive(false);
            Destroy(gameObject, 1.5f);
        } 
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}