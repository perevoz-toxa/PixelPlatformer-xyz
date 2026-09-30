using UnityEngine;
using UnityEngine.Events;

namespace Assets.Src.Components
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
