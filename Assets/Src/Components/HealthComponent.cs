using UnityEngine;
using UnityEngine.Events;

namespace Assets.Src.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;

        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                Debug.Log($"Health: {_health}");
            }
        }

        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private UnityEvent _onDie;

        public void ModifyHealth(int value)
        {
            Health += value;
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
    }
}
