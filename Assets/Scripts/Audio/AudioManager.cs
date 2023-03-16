using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Controls the audio playing functionality
/// </summary>
public class AudioManager : BaseGameComponent, IAudioManager
{

    [SerializeField]
    float InitialBeatDelay;

    [SerializeField]
    float minimumBeatDelay;

    [SerializeField]
    float rateOfTempoChange;

    float beatDelay;

    [SerializeField]
    AudioAssetCollection audioAssetCollection;
    [SerializeField]
    AudioSource BGMSource;
    [SerializeField]
    AudioSource SFXSource;
    [SerializeField]
    AudioSource EngineSource;

    private void OnEnable()
    {
        AudioEventHandler.OnPlayBGM += OnPlayBGMEvent;
        AudioEventHandler.OnPlaySFX += OnPlaySFXEvent;
        AudioEventHandler.OnPlayEngineSound += OnPlayEngineSound;
    }

    private void OnPlayEngineSound(bool play, bool isLoop = true)
    {
        if (play)
        {
            EngineSource.clip = AudioAssetCollection.Instance.GetSFX(AudioAssetCollection.Instance.EngineSound);
            EngineSource.loop = isLoop;
            EngineSource.Play();
        }
        else
        {
            EngineSource?.Stop();
        }
    }

    private void OnDisable()
    {
        AudioEventHandler.OnPlayBGM -= OnPlayBGMEvent;
        AudioEventHandler.OnPlaySFX -= OnPlaySFXEvent;
    }
    #region Only For Testing
    [ContextMenu("PlayBGM")]
    public void PlayBGM()
    {
        AudioEventHandler.Instance.PlayBGM(AudioAssetCollection.Instance.BGMbeat1, true);
    }
    [ContextMenu("PlayBGM")]
    public void StopBGM()
    {
        BGMSource.Stop();
    }
    #endregion



    private void OnPlaySFXEvent(int index)
    {
        SFXSource.PlayOneShot(AudioAssetCollection.Instance.GetSFX(index));
    }

    private void OnPlayBGMEvent(int index, bool isLoop = true)
    {
        BGMSource.clip = AudioAssetCollection.Instance.GetBGM(index);
        BGMSource.loop = isLoop;

        BGMSource.Play();
    }

    public void PlayGameBGM()
    {
        //Logic to mix 2 beats to make a bgm in the game

        beatDelay = InitialBeatDelay;

        float duration1 = AudioAssetCollection.Instance.GetBGM(AudioAssetCollection.Instance.BGMbeat1).length;
        float duration2 = AudioAssetCollection.Instance.GetBGM(AudioAssetCollection.Instance.BGMbeat2).length;

        StartCoroutine(PlayBeats(duration1, duration2));

    }

    IEnumerator PlayBeats(float duration1, float duration2)
    {
        while (true)
        {
            AudioEventHandler.Instance.PlayBGM(AudioAssetCollection.Instance.BGMbeat2, false);

            yield return new WaitForSeconds(duration1);
            yield return new WaitForSeconds(beatDelay);

            AudioEventHandler.Instance.PlayBGM(AudioAssetCollection.Instance.BGMbeat1, false);
            yield return new WaitForSeconds(duration2);
            yield return new WaitForSeconds(beatDelay);


            beatDelay -= rateOfTempoChange;
            beatDelay = Mathf.Clamp(beatDelay, minimumBeatDelay, InitialBeatDelay);
        }
    }

    public void StopGameBGM()
    {
        BGMSource.Stop();
        StopAllCoroutines();
    }

}



public interface IAudioManager : IBaseGameComponent
{
    void PlayGameBGM();

    void StopGameBGM();

}
