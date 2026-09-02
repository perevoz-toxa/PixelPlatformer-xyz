using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;


namespace Assets.Src
{
    [RequireComponent(typeof(SpriteRenderer))]

    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private int _frameRate;
        [SerializeField] private List<SpriteClip> _clips;
        [SerializeField] private string _currentClipName;
        [SerializeField] private UnityEvent _onComplete;

        private SpriteRenderer _renderer;
        private float _secondsPerFrame;
        private int _currentSpriteIndex;
        private float _nextFrameTime;
        private SpriteClip _currentClip;

        private bool _isPlaying = true;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _secondsPerFrame = 1f / _frameRate;
            _nextFrameTime = Time.time + _secondsPerFrame;
            SetClip(_currentClipName);
        }

        private void Update()
        {
            if (!_isPlaying || _nextFrameTime > Time.time) return;

            if (_currentSpriteIndex >= _currentClip.Sprites.Length)
            {
                if (_currentClip.Loop)
                {
                    _currentSpriteIndex = 0;
                }
                else
                {
                    if (_currentClip.AllowNext)
                    {
                        int currentIndex = _clips.IndexOf(_currentClip);
                        if (currentIndex >= 0 && currentIndex < _clips.Count - 1)
                        {
                            SetClip(_clips[currentIndex + 1].Name);
                        }
                    }
                    else
                    {
                        _isPlaying = false;
                        _onComplete?.Invoke();
                        return;
                    }
                }
            }

            _renderer.sprite = _currentClip.Sprites[_currentSpriteIndex];
            _nextFrameTime += _secondsPerFrame;
            _currentSpriteIndex++;
        }

        public void SetClip(string clipName)
        {
            _currentClip = _clips.Find(c => c.Name == clipName);
            if (_currentClip == null)
            {
                Debug.LogWarning($"Clip '{clipName}' not found!");
                return;
            }
            _currentSpriteIndex = 0;
            _isPlaying = true;
            _nextFrameTime = Time.time + _secondsPerFrame;
            _renderer.sprite = _currentClip.Sprites[0];
        }
    }

    [Serializable]
    public class SpriteClip
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _allowNext;

        public string Name => _name;
        public Sprite[] Sprites => _sprites;
        public bool Loop => _loop;
        public bool AllowNext => _allowNext;
    }
}
