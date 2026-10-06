using Components.TransformHandle;
using UnityEngine;

namespace Components.Animation
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Animator))]
    public class TotemSegmentAnimatorController : MonoBehaviour
    {
        [Header("Параметры стака")] [SerializeField]
        private float spriteHeight = 0.67f;

        [SerializeField] private float xOffset;

        [Header("Контроллеры анимации")] [SerializeField]
        private RuntimeAnimatorController topPositionAnimatorController;

        [SerializeField] private RuntimeAnimatorController animatorController;

        private Animator _animator;

        public float SpriteHeight => spriteHeight;
        public float XOffset => xOffset;

        private void OnValidate()
        {
            // Высоту поменяли в инспекторе — просим группу сразу пересобрать стак
            var group = GetComponentInParent<TotemGroupStackComponent>();
            if (group != null)
            {
                group.StackSegments();
            }
        }

        public void SetTopPosition(bool isTop)
        {
            // Ленивая инициализация: группу могли вызвать раньше нашего Awake
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }

            if (_animator == null) return;

            _animator.runtimeAnimatorController = isTop
                ? topPositionAnimatorController
                : animatorController;
        }
    }
}
