using System;
using Components.Animation;
using Components.ColliderBased;
using UnityEngine;
using Utils;

namespace Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] public LayerCheck _vision;
        [SerializeField] private Cooldown _cooldown;
        [SerializeField] private SpriteAnimationComponent _animation;

        private void Update()
        {
            if (_vision.IsTouchingLayer && _cooldown.IsReady)
            {
                Shoot();
            }
        }
        public void Shoot()
        {
            _cooldown.Reset();
            _animation.SetClip("start-attack");
        }
    }
}
