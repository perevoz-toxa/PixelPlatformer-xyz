using System.Collections;
using UnityEngine;

namespace Assets.Src.Creatures
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private LayerCheck _forwardGroundCheck;
        [SerializeField] private float _turnCooldown = 0.3f;

        private Creature _creature;
        private Vector2 _direction = Vector2.right;
        private bool _hasTurned;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }

        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (!_forwardGroundCheck.IsTouchingLayer && !_hasTurned)
                {
                    _direction.x *= -1;
                    _creature.SetDirection(_direction);
                    _hasTurned = true;
                    yield return new WaitForSeconds(_turnCooldown);
                    continue;
                }

                if (_forwardGroundCheck.IsTouchingLayer)
                {
                    _hasTurned = false;
                }

                _creature.SetDirection(_direction);
                yield return null;
            }
        }
    }
}
