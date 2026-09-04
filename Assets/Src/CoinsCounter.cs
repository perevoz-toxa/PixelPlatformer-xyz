using UnityEngine;

namespace Assets.Src
{
    public class CoinsCounter : MonoBehaviour
    {
        private int _count;

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                Debug.Log($"Coins: {_count}");
            }
        }

        public void Add(int value)
        {
            Count += value;
        }
    }
}
