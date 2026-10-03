using UnityEngine;

namespace Components.Health
{
    public class HealthEffectComponent : MonoBehaviour
    {
        [SerializeField] private int _effectValue;

        public void ApplyEffectToTarget(GameObject target)
        {
            if (target != null)
            {
                var healthComponent = target.GetComponent<HealthComponent>();
                if (healthComponent != null)
                {
                    healthComponent.ModifyHealth(_effectValue);
                }
            }
        }
    }
}
