using System;
using System.Collections;
using UnityEngine;

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

            /** Fade out */
            yield return SetAlpha(sprite, 0);

            /** Move to destination */
            var moveTime = 0f;
            while (moveTime < _moveTime)
            {
                moveTime += Time.deltaTime;
                var progress = moveTime / _moveTime;
                target.transform.position = Vector3.Lerp(target.transform.position, _destTransform.position, progress);

                yield return null;
            }

            /** Fade in */
            yield return SetAlpha(sprite, 1);
        }

        private IEnumerator SetAlpha(SpriteRenderer sprite, float destAlpha)
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
