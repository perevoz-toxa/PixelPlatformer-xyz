using UnityEngine;
using UnityEngine.Events;

namespace Components.Interactions
{
    public class InteractiveComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;

        public void Interact()
        {
            _action?.Invoke();
        }
    }
}
