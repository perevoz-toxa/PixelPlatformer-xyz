using System.Collections.Generic;
using System.Linq;
using Components.Health;
using UnityEngine;
using Utils;

namespace Creatures.Mobs
{
    public class TotemTower : MonoBehaviour
    {
        [SerializeField] private List<ShootingTrapAI> _traps;
        [SerializeField] private Cooldown _cooldown;

        private int _currentTrapIndex;

        private void Start()
        {
            foreach (var trap in _traps)
            {
                trap.enabled = false;
                var healthComponent = trap.GetComponent<HealthComponent>();
                healthComponent._onDie.AddListener(() => OnTrapDead(trap));
            }
        }

        private void OnTrapDead(ShootingTrapAI trap)
        {
            var index = _traps.IndexOf(trap);
            _traps.Remove(trap);
            if (index < _currentTrapIndex)
            {
                _currentTrapIndex--;
            }
        }

        private void Update()
        {
            if (_traps.Count <= 0)
            {
                enabled = false;
                Destroy(gameObject, 1f);
            }
            var hasAnyTarget = _traps.Any(trap => trap._vision.IsTouchingLayer);
            if (hasAnyTarget)
            {
                if (_cooldown.IsReady)
                {
                    _traps[_currentTrapIndex].Shoot();
                    _cooldown.Reset();
                    _currentTrapIndex = (int) Mathf.Repeat(_currentTrapIndex + 1, _traps.Count);
                }
            }
        }
    }
}
