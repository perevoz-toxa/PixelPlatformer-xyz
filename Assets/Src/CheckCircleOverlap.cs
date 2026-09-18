using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Src
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private OnOverlapEvent _onOverlap;
        [SerializeField] private string[] _tags;
        [SerializeField] private LayerMask _mask;
        private readonly Collider2D[] _interactionResult = new Collider2D[25];

        internal void Check()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                            transform.position,
                            _radius,
                            _interactionResult,
                            _mask
                        );

            for (var i = 0; i < size; i++)
            {
                var overlapResult = _interactionResult[i];
                var isInTag = _tags.Any(tag => overlapResult.CompareTag(tag));
                if (isInTag)
                {
                    _onOverlap?.Invoke(overlapResult.gameObject);
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = Utils.HandlesUtils.TransparentGreen;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
#endif

        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject>
        {

        }
    }
}
