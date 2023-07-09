using UnityEngine;
using Random = UnityEngine.Random;

public class Sanserisms : MonoBehaviour
{
    [SerializeField] private InputRemapping inputRemapping;
    [SerializeField] private float pitchVariance;

    [SerializeField] private AudioClip normalBlip;
    [SerializeField] private AudioClip stutterBlip;
    [SerializeField] private AudioClip uhhBlip;

    [SerializeField] private DoubleAudioSource doubleAudioSource;

    private void OnEnable()
    {
        inputRemapping.normalLetterTyped.AddListener(() => PlayBlip(normalBlip));
        inputRemapping.backspaceTyped.AddListener(() => PlayBlip(uhhBlip, false));
        inputRemapping.swappedLetterTyped.AddListener(() => PlayBlip(stutterBlip));
        inputRemapping.doubleLetterTyped.AddListener(() => PlayBlip(stutterBlip));
    }
    
    private void OnDisable()
    {
        inputRemapping.normalLetterTyped.RemoveAllListeners();
        inputRemapping.backspaceTyped.RemoveAllListeners();
        inputRemapping.swappedLetterTyped.RemoveAllListeners();
        inputRemapping.doubleLetterTyped.RemoveAllListeners();
    }
    
    private void PlayBlip(AudioClip clip, bool varyPitch = true)
    {
        doubleAudioSource.CrossFade(clip, 0.3f, 0.1f);
    
        if (varyPitch)
            doubleAudioSource.CurrentSource().pitch = 1 + Random.Range(-pitchVariance, pitchVariance);
        else
            doubleAudioSource.CurrentSource().pitch = 1;
    }
}