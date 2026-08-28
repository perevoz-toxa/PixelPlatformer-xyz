using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    [SerializeField] private int _damageValue;

    public void ApplyDamageToTarget(GameObject target)
    {
        if (target != null)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (target != null)
            {
                healthComponent.ApplyDamage(_damageValue);
            }
        }
    }
}
