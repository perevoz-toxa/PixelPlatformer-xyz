using UnityEngine;

public class Coin : MonoBehaviour
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
