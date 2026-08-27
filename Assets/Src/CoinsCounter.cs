using UnityEngine;

public class CoinsCounter : MonoBehaviour
{
    public int Count { get; private set; } = 0;

    public void Add(int value)
    {
        Count += value;
        Debug.Log($"Coins: {Count}");
    }
}
