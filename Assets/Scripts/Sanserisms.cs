using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Sanserisms : MonoBehaviour
    {
        [SerializeField] private InputRemapping inputRemapping;
        [SerializeField] private float pitchVariance;

        [SerializeField] private AudioClip normalBlip;
        [SerializeField] private AudioClip stutterBlip;
        [SerializeField] private AudioClip uhhBlip;

        [SerializeField] private DoubleAudioSource doubleAudioSource;

        private bool secondEvent;

        private void OnEnable()
        {
            inputRemapping.backspaceTyped.AddListener(() =>
            {
                Debug.Log("backspace");
                PlayBlip(uhhBlip, false);
            });
            inputRemapping.normalLetterTyped.AddListener(() =>
            {
                Debug.Log("normal");
                PlayBlip(normalBlip);
            });
            inputRemapping.doubleLetterTyped.AddListener(() => PlayBlip(stutterBlip));
            inputRemapping.swappedLetterTyped.AddListener(() => PlayBlip(stutterBlip));
        }

        private void PlayBlip(AudioClip clip, bool varyPitch = true)
        {
            secondEvent = !secondEvent;

            if (secondEvent) return;
            
            Debug.Log(clip.name);
            doubleAudioSource.CrossFade(clip, 0.5f, 0.1f);
            
            
            if (varyPitch)
                doubleAudioSource.CurrentSource().pitch = 1 + Random.Range(-pitchVariance, pitchVariance);
            else
                doubleAudioSource.CurrentSource().pitch = 1;
        }
    }
}