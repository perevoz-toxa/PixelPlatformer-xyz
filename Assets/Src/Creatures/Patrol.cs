using System.Collections;
using UnityEngine;

namespace Assets.Src.Creatures
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();

        private void Update()
        {
        }
    }
}
