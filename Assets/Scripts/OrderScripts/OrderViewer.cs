using System.Collections;
using System.Collections.Generic;
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
    private bool isUh = false;
    
    [SerializeField] private RectTransform pos;

    [SerializeField] private GameObject UICardNouveau;
    public OrderViewerHoverTrigger orderHoverTrigger;
    [SerializeField] private Material smudgeMat;
    private int smudgeLevel = 0;

    private Vector2 startPos;

    private bool tweenRunning = false;

    private void Start()
    {
        UICardNouveau.SetActive(false);
        orderHoverTrigger.enableCollision = false;

    }

    public void Initialise(string coffeeOrderList)
    {
        orderHoverTrigger.enableCollision = true;
        startPos = pos.localPosition;
        Destroy(DaddyManager.instance.kill);
        UICardNouveau.SetActive(true);

        textMesh.text = coffeeOrderList;
        smudgeLevel = 0;
        smudgeMat.SetFloat("_SmudgeLevel", 0f);
    }
    
    public void OnHoverEnter()
    {
        print("Entering hover");
        DaddyManager.instance.InputBox.SetOrderViewerActive(true);
        DaddyManager.instance.InputBox.DisableTyping();
        StartCoroutine(ShowOrder());
        isUh = true;
        StartCoroutine(UhBuffer());
    }
    
    public void OnHoverExit()
    {
        print("Exiting hover");
        DaddyManager.instance.InputBox.SetOrderViewerActive(false);
        DaddyManager.instance.InputBox.EnableTyping();
        StartCoroutine(HideOrder());
        isUh = false;
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

    private IEnumerator UhBuffer()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(Uh());
    }

    private IEnumerator Uh()
    {
        if (isUh)
        {
            DaddyManager.instance.InputBox.isProgramChangingText = true;
            DaddyManager.instance.InputBox.inputField.text += "U";
            while (isUh)
            {
                DaddyManager.instance.InputBox.isProgramChangingText = true;
                DaddyManager.instance.InputBox.inputField.text += "h";
                DaddyManager.instance.InputBox.backspaceTyped.Invoke();
                yield return new WaitForSeconds(0.2f);
            }
        }

    }
}