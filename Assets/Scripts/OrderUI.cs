using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderUI : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI textMesh;
    private Vector2 _endPoint;
    private Vector2 _startPoint;
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
        textMesh.text = order;
        _endPoint = rect.position;
    }
}
