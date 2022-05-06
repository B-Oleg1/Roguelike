using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IGun
    {
        public TypeGuns TypeGun { get; }
        public int MaxBullets { get; }
        public int Damage { get; }
        public int PriceBullet { get; }
        public float ShootFrequency { get; }
        public float TimeReload { get; }
        public float BulletSpeed { get; }
        public float LifeTime { get; }
        public float Spread { get; }
        public Transform ShootPoint { get; }
        public GameObject BulletObject { get; }
        public AudioSource AudioSource { get; }
        public AudioClip ShootAudioClip { get; }
    }
}
