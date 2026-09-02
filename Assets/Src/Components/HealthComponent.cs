using UnityEngine;
using UnityEngine.Events;

namespace Assets.Src.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private UnityEvent _onDie;

        public void ModifyHealth(int value)
        {
            _health += value;
            _onHeal?.Invoke();
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
            else
            {
                if (value < 0)
                {
                    _onDamage?.Invoke();
                }
                else if (value > 0)
                {
                    _onHeal?.Invoke();
                }
            }
        }

        public void PrintHealth()
        {
            Debug.Log($"Health: {_health}");
        }
    }
}
