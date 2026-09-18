using Assets.Src.Components;
using UnityEngine;

namespace Assets.Src.Creatures
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CoinsCounter))]

    public class Creature : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _speed;
        [SerializeField] protected float _jumpSpeed;
        [SerializeField] private float _damageVelocity;
        [SerializeField] private int _damage;

        [Header("Advanced")]
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private CheckCircleOverlap _attackRange;

        [Header("Particles")]
        [SerializeField] protected SpawnListComponent _particles;
        protected Rigidbody2D _rigidbody;
        protected Vector2 _direction;
        protected Animator _animator;
        protected bool _isGrounded;
        private bool _isJumping;


        private static readonly int isRunningKey = Animator.StringToHash("isRunning");
        private static readonly int isGroundedKey = Animator.StringToHash("isGrounded");
        private static readonly int verticalVelocityKey = Animator.StringToHash("verticalVelocity");
        private static readonly int hitKey = Animator.StringToHash("hit");
        private static readonly int attackKey = Animator.StringToHash("attack");

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        protected virtual void FixedUpdate()
        {
            if (_rigidbody.bodyType == RigidbodyType2D.Static) return;

            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);



            _animator.SetBool(isGroundedKey, _isGrounded);
            _animator.SetFloat(verticalVelocityKey, _rigidbody.velocity.y);
            _animator.SetBool(isRunningKey, _direction.x != 0);

            UpdateSpriteDirection();
        }

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            _isGrounded = _groundCheck.IsTouchingLayer;
        }

        protected virtual float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded)
            {
                _isJumping = false;
            }

            if (isJumpPressing)
            {
                _isJumping = true;
                var isFalling = _rigidbody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (_rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (_isGrounded)
            {
                yVelocity += _jumpSpeed;
                _particles.Spawn("Jump");
            }

            return yVelocity;
        }

        public virtual void TakeDamage()
        {
            _isJumping = false;
            _animator.SetTrigger(hitKey);
            _rigidbody.velocity = new Vector2(
                _rigidbody.velocity.x,
                _damageVelocity
                );
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public virtual void Attack()
        {
            _animator.SetTrigger(attackKey);
            _particles.Spawn("attack");
        }

        public void ApplyAttackEffect()
        {
            var gos = _attackRange.GetObjectsInRange();
            foreach (var go in gos)
            {
                var health = go.GetComponent<HealthComponent>();
                if (health != null)
                {
                    health.ModifyHealth(-_damage);
                }
            }
        }

        public void SpawnFootDust()
        {
            _particles.Spawn("footDust");
        }
    }
}
