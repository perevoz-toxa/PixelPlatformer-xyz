using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Src.Creatures
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CoinsCounter))]

    public class Creature : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _damageVelocity;
        [SerializeField] private int _damage;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerCheck _groundCheck;
    }
}