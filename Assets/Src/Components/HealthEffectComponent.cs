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
                if (_effectValue < 0)
                {
                    healthComponent.ApplyDamage(Math.Abs(_effectValue));
                }
                else
                {
                    healthComponent.ApplyHeal(_effectValue);
                }
            }
        }
    }
}
