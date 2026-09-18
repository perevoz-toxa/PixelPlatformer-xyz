using UnityEngine;

namespace Assets.Src.Components
{
    public class DoInteractionComponent : MonoBehaviour
    {
        public void DoInteraction(GameObject go)
        {
            var interactableComponent = go.GetComponent<InteractiveComponent>();
            if (interactableComponent != null)
            {
                interactableComponent.Interact();
            }
        }
    }
}
