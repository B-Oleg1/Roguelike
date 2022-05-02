using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IGun
    {
        public TypeGuns TypeGun { get; }
        public int Damage { get; }
        public float ShootFrequency { get; }
        public float BulletSpeed { get; }
        public Transform ShootPoint { get; }
        public GameObject BulletObject { get; }
    }
}
