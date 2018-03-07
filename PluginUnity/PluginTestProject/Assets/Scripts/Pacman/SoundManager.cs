using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pacman
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [SerializeField]
        private AudioClip _soundStartGame;

        [SerializeField]
        private AudioClip _soundDeath;

        private AudioSource _audioSource;
        private Queue<AudioClip> _queuedClips = new Queue<AudioClip>();

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            GameManager.Instance.OnGameStateChange += GameStateChanged;
            StartCoroutine(SoundRoutine());
        }

        private void GameStateChanged(object sender, System.EventArgs e)
        {
            var ev = e as GameStateEvent;

            if (ev.CurrentState == GameManager.GameState.Warmup && ev.PreviousState == GameManager.GameState.Menu)
            {
                _audioSource.clip = _soundStartGame;
                _audioSource.Play();
            }
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnGameStateChange -= GameStateChanged;
        }

        public void PlayOneShot(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }

        internal void PlayDeathSound()
        {
            _audioSource.clip = _soundDeath;
            _audioSource.Play();
        }

        internal void PlaySoundQueued(AudioClip clip)
        {
            if (_queuedClips.Count >= 1)
                return;
            _queuedClips.Enqueue(clip);
        }

        internal void PlaySoundSolo(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        internal void StopSounds()
        {
            _audioSource.Stop();
        }

        private IEnumerator SoundRoutine()
        {
            while (true)
            {
                //yield return new WaitUntil(() => !_audioSource.isPlaying);
                if (_queuedClips.Count > 0)
                {
                    //_audioSource.clip = _queuedClips.Dequeue();
                    var clip = _queuedClips.Dequeue();
                    _audioSource.PlayOneShot(clip);
                    yield return new WaitForSeconds(clip.length);
                }
                else
                {
                    yield return new WaitUntil(() => _queuedClips.Count > 0);
                }
            }
        }
    }
}