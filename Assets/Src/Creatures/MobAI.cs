using System;
using System.Collections;
using Assets.Src.Components;
using UnityEngine;

namespace Assets.Src.Creatures
{
    [RequireComponent(typeof(SpawnListComponent))]
    [RequireComponent(typeof(Creature))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Patrol))]
    public class MobAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;

        [Space] [Header("Parameters")] [SerializeField]
        private float _alarmDelay = 0.5f;

        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _missCooldown = 0.5f;

        private bool _isDead;

        private Coroutine _current;
        private GameObject _target;

        private Creature _creature;
        private SpawnListComponent _particles;
        private Animator _animator;
        private Patrol _patrol;

        private static readonly int isDeadKey = Animator.StringToHash("isDead");

        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }

        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        private IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }

                yield return null;
            }

            _creature.SetDirection(Vector2.zero);
            _particles.Spawn("miss");
            yield return new WaitForSeconds(_missCooldown);
            StartState(_patrol.DoPatrol());
        }

        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(GoToHero());
        }

        private void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(direction);
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }

        private IEnumerator AgroToHero()
        {
            LookAtHero();
            _particles.Spawn("exclamation");
            yield return new WaitForSeconds(_alarmDelay);

            StartState(GoToHero());
        }

        private void LookAtHero()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(Vector2.zero);
            _creature.UpdateSpriteDirection(direction);
        }

        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);

            if (_current != null)
            {
                StopCoroutine(_current);
            }

            _current = StartCoroutine(coroutine);
        }

        public void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;
            _target = go;

            StartState(AgroToHero());
        }

        public void OnDie()
        {
            _creature.SetDirection(Vector2.zero);
            _isDead = true;
            _animator.SetBool(isDeadKey, true);

            if (_current != null)
            {
                StopCoroutine(_current);
            }
        }
    }
}