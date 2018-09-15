using UnityEngine;

namespace Assets.Scripts.Pacman
{
    /// <summary>
    /// Supergum.
    /// </summary>
    public class Supergum : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _eatSound;

        [SerializeField]
        private int _reward = 100;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                GameManager.Instance.AddScore(_reward);
                TerrainManager.Instance.OnGumEaten();
                SoundManager.Instance.PlaySoundSolo(_eatSound);
                Destroy(gameObject);
            }
        }
    }
}