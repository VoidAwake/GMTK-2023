using DG.Tweening;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private RectTransform credits;
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private Vector2 shownPos;
    [SerializeField] private float tweenTime;

    private Vector2 hiddenPos;
    
    private void Awake()
    {
        hiddenPos = credits.anchoredPosition;
    }

    public void ShowCredits()
    {
        mainMenu.DOFade(0, tweenTime).SetEase(Ease.OutCubic);
        credits.DOAnchorPos(shownPos, tweenTime, false).SetEase(Ease.OutCubic);
    }

    public void HideCredits()
    {
        mainMenu.DOFade(1, tweenTime).SetEase(Ease.InCubic);
        credits.DOAnchorPos(hiddenPos, tweenTime, false).SetEase(Ease.InCubic);
    }
}