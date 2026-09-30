using UnityEngine;
using Assets.Src.Creatures;

namespace Assets.Src.Components
{
    public class ArmHeroComponent : MonoBehaviour
    {

        public void ArmHero(GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            if (hero != null)
            {
                hero.ArmHero();
            }
        }
    }
}
