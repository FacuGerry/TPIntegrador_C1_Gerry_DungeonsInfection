using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;

    [Header("Sources")]
    [SerializeField] private AudioSource _sfx;
    [SerializeField] private AudioSource _ui;

    [Header("Pickables")]
    [SerializeField] private AudioClip _bookColected;

    [Header("Player in Overworld")]
    [SerializeField] private AudioClip _playerStepsSound;

    [Header("Player in Combat")]
    [SerializeField] private AudioClip _playerAttackSound;
    [SerializeField] private AudioClip _playerHurtSound;
    [SerializeField] private AudioClip _playerDefendSound;
    [SerializeField] private AudioClip _playerDeathSound;

    [Header("Spells")]
    [SerializeField] private AudioClip _fireballSound;
    [SerializeField] private AudioClip _iceWallSound;
    [SerializeField] private AudioClip _DarkShieldSound;
    [SerializeField] private AudioClip _HealingRootSound;

    [Header("Infected")]
    [SerializeField] private AudioClip _infectedAttackSound;
    [SerializeField] private AudioClip _infectedHurtSound;
    [SerializeField] private AudioClip _infectedDeadSound;

    [Header("Virus")]
    [SerializeField] private AudioClip _virusAttackSound;
    [SerializeField] private AudioClip _virusHurtSound;
    [SerializeField] private AudioClip _virusDeadSound;

    [Header("Ui Sounds")]
    [SerializeField] private AudioClip _btnHover;
    [SerializeField] private AudioClip _btnClick;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        UiButtonHoverSFXEvent.OnButtonHover += OnButtonHover_SoundButtonHover;
        UiButtonHoverSFXEvent.OnButtonClick += OnButtonClick_SoundButtonClick;
    }

    private void OnDisable()
    {
        UiButtonHoverSFXEvent.OnButtonHover -= OnButtonHover_SoundButtonHover;
        UiButtonHoverSFXEvent.OnButtonClick -= OnButtonClick_SoundButtonClick;
    }

    public void OnButtonHover_SoundButtonHover()
    {
        _ui.PlayOneShot(_btnHover);
    }

    public void OnButtonClick_SoundButtonClick()
    {
        _ui.PlayOneShot(_btnClick);
    }
}
