using System;
using UnityEngine;

namespace Model.Definitions
{
    [CreateAssetMenu(fileName = "InventoryItems", menuName = "Defs/InventoryItems")]
    public class InventoryItemsDef : ScriptableObject
    {
        [SerializeField] private ItemDef[] items;

        public ItemDef Get(string id)
        {
            foreach (var itemDef in items)
            {
                if (itemDef.Id == id) return itemDef;
            }

            return default;
        }
        
#if UNITY_EDITOR
        public ItemDef[] itemsForEditor => items;
#endif
    }


    [Serializable]
    public struct ItemDef
    {
        [SerializeField] public string id;

        public string Id => id;

        public bool IsVoid => string.IsNullOrEmpty(id);
    }
}
