using System;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;

        [SerializeField] private AudioSource music;
        [SerializeField] private AudioSource ui;
        [SerializeField] private AudioSource sfx;
        [SerializeField] private AudioSource sfx2;
        [SerializeField] private AudioSource sfx3;

        [SerializeField] private AudioClip[] clips;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }

            Destroy(gameObject);
        }

        public static void Play(int index, SourceType sourceType)
        {
            switch (sourceType)
            {
                case SourceType.Music:
                    _instance.music.clip = _instance.clips[index];
                    if (_instance.music.isPlaying)
                    {
                        _instance.music.Stop();
                    }

                    _instance.music.Play();
                    break;
                case SourceType.UI:
                    _instance.ui.clip = _instance.clips[index];
                    if (_instance.ui.isPlaying)
                    {
                        _instance.ui.Stop();
                    }

                    _instance.ui.Play();
                    break;
                case SourceType.SFX:
                    _instance.sfx.clip = _instance.clips[index];
                    if (_instance.sfx.isPlaying)
                    {
                        _instance.sfx.Stop();
                    }

                    _instance.sfx.Play();
                    break;
                case SourceType.SFX2:
                    _instance.sfx2.clip = _instance.clips[index];
                    if (_instance.sfx2.isPlaying)
                    {
                        _instance.sfx2.Stop();
                    }
                    
                    _instance.sfx2.Play();
                    break;
                case SourceType.SFX3:
                    _instance.sfx3.clip = _instance.clips[index];
                    if (_instance.sfx3.isPlaying)
                    {
                        _instance.sfx3.Stop();
                    }
                    
                    _instance.sfx3.Play();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sourceType), sourceType, null);
            }
        }
    }
}