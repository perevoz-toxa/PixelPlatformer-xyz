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

            if (itemDef.IsStackable)
            {
                AddToStack(id, value);
            }
            else
            {
                AddNonStack(id, value);
            }


            OnChanged?.Invoke(id, Count(id));
        }

        private void AddNonStack(string id, int value)
        {
            var itemsLasts = DefsFacade.Instance.Player.InventorySize - inventory.Count;
            value = Mathf.Min(itemsLasts, value);
            for (int i = 0; i < value; i++)
            {
                var item = new InventoryItemData(id) { value = 1 };
                inventory.Add(item);
            }
        }

        private void AddToStack(string id, int value)
        {
            var isFull = inventory.Count >= DefsFacade.Instance.Player.InventorySize;
            var item = GetItem(id);
            if (item == null)
            {
                if (isFull) return;

                item = new InventoryItemData(id);
                inventory.Add(item);
            }

            item.value += value;
        }

        public void Remove(string id, int value)
        {
            var itemDef = DefsFacade.Instance.Items.Get(id);
            if (itemDef.IsVoid) return;

            if (itemDef.IsStackable)
            {
                RemoveFromStack(id, value);
            }
            else
            {
                RemoveNonStack(id, value);
            }

            OnChanged?.Invoke(id, Count(id));
        }

        private void RemoveFromStack(string id, int value)
        {
            var item = GetItem(id);
            if (item == null) return;

            item.value -= value;

            if (item.value <= 0)
                inventory.Remove(item);
        }

        private void RemoveNonStack(string id, int value)
        {
            for (int i = 0; i < value; i++)
            {
                var item = GetItem(id);
                if (item == null) return;

                inventory.Remove(item);
            }
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