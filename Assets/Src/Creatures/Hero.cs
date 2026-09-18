using UnityEngine;
using Assets.Src.Components;
using System;
using Assets.Src.Utils;
using UnityEditor.Animations;
using Assets.Src.Model;
using System.Collections;

namespace Assets.Src.Creatures
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CoinsCounter))]

    public class Hero : Creature
    {
        [Space]
        [Header("Hero Parameters")]
        [SerializeField] private float _interactionRadius;
        [SerializeField] private float _slamDownVelocity;

        [Header("Hero Advanced")]
        [SerializeField] private LayerCheck _wallCheck;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private AnimatorController _armedAnimatorController;
        [SerializeField] private AnimatorController _unarmedAnimatorController;

        [Header("Hero Particles")]
        [SerializeField] private ParticleSystem _coinsParticleSystem;

        private CoinsCounter _coinsCounter;
        private bool _allowDoubleJump;
        private Collider2D[] _interactionResult = new Collider2D[1];
        private bool _isOnWall;
        private GameSession _session;
        private float _defaultGravityScale;

        private static readonly int isOnWallKey = Animator.StringToHash("isOnWall");

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
            if (_coinsCounter.Count > 0)
            {
                SpawnCoins();
            }
        }

        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _interactionRadius,
                _interactionResult,
                _interactionLayer
                );

            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractiveComponent>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        public override void Attack()
        {
            if (!_session.Data.IsArmed) return;

            base.Attack();
        }

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
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
