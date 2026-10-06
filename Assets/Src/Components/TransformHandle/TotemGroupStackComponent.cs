using System.Collections.Generic;
using Components.Animation;
using UnityEngine;

namespace Components.TransformHandle
{
    [ExecuteInEditMode]
    public class TotemGroupStackComponent : MonoBehaviour
    {
        private readonly List<Transform> _segments = new List<Transform>();

        private void OnEnable()
        {
            StackSegments();
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                StackSegments();
            }
        }

        private void OnTransformChildrenChanged()
        {
            StackSegments();
        }

        public void StackSegments()
        {
            _segments.Clear();
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                _segments.Add(transform.GetChild(i));
            }

            var currentY = 0f;
            for (var i = 0; i < _segments.Count; i++)
            {
                var segment = _segments[i];

                var position = segment.localPosition;
                position.x = GetSegmentXOffset(segment);
                position.z = 0f;
                position.y = currentY;   
                segment.localPosition = position;
                
                currentY += GetSegmentHeight(segment);
            }

            UpdateTopPosition();
        }

        private float GetSegmentHeight(Transform segment)
        {
            var animatorController = segment.GetComponent<TotemSegmentAnimatorController>();
            return animatorController != null ? animatorController.SpriteHeight : 1f;
        }

        private float GetSegmentXOffset(Transform segment)
        {
            var animatorController = segment.GetComponent<TotemSegmentAnimatorController>();
            return animatorController != null ? animatorController.XOffset : 0f;
        }

        private void UpdateTopPosition()
        {
            if (_segments.Count == 0) return;

            var topIndex = _segments.Count - 1;

            for (var i = 0; i < _segments.Count; i++)
            {
                var animatorController = _segments[i].GetComponent<TotemSegmentAnimatorController>();
                if (animatorController != null)
                {
                    animatorController.SetTopPosition(i == topIndex);
                }
            }
        }
    }
}
