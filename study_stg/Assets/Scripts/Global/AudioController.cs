using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   効果音再生で用いる
/// </summary>
public class AudioController : MonoBehaviour {
    /// <summary>効果音のAudioClip</summary>
    public AudioClip[] soundEffect;

    /// <summary>BGMのAudioClip</summary>
    public AudioClip[] BGM;

    /// <summary>アタッチされているAudioSource(効果音用)</summary>
    public AudioSource audioSourceSoundEffect;

    /// <summary>アタッチされているAudioSource(BGM用)</summary>
    public AudioSource audioSourceBGM;

    /// <summary>BGM音量(0.0f~1.0f)</summary>
    public float BGMvolume
    {
        get
        {
            return audioSourceBGM.volume;
        }
        set
        {
            audioSourceBGM.volume = value;
        }
    }


    /// <summary>
    ///   効果音を鳴らす
    /// </summary>
    /// <param name="soundID">効果音ID</param>
    public void PlaySoundEffect(Define.SoundID soundID)
    {
        audioSourceSoundEffect.PlayOneShot(soundEffect[(int)soundID]);
    }


    /// <summary>
    ///   BGMを鳴らす
    /// </summary>
    /// <param name="BGMID">BGMID</param>
    public void PlayBGM(Define.BGMID bgmID)
    {
        audioSourceBGM.clip = BGM[(int)bgmID];
        audioSourceBGM.volume = 1.0f;
        audioSourceBGM.Play();
    }


    /// <summary>
    ///   BGMを止める
    /// </summary>
    public void StopBGM()
    {
        audioSourceBGM.Stop();
    }
}
