using UnityEngine;

namespace Components.Movement
{
    public class CircularLevitationComponent : LevitationComponent
    {
        [SerializeField] private bool _invertDirection = false;

        protected override Vector2 CalculateOffset()
        {
            var direction = _invertDirection ? -1f : 1f;
            var angle = PhaseOffset + Time.time * Frequency * direction;
            return new Vector2(
                Mathf.Cos(angle) * Amplitude,
                Mathf.Sin(angle) * Amplitude
            );
        }
    }
}