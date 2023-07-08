using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;

public class OrderUI : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI textMesh;
    private Vector2 _tweenPos;
    private bool lerp;
    public RectTransform rect;
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OrderInit(string order)
    {
        
        rect = GetComponent<RectTransform>();
        textMesh.text = order;
        rect.position = new Vector2(rect.position.x, rect.position.y + 800);
        rect.DOAnchorPos(new Vector2(-60f,10f), 0.8f, false).SetEase(Ease.OutCubic);
        rect.DORotate(new Vector3(0f,0f,15f), 0.8f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic);
        //MoveOrder();

    }
}
