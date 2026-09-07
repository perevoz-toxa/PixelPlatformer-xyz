using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Assets.Src.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;
        [SerializeField] private float _alphaTime = 1;
        [SerializeField] private float _moveTime = 1;

        public void Teleport(GameObject target)
        {
            // target.transform.position = _destTransform.position;
            StartCoroutine(AnimateTeleport(target));
        }

        private IEnumerator AnimateTeleport(GameObject target)
        {
            var sprite = target.GetComponent<SpriteRenderer>();
            var playerInput = target.GetComponent<PlayerInput>();

            /** Locking Input */
            SetLockInput(playerInput, true);

            /** Lock Physics */
            SetLockPhysics(target, true);

            /** Fade out */
            yield return AlphaAnimation(sprite, 0);
            target.SetActive(false);

            /** Move to destination */
            yield return MoveAnimation(target);

            /** Fade in */
            target.SetActive(true);
            yield return AlphaAnimation(sprite, 1);

            /** Unlock Physics */
            SetLockPhysics(target, false);

            /** Unlocking Input */
            SetLockInput(playerInput, false);
        }

        private void SetLockInput(PlayerInput playerInput, bool isLocked)
        {
            if (playerInput != null)
            {
                playerInput.enabled = !isLocked;
            }
        }

        private void SetLockPhysics(GameObject target, bool isLocked)
        {
            if (target != null)
            {
                var rigidbody = target.GetComponent<Rigidbody2D>();
                if (rigidbody != null)
                {
                    rigidbody.bodyType = isLocked ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
                }
            }
        }

        private IEnumerator MoveAnimation(GameObject target)
        {
            var moveTime = 0f;
            while (moveTime < _moveTime)
            {
                moveTime += Time.deltaTime;
                var progress = moveTime / _moveTime;
                target.transform.position = Vector3.Lerp(target.transform.position, _destTransform.position, progress);

                yield return null;
            }
        }

        private IEnumerator AlphaAnimation(SpriteRenderer sprite, float destAlpha)
        {
            var time = 0f;
            var spriteAlpha = sprite.color.a;

            while (time < _alphaTime)
            {
                time += Time.deltaTime;
                var progress = time / _alphaTime;
                var tmpAlpha = Mathf.Lerp(spriteAlpha, destAlpha, progress);

                var color = sprite.color;
                color.a = tmpAlpha;
                sprite.color = color;
                yield return null;
            }
        }
    }
}
