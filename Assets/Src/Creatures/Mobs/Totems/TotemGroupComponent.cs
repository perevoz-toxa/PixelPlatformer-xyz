using System.Collections.Generic;
using Components.TransformHandle;
using UnityEngine;
using Utils;

namespace Creatures.Mobs.Totems
{
    [RequireComponent(typeof(TotemGroupStackComponent))]
    public class TotemGroupComponent : MonoBehaviour
    {
        [SerializeField] private Cooldown cooldown;
        private List<TotemSegmentComponent> segments = new List<TotemSegmentComponent>();
        private List<TotemSegmentComponent> detections = new List<TotemSegmentComponent>();
        

        private void Awake()
        {
            foreach (var segment in GetComponentsInChildren<TotemSegmentComponent>(true))
            {
                segments.Add(segment);
                segment.SubscribeOnDetect(() => OnSegmentDetected(segment));
            }
        }

        private void OnSegmentDetected(TotemSegmentComponent segment)
        {
            if (!detections.Contains(segment))
            {
                detections.Add(segment);
            }
        }

        private void Update()
        {
            if (!cooldown.IsReady) return;

            for (var i = detections.Count - 1; i >= 0; i--)
            {
                var segment = detections[i];

                // Уничтоженный сегмент — выкидываем из истории, смотрим предыдущего
                if (!segment)
                {
                    detections.RemoveAt(i);
                    continue;
                } 

                if (segment.IsCooldownReady())
                {
                    segment.Shoot();
                    cooldown.Reset();
                    detections.Clear();
                }

                break;
            }
        }
    }
}
