using System;
using System.Collections.Generic;
using Model.Definitions;
using UnityEngine;

namespace Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> inventory = new List<InventoryItemData>();

        public delegate void OnInventoryChanged(string id, int value);

        public OnInventoryChanged OnChanged;

        public void Add(string id, int value)
        {
            if (value <= 0) return;

            var itemDef = DefsFacade.Instance.Items.Get(id);
            if (itemDef.IsVoid) return;


            var item = GetItem(id);
            if (item == null)
            {
                item = new InventoryItemData(id);
                inventory.Add(item);
            }

            item.value += value;
            OnChanged?.Invoke(id, Count(id));
        }

        public void Remove(string id, int value)
        {
            var itemDef = DefsFacade.Instance.Items.Get(id);
            if (itemDef.IsVoid) return;

            var item = GetItem(id);
            if (item == null) return;

            item.value -= value;

            if (item.value <= 0)
                inventory.Remove(item);

            OnChanged?.Invoke(id, Count(id));
        }

        private InventoryItemData GetItem(string id)
        {
            foreach (var item in inventory)
            {
                if (item.id == id) return item;
            }

            return null;
        }

        public int Count(string id)
        {
            var count = 0;
            foreach (var item in inventory)
            {
                if (item.id == id)
                    count += item.value;
            }

            return count;
        }
    }

    [Serializable]
    internal class InventoryItemData
    {
        [InventoryId] public string id;
        public int value;

        public InventoryItemData(string id)
        {
            this.id = id;
        }
    }
}