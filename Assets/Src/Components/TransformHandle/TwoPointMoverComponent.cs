using System.Collections.Generic;
using UnityEngine;

namespace Assets.Src.Components
{
    public class TwoPointMoverComponent : MonoBehaviour
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private float _speed = 2f;

        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private float _elapsedTime;
        private Vector3 _previousPosition;
        private HashSet<Collider2D> _attachedObjects = new HashSet<Collider2D>();

        private void Start()
        {
            _startPosition = _startPoint.position;
            _endPosition = _endPoint.position;
            _elapsedTime = 0f;
            _previousPosition = transform.position;
        }

        private void Update()
        {
            float distance = Vector3.Distance(_startPosition, _endPosition);
            if (distance < 0.001f) return;

            _elapsedTime += Time.deltaTime * _speed;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.PingPong(_elapsedTime, 1f));

            transform.position = Vector3.Lerp(_startPosition, _endPosition, t);

            Vector3 deltaPosition = transform.position - _previousPosition;
            foreach (var obj in _attachedObjects)
            {
                obj.transform.position += deltaPosition;
            }

            _previousPosition = transform.position;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _attachedObjects.Add(collision.collider);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            _attachedObjects.Remove(collision.collider);
        }
    }
}
