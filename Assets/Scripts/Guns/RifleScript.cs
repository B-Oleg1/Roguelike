using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleScript : MonoBehaviour, IItem, IGun
{
    [SerializeField] private TypeItems _typeItem;
    [SerializeField] private RarityItems _rarityItem;
    [SerializeField] private TypeGuns _typeGun;
    [SerializeField] private int _maxBullets;
    [SerializeField] private int _damage;
    [SerializeField] private int _priceBullet;
    [SerializeField] private float _shootFrequency;
    [SerializeField] private float _timeReload;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _spread;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private GameObject _bulletObject;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _shootAudioClip;

    public TypeItems TypeItem => _typeItem;
    public RarityItems RarityItem => _rarityItem;
    public TypeGuns TypeGun => _typeGun;
    public int MaxBullets => _maxBullets;
    public int Damage => _damage;
    public int PriceBullet => _priceBullet;
    public float ShootFrequency => _shootFrequency;
    public float TimeReload => _timeReload;
    public float BulletSpeed => _bulletSpeed;
    public float LifeTime => _lifeTime;
    public float Spread => _spread;
    public Transform ShootPoint => _shootPoint;
    public GameObject BulletObject => _bulletObject;
    public AudioSource AudioSource => _audioSource;
    public AudioClip ShootAudioClip => _shootAudioClip;

    private int _quantityBullets;
    private float _shootFreq = 0;
    private float _reloadTime = 0;

    private void Start()
    {
        _quantityBullets = MaxBullets;
    }

    private void Update()
    {
        if (_shootFreq > 0)
        {
            _shootFreq -= Time.deltaTime;
        }
        if (_reloadTime > 0)
        {
            _reloadTime -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Mouse0) && _shootFreq <= 0 && _reloadTime <= 0 && _quantityBullets > 0 && PlayerInfoScript.Instance.Energy >= _priceBullet)
        {
            Shoot();
        }
        else if (_quantityBullets <= 0)
        {
            _reloadTime = TimeReload;
            _quantityBullets = MaxBullets;
        }
    }

    private void Shoot()
    {
        _shootFreq = ShootFrequency;
        _quantityBullets--;

        PlayerInfoScript.Instance.UpdateEnergy(-PriceBullet);

        AudioSource.clip = ShootAudioClip;
        AudioSource.Play();

        Vector2 direction = ShootPoint.position - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        Vector3 targetRotation = new Vector3(0, 0, angle);

        var bullet = Instantiate(BulletObject, new Vector2(ShootPoint.position.x, ShootPoint.position.y + 0.1f), Quaternion.Euler(targetRotation + new Vector3(0, 0, Random.Range(-Spread, Spread))));
        
        bullet.GetComponent<BulletScript>().LifeTime = LifeTime;
        bullet.GetComponent<BulletScript>().Speed = BulletSpeed;
        bullet.GetComponent<BulletScript>().Damage = Damage;
    }
}