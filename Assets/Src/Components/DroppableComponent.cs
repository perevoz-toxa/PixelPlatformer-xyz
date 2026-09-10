using UnityEngine;

public class DroppableComponent : MonoBehaviour
{
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private DroppableProp[] _droppableProps;

    public void SpawnDroppable()
    {
        if (_targetPosition == null)
            return;

        foreach (DroppableProp prop in _droppableProps)
        {
            if (prop._prefab == null)
                continue;

            int spawnCount = Random.Range(1, prop._maxSpawnCount + 1);

            for (int i = 0; i < spawnCount; i++)
            {
                float randomValue = Random.Range(0f, 100f);
                if (randomValue <= prop._dropChancePercent)
                {
                    Vector3 spawnPosition = _targetPosition.position
                        + new Vector3(Random.Range(-0.5f, 0.5f), 0.1f, 0);
                    Instantiate(prop._prefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }
}

[System.Serializable]
public class DroppableProp
{
    [SerializeField] public GameObject _prefab;
    [SerializeField] public int _dropChancePercent;
    [SerializeField] public int _maxSpawnCount;

}