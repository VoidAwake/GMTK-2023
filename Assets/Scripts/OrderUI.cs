using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class OrderUI : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI textMesh;
    public Vector2 _tweenPos;
    public float _tweenTime;
    public RectTransform rect;

    public Vector2 endPos;
    
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    public void Continue(InputAction.CallbackContext context)
    {
        if (context.performed)
            StartCoroutine(SomeCoroutine());
        //DaddyManager.instance.GameStart();
    }
    
    IEnumerator SomeCoroutine()
    {
        Tween myTween = rect.DOAnchorPos(endPos, _tweenTime, false).SetEase(Ease.OutCubic);
        rect.DORotate(new Vector3(0f,0f,0f), _tweenTime, RotateMode.FastBeyond360).SetEase(Ease.OutCubic);
        
        yield return myTween.WaitForCompletion();
        
        DaddyManager.instance.GameStart();
        Destroy(gameObject);
    }

    public void OrderInit(string order)
    {
        
        rect = GetComponent<RectTransform>();
        textMesh.text = order;
        Vector2 position = rect.position;
        position = new Vector2(position.x, position.y + 800);
        rect.position = position;
        rect.DOAnchorPos(_tweenPos, _tweenTime, false).SetEase(Ease.OutCubic);
        rect.DORotate(new Vector3(0f,0f,15f), _tweenTime, RotateMode.FastBeyond360).SetEase(Ease.OutCubic);
        //MoveOrder();

    }
}
