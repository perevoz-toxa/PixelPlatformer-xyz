using UnityEngine;

namespace Components.Interactions
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
