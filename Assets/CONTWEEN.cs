using DG.Tweening;
using UnityEngine;

public class CONTWEEN : MonoBehaviour
{
    [SerializeField] private RectTransform pos;
    
    private void Start()
    {
        if (!pos) return;
        
        RectTransform rect = GetComponent<RectTransform>();
        
        rect.DOAnchorPos(pos.anchoredPosition, 0.5f).SetEase(Ease.OutCubic);
    }
}
