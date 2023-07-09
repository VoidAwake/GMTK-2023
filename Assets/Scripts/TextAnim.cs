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

    [NonSerialized] public UnityEvent charDisplayed = new();
    void Start()
    {
        textObject = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public void onTextEnd()
    {
        
    }
    
    IEnumerator BubbleClose()
    {
        Tween fade = textObject.DOFade(0, 0.1f);
        yield return fade.WaitForCompletion();
        Tween myTween = textBackground.rectTransform.DOScale(new Vector3(0f,0f,0f), 0.1f).SetEase(Ease.OutCubic);
        StartCoroutine(TextAnimator());
    }
    
    public void SetText(string text)
    {
        
        textObject.text = text;
        currentChar = 0;
        totalChars = text.Length;
        StartCoroutine(BubblePop());

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
        yield break;
        textObject.maxVisibleCharacters = totalChars;
    }

    IEnumerator BubblePop()
    {
        Tween myTween = textBackground.rectTransform.DOScale(new Vector3(0.9f,0.9f,0.9f), 0.1f).SetEase(Ease.OutCubic);
        yield return myTween.WaitForCompletion();
        StartCoroutine(TextAnimator());
    }
}
