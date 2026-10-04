using UnityEngine;

namespace Components.Movement
{
    public class GroupCircularLevitationComponent : MonoBehaviour
    {
        [SerializeField] private float radius;
        [SerializeField] private float speed = 1f;
        [SerializeField] private bool invertDirection;

        private Transform[] _children;

        private void Start()
        {
            _children = new Transform[transform.childCount];
            for (var i = 0; i < transform.childCount; i++)
            {
                _children[i] = transform.GetChild(i);
            }
        }

        private void Update()
        {
            if (_children.Length == 0) return;

            var angleStep = Mathf.PI * 2 / _children.Length;
            var direction = invertDirection ? -1f : 1f;
            var angle = Time.time * speed * direction;

            for (var i = 0; i < _children.Length; i++)
            {
                if (!_children[i]) continue;

                var childAngle = angle + i * angleStep;
                var x = Mathf.Cos(childAngle) * radius;
                var y = Mathf.Sin(childAngle) * radius;
                _children[i].localPosition = new Vector2(x, y);
            }
        }
    }
}