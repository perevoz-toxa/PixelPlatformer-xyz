using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Src
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private string _tag;
        private readonly Collider2D[] _interactionResult = new Collider2D[5];

        public GameObject[] GetObjectsInRange()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _radius,
                _interactionResult
            );

            var overlaps = new List<GameObject>();
            for (var i = 0; i < size; i++)
            {
                if (_interactionResult[i].gameObject.CompareTag(_tag))
                {
                    overlaps.Add(_interactionResult[i].gameObject);
                }
            }

            return overlaps.ToArray();
        }

        private void OnDrawGizmosSelected()
        {
            Handles.color = Utils.HandlesUtils.TransparentGreen;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
    }
}
