using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip _musicClip;
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _source.PlayOneShot(_musicClip);
    }
}
