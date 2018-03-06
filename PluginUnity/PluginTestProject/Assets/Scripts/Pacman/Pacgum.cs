using UnityEngine;

namespace Assets.Scripts.Pacman
{
    public class Pacgum : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _eatSound;

        [SerializeField]
        private int _reward = 100;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.tag == "Player" /*&& Vector3.Distance(transform.position, other.transform.position) < 0.01f*/)
            {
                GameManager.Instance.AddScore(_reward);
                TerrainManager.Instance.OnGumEaten();
                _audioSource.PlayOneShot(_eatSound);
                Destroy(gameObject);
            }
        }
    }
}