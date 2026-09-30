using UnityEngine;

namespace Assets.Src.Components
{
    public class CollectComponent : MonoBehaviour
    {
        [SerializeField] private int _value;

        public void Collect()
        {
            var counter = FindObjectOfType<CoinsCounter>();
            if (counter != null)
            {
                counter.Add(_value);
            }
        }
    }
}
