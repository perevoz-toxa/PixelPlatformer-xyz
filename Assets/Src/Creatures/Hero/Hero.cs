using UnityEngine;
using System;
using System.Collections;
using Components.ColliderBased;
using Components.GoBased;
using Components.Health;
using Model;
using Utils;

namespace Creatures.Hero
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(SpawnListComponent))]
    [RequireComponent(typeof(HealthComponent))]
    public class Hero : Creature
    {
        [Space] [Header("Hero Parameters")] [SerializeField]
        private float _interactionRadius;

        [SerializeField] private float _slamDownVelocity;
        [SerializeField] private float _damageVelocity;
        [SerializeField] private float _throwSeriesInterval = 0.2f;
        [SerializeField] private Cooldown _throwCooldown;

        [Header("Hero Advanced")] [SerializeField]
        private LayerCheck _wallCheck;

        [SerializeField] private RuntimeAnimatorController _armedAnimatorController;
        [SerializeField] private RuntimeAnimatorController _unarmedAnimatorController;
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        [Header("Hero Particles")] [SerializeField]
        private ParticleSystem _coinsParticleSystem;

        private bool _allowDoubleJump;
        private bool _isOnWall;
        private GameSession _session;
        private float _defaultGravityScale;

        private static readonly int isOnWallKey = Animator.StringToHash("isOnWall");
        private static readonly int throwKey = Animator.StringToHash("throw");

        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count("Sword");

        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = _rigidbody.gravityScale;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            UpdateHeroWeapon();
            var inventory = _session.Data.Inventory;
            inventory.OnChanged += OnInventoryChanged;

            var healthComponent = GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.Health = _session.Data.Hp;
            }
        }

        private void OnDestroy()
        {
            var inventory = _session.Data.Inventory;
            inventory.OnChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == "Sword")
                UpdateHeroWeapon();
        }

        protected override void Update()
        {
            base.Update();

            var moveToSameDirection = _direction.x * transform.localScale.x > 0;
            if (_wallCheck.IsTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                _rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                _rigidbody.gravityScale = _defaultGravityScale;
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _animator.SetBool(isOnWallKey, _isOnWall);
        }

        protected override float CalculateYVelocity()
        {
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded || _isOnWall)
            {
                _allowDoubleJump = true;
            }

            if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (_allowDoubleJump && !_isGrounded && !_isOnWall)
            {
                _particles.Spawn("jump");
                _allowDoubleJump = false;
                return _jumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(_groundLayer))
            {
                var contact = collision.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _particles.Spawn("slamDown");
                }
            }
        }

        private void UpdateHeroWeapon()
        {
            _animator.runtimeAnimatorController = IsArmed()
                ? _armedAnimatorController
                : _unarmedAnimatorController;
        }

        private void SpawnCoins()
        {
            var inventory = _session.Data.Inventory;
            var numCoinsToDispose = Math.Min(CoinCount, 5);

            inventory.Remove("Coin", numCoinsToDispose);

            var burst = _coinsParticleSystem.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _coinsParticleSystem.emission.SetBurst(0, burst);

            _coinsParticleSystem.gameObject.SetActive(true);
            _coinsParticleSystem.Play();
            StartCoroutine(DisableCoinsParticleAfterFinish());
        }

        private IEnumerator DisableCoinsParticleAfterFinish()
        {
            yield return new WaitForSeconds(_coinsParticleSystem.main.duration);
            _coinsParticleSystem.gameObject.SetActive(false);
        }

        private bool IsArmed()
        {
            return SwordCount > 0;
        }

        public override void TakeDamage()
        {
            base.TakeDamage();

            if (CoinCount > 0)
                SpawnCoins();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        public void AddInInventory(string id, int value)
        {
            Debug.Log($"Added to inventory {id}: {value}");

            var inventory = _session.Data.Inventory;
            inventory.Add(id, value);
        }

        public override void Attack()
        {
            if (!IsArmed()) return;

            base.Attack();
        }

        public void Throw()
        {
            if (SwordCount <= 1 || !_throwCooldown.IsReady) return;

            _animator.SetTrigger(throwKey);
            _throwCooldown.Reset();
        }

        [ContextMenu("Throw Series")]
        public void ThrowSeries()
        {
            var inventory = _session.Data.Inventory;
            var swordsCount = inventory.Count("Sword");

            if (swordsCount <= 1 || !_throwCooldown.IsReady) return;

            StartCoroutine(ThrowSeriesCoroutine());
        }

        private IEnumerator ThrowSeriesCoroutine()
        {
            var swordsCount = CalculateSwordsForThrowSeries();
            for (var i = swordsCount; i > 0; i--)
            {
                _animator.SetTrigger(throwKey);
                _throwCooldown.Reset();
                yield return new WaitForSeconds(_throwSeriesInterval);
            }
        }

        private int CalculateSwordsForThrowSeries()
        {
            var inventory = _session.Data.Inventory;
            var swordsCount = inventory.Count("Sword");

            return Mathf.Min(swordsCount - 1, 3);
        }

        public void OnDoThrow()
        {
            var inventory = _session.Data.Inventory;

            inventory.Remove("Sword", 1);
            _particles.Spawn("throw");
        }

        public void OnHealthChange(int hp)
        {
            _session.Data.Hp = hp;
        }
    }
}
