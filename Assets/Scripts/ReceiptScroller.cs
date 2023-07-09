using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class ReceiptScroller : MonoBehaviour
    {
        [SerializeField] private RectTransform target;
        [SerializeField] private RectTransform sizeTarget;
        [SerializeField] private float scrollSpeed;

        private void Awake()
        {
            StartCoroutine(ScrollRoutine());
        }

        private IEnumerator ScrollRoutine()
        {
            yield return null;
            Debug.Log(sizeTarget.position.y);
            Debug.Log(Screen.height / 2);
            target.anchoredPosition = new Vector2(0, -sizeTarget.anchoredPosition.y / 2 + Screen.height / 2);
            while (target.anchoredPosition.y < sizeTarget.anchoredPosition.y / 2 - Screen.height / 2)
            {
                target.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            
                yield return null;
            }
        }
    }
}