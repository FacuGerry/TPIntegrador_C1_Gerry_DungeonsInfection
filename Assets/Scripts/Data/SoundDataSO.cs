using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundSettings", menuName = "GameData/SoundSettings")]
public class SoundDataSO : ScriptableObject
{
    public AudioMixer mixer;
    public float masterVol;
    public float musicVol;
    public float sfxVol;
}
