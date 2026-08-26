using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Hero))]
public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private Hero _hero;

    private void Update()
    {

    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _hero.SetDirection(direction);
    }

    public void OnSaySomething(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.SaySomething();
        }
    }
}
