using UnityEngine;
using Assets.Src.Components;
using System;
using Assets.Src.Utils;
using Assets.Src.Model;
using System.Collections;
using UnityEditor.Animations;

namespace Assets.Src.Creatures
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CoinsCounter))]
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

        [SerializeField] private AnimatorController _armedAnimatorController;
        [SerializeField] private AnimatorController _unarmedAnimatorController;
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        [Header("Hero Particles")] [SerializeField]
        private ParticleSystem _coinsParticleSystem;

        private CoinsCounter _coinsCounter;
        private bool _allowDoubleJump;
        private bool _isOnWall;
        private GameSession _session;
        private float _defaultGravityScale;

        private static readonly int isOnWallKey = Animator.StringToHash("isOnWall");
        private static readonly int throwKey = Animator.StringToHash("throw");

        protected override void Awake()
        {
            base.Awake();
            _coinsCounter = GetComponent<CoinsCounter>();
            _defaultGravityScale = _rigidbody.gravityScale;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            UpdateHeroWeapon();

            var healthComponent = GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.Health = _session.Data.Hp;
            }

            var coinsCounter = GetComponent<CoinsCounter>();
            if (coinsCounter != null)
            {
                coinsCounter.Count = _session.Data.Coins;
            }
        }

        protected override void Update()
        {
            base.Update();
            if (_wallCheck.IsTouchingLayer && _direction.x == transform.localScale.x)
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
            if (_allowDoubleJump && !_isGrounded)
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
            _animator.runtimeAnimatorController = _session.Data.IsArmed
                ? _armedAnimatorController
                : _unarmedAnimatorController;
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Math.Min(_coinsCounter.Count, 5);
            _coinsCounter.Count -= numCoinsToDispose;

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

        public override void TakeDamage()
        {
            base.TakeDamage();
            _rigidbody.velocity = new Vector2(
                _rigidbody.velocity.x,
                _damageVelocity
            );
            if (_coinsCounter.Count > 0)
            {
                SpawnCoins();
            }
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        public override void Attack()
        {
            if (!_session.Data.IsArmed) return;

            base.Attack();
        }

        public void Throw()
        {
            if (_session.Data.HeroSwords <= 1 ||
                !_throwCooldown.IsReady) return;

            _animator.SetTrigger(throwKey);
            _throwCooldown.Reset();
        }

        [ContextMenu("Throw Series")]
        public void ThrowSeries()
        {
            if (_session.Data.HeroSwords <= 1 ||
                !_throwCooldown.IsReady) return;

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
            return Mathf.Min(_session.Data.HeroSwords - 1, 3);
        }

        public void OnDoThrow()
        {
            _session.Data.HeroSwords = Mathf.Max(1, _session.Data.HeroSwords - 1);
            _particles.Spawn("throw");
        }

        public void ArmHero()
        {
            _session.Data.HeroSwords++;
            UpdateHeroWeapon();
        }

        public void OnHealthChange(int hp)
        {
            _session.Data.Hp = hp;
        }

        public void OnCoinsChange(int count)
        {
            _session.Data.Coins = count;
        }
    }
}