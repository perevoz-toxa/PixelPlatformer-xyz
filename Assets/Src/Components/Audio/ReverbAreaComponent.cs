using Creatures.Hero;
using UnityEngine;
using UnityEngine.Audio;

namespace Components.Audio
{
    [RequireComponent(typeof(Collider2D))]
    public class ReverbAreaComponent : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private string _reverbParameterName = "ReverbWet";
        [SerializeField] private float _wetInside = 0f;
        [SerializeField] private float _wetOutside = -80f;

        private Hero _hero;

        private void Awake()
        {
            _hero = FindObjectOfType<Hero>();
            _audioMixer.SetFloat(_reverbParameterName, _wetOutside);
        }

        private void Update()
        {
            var isInside = _collider.OverlapPoint(_hero.transform.position);
            var value = isInside ? _wetInside : _wetOutside;
            _audioMixer.SetFloat(_reverbParameterName, value);
        }
    }
}
