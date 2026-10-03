using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Components.GoBased
{
    public class RandomSpawner : MonoBehaviour
    {
        [Header("Spawn bound:")] [SerializeField]
        private float _sectorAngle = 60f;

        [SerializeField] private float _sectorRotation;

        [SerializeField] private float _waitTime = 0.1f;
        [SerializeField] private float _speed = 6;

        [SerializeField] private UnityEvent _onSpawned;

        private Coroutine _routine;

        public void StartDrop(GameObject[] items)
        {
            TryStopRoutine();

            if (!enabled) return;

            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }

            _routine = StartCoroutine(StartSpawn(items));
        }

        private IEnumerator StartSpawn(GameObject[] particles)
        {
            for (var i = 0; i < particles.Length; i++)
            {
                Spawn(particles[i]);
                yield return new WaitForSeconds(_waitTime);
            }

            _onSpawned?.Invoke();
        }

        [ContextMenu("SpawnOne")]
        private void Spawn(GameObject particle)
        {
            var instance = Instantiate(particle, transform.position, Quaternion.identity);
            var rigidBody = instance.GetComponent<Rigidbody2D>();

            var randomAngle = Random.Range(0, _sectorAngle);
            var forceVector = AngleToVectorInSector(randomAngle);
            rigidBody.AddForce(forceVector * _speed, ForceMode2D.Impulse);
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var position = transform.position;

            var middleAngleDelta = (180 - _sectorRotation - _sectorAngle) / 2;
            var rightBound = GetUnitOnCircle(middleAngleDelta);
            Handles.DrawLine(position, position + rightBound);

            var leftBound = GetUnitOnCircle(middleAngleDelta + _sectorRotation);
            Handles.DrawLine(position, position + leftBound);
            Handles.DrawWireArc(position, Vector3.forward, rightBound, _sectorAngle, 2);

            Handles.color = new Color(1f, 1f, 1f, 0.1f);
            Handles.DrawSolidArc(position, Vector3.forward, rightBound, _sectorAngle, 2);
        }
#endif
        private Vector2 AngleToVectorInSector(float angle)
        {
            var angleMiddleDelta = (180 - _sectorRotation - _sectorAngle) / 2;
            return GetUnitOnCircle(angle + angleMiddleDelta);
        }

        private Vector3 GetUnitOnCircle(float angleDegrees)
        {
            var angleRadians = angleDegrees * Mathf.PI / 180f;

            var x = Mathf.Cos(angleRadians);
            var y = Mathf.Sin(angleRadians);

            return new Vector3(x, y, 0);
        }

        private void TryStopRoutine()
        {
            if (_routine != null)
                StopCoroutine(_routine);
        }
    }
}
