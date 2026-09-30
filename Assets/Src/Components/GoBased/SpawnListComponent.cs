using System;
using System.Linq;
using UnityEngine;

namespace Assets.Src.Components
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private SpawnData[] _spawners;

        public void SpawnAll()
        {
            foreach (var spawner in _spawners)
            {
                spawner.Component.Spawn();
            }
        }

        public void Spawn(string id)
        {
            var spawner = _spawners.FirstOrDefault(element => element.Id == id);
            spawner?.Component.Spawn();
        }

        [Serializable]
        private class SpawnData
        {
            public string Id;
            public SpawnComponent Component;
        }
    }
}
