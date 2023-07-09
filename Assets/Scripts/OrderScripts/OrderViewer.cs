using System.Collections;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class OrderViewer : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public Vector2 _tweenPos;
    public float _tweenTime;
    public RectTransform rect;

    private Vector2 startPos;

    private bool tweenRunning = false;
    
    public void Initialise(string coffeeOrderList)
    {
        startPos = rect.localPosition;
        
        textMesh.text = coffeeOrderList;
    }
    
    public void OnHoverEnter()
    {
        StartCoroutine(ShowOrder());
    }
    
    public void OnHoverExit()
    {
        StartCoroutine(HideOrder());
    }
    
    private IEnumerator ShowOrder()
    {
        tweenRunning = true;
        
        Tween myTween = rect.DOAnchorPos(_tweenPos, _tweenTime, false).SetEase(Ease.OutCubic);
        yield return myTween.WaitForCompletion();
        
        tweenRunning = false;
    }
    
    private IEnumerator HideOrder()
    {
        tweenRunning = true;
        
        Vector2 newPosition = new Vector2(startPos.x, startPos.y);
        
        Tween myTween = rect.DOAnchorPos(newPosition, _tweenTime, false).SetEase(Ease.OutCubic);
        yield return myTween.WaitForCompletion();
        
        tweenRunning = false;
    }
}