using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Src
{
    public class CoinsCounter : MonoBehaviour
    {

        [SerializeField] private CoinsChangeEvent _onChange;

        private int _count;

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                _onChange?.Invoke(_count);
                Debug.Log($"Coins: {_count}");
            }
        }

        public void Add(int value)
        {
            Count += value;
        }
    }

    [Serializable]
    public class CoinsChangeEvent : UnityEvent<int>
    {

    }
}
