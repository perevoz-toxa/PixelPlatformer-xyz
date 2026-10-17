using Creatures.Hero;
using Model.Definitions;
using UnityEngine;

namespace Components.Collectables
{
    public class CollectComponent : MonoBehaviour
    {
        [InventoryId][SerializeField] private string itemName;
        [SerializeField] private int value;

        public void Collect()
        {
            var hero = FindObjectOfType<Hero>();
            if (hero != null)
            {
                hero.AddInInventory(itemName, value);
            }
        }
    }
}
