using System;
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
            inputRemapping.normalLetterTyped.AddListener(() =>
            {
                Debug.Log("normal");
                PlayBlip(normalBlip);
            });
            
            inputRemapping.backspaceTyped.AddListener(() =>
            {
                Debug.Log("backspace");
                PlayBlip(uhhBlip, false);
            });
            
            inputRemapping.swappedLetterTyped.AddListener(() =>
            {
                Debug.Log("swap");
                PlayBlip(uhhBlip, false);
            });
            
            inputRemapping.doubleLetterTyped.AddListener(() =>
            {
                Debug.Log("double");
                PlayBlip(uhhBlip, false);
            });
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
            secondEvent = !secondEvent;

            if (secondEvent) return;
            
            //Debug.Log(clip.name);
            
            if (doubleAudioSource != null)
            {
                doubleAudioSource.CrossFade(clip, 0.5f, 0.1f);
            
            
                if (varyPitch)
                    doubleAudioSource.CurrentSource().pitch = 1 + Random.Range(-pitchVariance, pitchVariance);
                else
                    doubleAudioSource.CurrentSource().pitch = 1;
            }
        }
    }
}