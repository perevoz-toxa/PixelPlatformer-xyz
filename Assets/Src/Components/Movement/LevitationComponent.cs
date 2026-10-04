using UnityEngine;

namespace Components.Movement
{
    public abstract class LevitationComponent : MonoBehaviour
    {
        [SerializeField] private float _frequency = 1f;
        [SerializeField] private float _amplitude = 1f;
        [SerializeField] private bool _randomize = true;

        private Vector3 _originalPosition;
        private Rigidbody2D _rigidbody;
        private float _phaseOffset;

        protected float Frequency => _frequency;
        protected float Amplitude => _amplitude;
        protected float PhaseOffset => _phaseOffset;
        protected Rigidbody2D Rigidbody => _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _originalPosition = _rigidbody.position;

            if (_randomize)
            {
                _phaseOffset = Random.value * Mathf.PI * 2;
            }
        }

        private void Update()
        {
            var offset = CalculateOffset();
            var position = _originalPosition + new Vector3(offset.x, offset.y, 0);
            _rigidbody.MovePosition(position);
        }
        
        protected abstract Vector2 CalculateOffset();
    }
}
