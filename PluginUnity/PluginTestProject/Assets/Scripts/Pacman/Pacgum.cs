using UnityEngine;

namespace Assets.Scripts.Pacman
{
    /// <summary>
    /// Represents a pacgum.
    /// </summary>
    public class Pacgum : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _eatSound;

        [SerializeField]
        private int _reward = 100;

        /// <summary>
        /// When the player hits the pacman, just eat it.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                GameManager.Instance.AddScore(_reward);
                TerrainManager.Instance.OnGumEaten();
                SoundManager.Instance.PlaySoundQueued(_eatSound);
                Destroy(gameObject);
            }
        }
    }
}