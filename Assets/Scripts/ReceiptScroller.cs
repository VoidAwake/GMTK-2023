using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class ReceiptScroller : MonoBehaviour
    {
        [SerializeField] private RectTransform target;
        [SerializeField] private float scrollSpeed;

        private void Awake()
        {
            target.anchoredPosition = new Vector2(0, -target.rect.height / 2);
            StartCoroutine(ScrollRoutine());
        }

        private IEnumerator ScrollRoutine()
        {
            while (target.anchoredPosition.y > target.rect.height / 2)
            {
                target.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

                yield return null;
            }
        }
    }
}