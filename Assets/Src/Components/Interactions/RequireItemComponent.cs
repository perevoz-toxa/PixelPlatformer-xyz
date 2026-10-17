using Model;
using Model.Data;
using Model.Definitions;
using UnityEngine;
using UnityEngine.Events;

namespace Components.Interactions
{
    public class RequireItemComponent : MonoBehaviour
    {
        [SerializeField] private InventoryItemData[] requiredItems;
        [SerializeField] private bool removeAfterUse;


        [SerializeField] private UnityEvent onSuccess;
        [SerializeField] private UnityEvent onFailure;

        public void Check()
        {
            var session = FindObjectOfType<GameSession>();
            var inventory = session.Data.Inventory;
            var areAllRequirementsMet = true;
            foreach (var item in requiredItems)
            {
                var itemCount = inventory.Count(item.id);
                if (itemCount < item.value)
                    areAllRequirementsMet = false;
            }
            
            if (areAllRequirementsMet)
            {
                if (removeAfterUse)
                {
                    foreach (var item in requiredItems)
                        inventory.Remove(item.id, item.value);
                }
                onSuccess?.Invoke();
            }
            else
            {
                onFailure?.Invoke();
            }
        }
    }
}
