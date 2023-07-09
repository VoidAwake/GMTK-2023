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
    [SerializeField] private Material smudgeMat;
    private int smudgeLevel = 0;

    private Vector2 startPos;

    private bool tweenRunning = false;
    
    public void Initialise(string coffeeOrderList)
    {
        startPos = rect.localPosition;
        
        textMesh.text = coffeeOrderList;
        smudgeLevel = 0;
        smudgeMat.SetFloat("SmudgeLevel", 0f);
    }
    
    public void OnHoverEnter()
    {
        print("Entering hover");
        DaddyManager.instance.InputBox.SetOrderViewerActive(true);
        DaddyManager.instance.InputBox.DisableTyping();
        StartCoroutine(ShowOrder());
    }
    
    public void OnHoverExit()
    {
        print("Exiting hover");
        DaddyManager.instance.InputBox.SetOrderViewerActive(false);
        DaddyManager.instance.InputBox.EnableTyping();
        StartCoroutine(HideOrder());
        smudgeLevel++;
        smudgeMat.SetFloat("_SmudgeLevel", (float)smudgeLevel);
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