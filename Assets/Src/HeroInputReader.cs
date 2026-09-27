using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Src.Creatures;


namespace Assets.Src
{
    [RequireComponent(typeof(Hero))]
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;
        [SerializeField] private float _holdThreshold = 1f;

        private Coroutine _holdRoutine;

        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Interact();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Attack();
            }
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.started) 
            {
                _holdRoutine = StartCoroutine(HoldThrow());
            }
            else if (context.canceled) 
            {
                if (_holdRoutine == null) return; 
                StopCoroutine(_holdRoutine);
                _holdRoutine = null;
                _hero.Throw(); 
            }
        }

        private IEnumerator HoldThrow()
        {
            yield return new WaitForSeconds(_holdThreshold);
            _holdRoutine = null; 
            _hero.ThrowSeries(); 
        }
    }
}