using System;
using UnityEngine;

namespace Components.Audio
{
    public class PlaySoundsComponent : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioData[] sounds;

        public void Play(string id)
        {
            if (!source) return;

            foreach (var sound in sounds)
            {
                if (sound.Id != id) continue;

                source.PlayOneShot(sound.Clip);
                break;
            }
        }
    }

    [Serializable]
    public class AudioData
    {
        [SerializeField] private string id;
        [SerializeField] private AudioClip clip;

        public string Id => id;
        public AudioClip Clip => clip;
    }
}
