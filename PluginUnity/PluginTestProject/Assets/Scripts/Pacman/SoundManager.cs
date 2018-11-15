using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pacman
{
    /// <summary>
    /// Manages the sound in the game.
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        /// <summary>
        /// Sound singleton instance.
        /// </summary>
        public static SoundManager Instance { get; private set; }

        [SerializeField]
        private AudioClip _soundStartGame;

        [SerializeField]
        private AudioClip _soundDeath;

        private AudioSource _audioSource;
        private Queue<AudioClip> _queuedClips = new Queue<AudioClip>();

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        /// <summary>
        /// Subscribes to the game state changing event.
        /// </summary>
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            GameManager.Instance.OnGameStateChange += GameStateChanged;
            StartCoroutine(SoundRoutine());
        }

        /// <summary>
        /// Plays the game starting sound if needed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameStateChanged(object sender, System.EventArgs e)
        {
            var ev = e as GameStateEvent;

            if (ev.CurrentState == GameManager.GameState.Warmup && ev.PreviousState == GameManager.GameState.Menu)
            {
                _audioSource.clip = _soundStartGame;
                _audioSource.Play();
            }
        }

        /// <summary>
        /// Unsubscribes to the game state change event to avoid leaks.
        /// </summary>
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
                if (_queuedClips.Count > 0)
                {
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