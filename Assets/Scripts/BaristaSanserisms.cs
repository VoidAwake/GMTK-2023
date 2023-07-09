using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaristaSanserisms : MonoBehaviour
{
    [SerializeField] private float pitchVariance;

    [SerializeField] private DoubleAudioSource doubleAudioSource;

    [SerializeField] private Barista barista;
    [SerializeField] private TextAnim textAnim;

    private void OnEnable()
    {
        // barista.textDisplayed.AddListener(PlayBlips);
        textAnim.charDisplayed.AddListener(PlayBlip);
    }
    
    private void OnDisable()
    {
        // inputRemapping.normalLetterTyped.RemoveAllListeners();
        textAnim.charDisplayed.RemoveAllListeners();
    }
    
    private void PlayBlips(string text)
    {
        StartCoroutine(PlayBlipsRoutine(text));
    }

    private IEnumerator PlayBlipsRoutine(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            doubleAudioSource.CrossFade(barista.currentBarista.blip, 0.5f, 0.1f);
        
            doubleAudioSource.CurrentSource().pitch = 1 + Random.Range(-pitchVariance, pitchVariance);

            yield return new WaitForSeconds(1 / barista.currentBarista.textSpeed);
        }
    }
    
    private void PlayBlip()
    {
        Debug.Log("yeah");
        doubleAudioSource.CrossFade(barista.currentBarista.blip, 0.5f, 0.02f);
    
        doubleAudioSource.CurrentSource().pitch = 1 + Random.Range(-pitchVariance, pitchVariance);
    }
}
