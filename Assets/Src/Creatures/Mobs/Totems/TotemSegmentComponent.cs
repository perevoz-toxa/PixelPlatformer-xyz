using System;
using Components.Animation;
using Components.ColliderBased;
using Components.GoBased;
using UnityEngine;
using Utils;

namespace Creatures.Mobs.Totems
{
    [RequireComponent(typeof(TotemSegmentAnimatorController))]
    [RequireComponent(typeof(Animator))]
    public class TotemSegmentComponent : MonoBehaviour
    {
        [SerializeField] public LayerCheck vision;
        [SerializeField] private SpawnComponent shootPosition;
        [SerializeField] private Cooldown cooldown;

        private event Action OnDetect;
        private Animator _animator;
        
        private static readonly int HitKey = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        private void Update()
        {
            if (vision.IsTouchingLayer)
            {
                OnDetect?.Invoke();
            }
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Shoot()
        {
            _animator.SetTrigger(AttackKey);
            cooldown.Reset();
        }

        public void OnShoot()
        {
            shootPosition.Spawn();
        }

        public bool IsCooldownReady()
        {
            return cooldown.IsReady;
        }

        public void SubscribeOnDetect(Action onDetect)
        {
            OnDetect += onDetect;
        }

        public void OnDamage()
        {
            _animator.SetTrigger(HitKey);
        }
    }
}
