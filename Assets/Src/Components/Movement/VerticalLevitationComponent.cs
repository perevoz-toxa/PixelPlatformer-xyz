using UnityEngine;

namespace Components.Movement
{
    public class VerticalLevitationComponent : LevitationComponent
    {
        protected override Vector2 CalculateOffset()
        {
            return new Vector2(
                0,
                Mathf.Sin(PhaseOffset + Time.time * Frequency) * Amplitude
            );
        }
    }
}