using System;
using UnityEngine;

public class HealthEffectComponent : MonoBehaviour
{
    [SerializeField] private int _effectValue;

    public void ApplyEffectToTarget(GameObject target)
    {
        if (target != null)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (target != null)
            {
                healthComponent.ModifyHealth(_effectValue);
            }
        }
    }
}
