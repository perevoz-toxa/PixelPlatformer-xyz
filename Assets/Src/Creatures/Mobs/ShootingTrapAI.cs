using Components.ColliderBased;
using Components.GoBased;
using UnityEngine;
using Utils;

namespace Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;

        [Header("Melee Attack")] [SerializeField]
        private Cooldown _meleeCooldown;

        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;

        [Header("Range Attack")] [SerializeField]
        private Cooldown _rangeCooldown;

        [SerializeField] private SpawnComponent _rangeAttack;
        
        private Animator _animator;
        private static readonly int meleeKey = Animator.StringToHash("melee");
        private static readonly int hitKey = Animator.StringToHash("hit");
        private static readonly int rangeKey = Animator.StringToHash("range");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                if (_meleeCanAttack.IsTouchingLayer)
                {
                    if (_meleeCooldown.IsReady) MeleeAttack();
                    return;
                }

                if (_rangeCooldown.IsReady) RangeAttack();
            }
        }

        private void RangeAttack()
        {
            _animator.SetTrigger(rangeKey);
            _rangeCooldown.Reset();
        }

        private void MeleeAttack()
        {
            _meleeCooldown.Reset();
            _animator.SetTrigger(meleeKey);
        }

        public void OnMeleeAtack()
        {
            _meleeAttack.Check();
        }

        public void OnRangedAtack()
        {
            _rangeAttack.Spawn();
        }

        public void OnDamage()
        {
            _animator.SetTrigger(hitKey);
        }
    }
}