using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    [RequireComponent(typeof(AudioSource))]
    public class Sanserisms : MonoBehaviour
    {
        [SerializeField] private InputRemapping inputRemapping;
        [SerializeField] private float pitchVariance;

        [SerializeField] private AudioClip normalBlip;
        [SerializeField] private AudioClip stutterBlip;
        [SerializeField] private AudioClip uhhBlip;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            inputRemapping.backspaceTyped.AddListener(() => PlayBlip(uhhBlip));
            inputRemapping.normalLetterTyped.AddListener(() => PlayBlip(normalBlip));
            inputRemapping.doubleLetterTyped.AddListener(() => PlayBlip(stutterBlip));
            inputRemapping.swappedLetterTyped.AddListener(() => PlayBlip(stutterBlip));
        }

        private void PlayBlip(AudioClip clip)
        {
            audioSource.pitch = 1 + Random.Range(-pitchVariance, pitchVariance);
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}