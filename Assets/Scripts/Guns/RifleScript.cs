using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleScript : MonoBehaviour, IItem, IGun
{
    [SerializeField] private TypeItems _typeItem;
    [SerializeField] private TypeGuns _typeGun;
    [SerializeField] private int _damage;
    [SerializeField] private float _shootFrequency;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private GameObject _bulletObject;

    public TypeItems TypeItems => _typeItem;
    public TypeGuns TypeGun => _typeGun;
    public int Damage => _damage;
    public float ShootFrequency => _shootFrequency;
    public float BulletSpeed => _bulletSpeed;
    public Transform ShootPoint => _shootPoint;
    public GameObject BulletObject => _bulletObject;

    private bool _canTake = false;
    private bool _isTaken = false;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && _isTaken)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.E) && _canTake && !_isTaken)
        {
        }
    }

    private void Shoot()
    {
        var bullet = Instantiate(BulletObject, ShootPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.forward * BulletSpeed);
        bullet.GetComponent<BulletScript>().BulletSpeed = BulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _canTake = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _canTake = false;
        }
    }
}