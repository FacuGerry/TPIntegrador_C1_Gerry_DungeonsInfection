using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioClip _musicClip;

    private void Start()
    {
        _music.PlayOneShot(_musicClip);
    }
}
