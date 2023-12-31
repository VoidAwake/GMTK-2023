using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextAnim : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI textObject;
    [SerializeField] private float timeBetweenChars;
    [SerializeField] private int currentChar;
    [SerializeField] private int totalChars;
    [SerializeField] private Image textBackground;

    [NonSerialized] public UnityEvent animationCompleted = new();

    [NonSerialized] public UnityEvent charDisplayed = new();
    void Start()
    {
        textObject = GetComponent<TextMeshProUGUI>();
        textBackground.rectTransform.localScale = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    public void onTextEnd()
    {
        StartCoroutine(BubbleClose());
    }
    
    IEnumerator BubbleClose()
    {
        Tween fade = textObject.DOFade(0, 0.1f);
        yield return fade.WaitForCompletion();
        Tween myTween = textBackground.rectTransform.DOScale(new Vector3(0f,0f,0f), 0.1f).SetEase(Ease.OutCubic);
        animationCompleted.Invoke();
    }
    
    public void SetText(string text, bool response = false)
    {
        textObject.maxVisibleCharacters = 0;
        StopAllCoroutines();
        //DOTween.KillAll();
        textObject.DOFade(1, 0.1f);
        textObject.text = text;
        currentChar = 0;
        totalChars = text.Length;
        if (response)
        {
            StartCoroutine(TextAnimatorToClose());
        }
        else
        {
            StartCoroutine(BubblePop());
        }

    }

    IEnumerator TextAnimator()
    {
        while (currentChar < totalChars)
        {
            currentChar++;
            textObject.maxVisibleCharacters = currentChar;
            
            charDisplayed.Invoke();
            
            yield return new WaitForSeconds(timeBetweenChars);
        }
        
        animationCompleted.Invoke();
        yield break;
        textObject.maxVisibleCharacters = totalChars;
    }
    
    IEnumerator TextAnimatorToClose()
    {
        while (currentChar < totalChars)
        {
            currentChar++;
            textObject.maxVisibleCharacters = currentChar;
            yield return new WaitForSeconds(timeBetweenChars);
        }
        textObject.maxVisibleCharacters = totalChars;
        animationCompleted.Invoke();
        yield return new WaitForSeconds(1f);
        StartCoroutine(BubbleClose());
    }

    IEnumerator BubblePop()
    {
        Tween myTween = textBackground.rectTransform.DOScale(new Vector3(0.9f,0.9f,0.9f), 0.1f).SetEase(Ease.OutCubic);
        yield return myTween.WaitForCompletion();
        StartCoroutine(TextAnimator());
    }
}
