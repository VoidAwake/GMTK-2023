using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CONTWEEN : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private RectTransform pos;
    void Start()
    {
        if (pos)
        {
            RectTransform rect = GetComponent<RectTransform>();
            rect.DOAnchorPos(pos.anchoredPosition, 0.5f, false).SetEase(Ease.OutCubic);
        }
    }

    // Update is called once per frame
    IEnumerator tween()
    {
        yield return null;
    }
}
