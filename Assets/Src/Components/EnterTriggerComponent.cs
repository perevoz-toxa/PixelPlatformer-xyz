using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Src.Components
{
    [RequireComponent(typeof(Collider2D))]

    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private EnterTriggerEvent _action;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(_tag))
            {
                _action?.Invoke(other.gameObject);
            }
        }
    }

    [Serializable]
    public class EnterTriggerEvent : UnityEvent<GameObject>
    {

    }
}
