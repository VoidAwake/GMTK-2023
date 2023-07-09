using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;


public class CreditsRoll : MonoBehaviour
{

    [Serializable]
    public enum AnimationAction
    {
        slideUp,
        slideDown,
        noAction
    }
    //Enum option for opening and closing panel
    public AnimationAction OpenAnimation, CloseAnimation;

    //Animation Effect adding to the animation of the panel. 
    public Ease OpeningEase, ClosingEase;

    //CanvasGroup attached to the gameObject
    public CanvasGroup thisPanelCanvasGroup;

    //total time for the animation duration.
    public float animationTime = 0.25f;
    // Start is called before the first frame update

    ///<summary>
    /// Positions of all four side of the screen. It is calculated using the screen camera .
    ///</summary>
    private Vector3
        leftScreenPosition,
        rightScreenPosition,
        topScreenPosition,
        bottomScreenPosition;

    ///<summary>
    ///screen positions updated status. It must be set to true when 		all side of the screen positions are set.
    ///</summary>
    private bool positionSet;

    ///<summary>
    /// Positions of all four side of the screen. It is calculated using the screen camera .
    ///</summary>
    private Vector3
    leftOffSetScreenPosition,
    rightOffSetScreenPosition,
    topOffSetScreenPosition,
    bottomOffSetScreenPosition;

    public Button _closeButton;


    private void Awake()
    {
        if (thisPanelCanvasGroup == null)
            thisPanelCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.AddListener(() => PlayAnimation(false));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }



    private Camera _mainCamera;//Main camera of the scene.

    //Get the all position of all the screen side positions.
    private void UpdateOffsetScreenPositions()
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main;
        Vector3 screenDimen = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)) * 2;
        if (screenDimen.x < screenDimen.y)
        {
            screenDimen = new Vector3(screenDimen.y, screenDimen.x, screenDimen.z);
        }

        topScreenPosition = new Vector3(0, screenDimen.y / 2, 0);
        bottomScreenPosition = new Vector3(0, -screenDimen.y / 2, 0);
        leftScreenPosition = new Vector3(-screenDimen.x / 2, 0, 0);
        rightScreenPosition = new Vector3(screenDimen.x / 2, 0, 0);


    }

    //Set the all position of all the screen side positions.
    private void UpdateAllSideScreenPositions()
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main;
        Vector3 screenDimen = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)) * 2;
        if (screenDimen.x < screenDimen.y)
        {
            screenDimen = new Vector3(screenDimen.y, screenDimen.x, screenDimen.z);
        }

        topScreenPosition = new Vector3(0, screenDimen.y / 2, 0);
        bottomScreenPosition = new Vector3(0, -screenDimen.y / 2, 0);
        leftScreenPosition = new Vector3(-screenDimen.x / 2, 0, 0);
        rightScreenPosition = new Vector3(screenDimen.x / 2, 0, 0);
    }

    /// <summary>
    /// Set the positions outside of the screen so the panel could outside of the view/screen if necessary.
    /// </summary>
    private void SetOffScreenPositionForPanel()
    {
        RectTransform myRect = transform.GetComponent<RectTransform>();
        float sizeToAdd = myRect.rect.width / 100;
        leftOffSetScreenPosition = leftScreenPosition - new Vector3(sizeToAdd, 0, 0);
        rightOffSetScreenPosition = rightScreenPosition + new Vector3(sizeToAdd, 0, 0);
        sizeToAdd = myRect.rect.height / 100;
        topOffSetScreenPosition = topScreenPosition + new Vector3(0, sizeToAdd, 0);
        bottomOffSetScreenPosition = bottomScreenPosition - new Vector3(0, sizeToAdd, 0);
        positionSet = true;
    }


    private void PlayAnimation(bool playOpenAnimation)
    {
        try
        {
            Vector3 posToGive = Vector3.zero;
            Vector3 initPos = Vector3.zero;
            float scale = 1;
            AnimationAction playAnimationType = playOpenAnimation ? OpenAnimation : CloseAnimation;
            switch (playAnimationType)
            {
                case AnimationAction.slideUp:
                    initPos = playOpenAnimation ? bottomOffSetScreenPosition : transform.position;
                    transform.position = initPos;
                    posToGive = playOpenAnimation ? new Vector3(transform.position.x, 0, 0) : topOffSetScreenPosition;
                    StartSlideAnimation(playOpenAnimation, posToGive);
                    break;
                case AnimationAction.slideDown:
                    initPos = playOpenAnimation ? topOffSetScreenPosition : transform.position;
                    transform.position = initPos;
                    posToGive = playOpenAnimation
                        ? new Vector3(transform.position.x, 0, 0)
                        : bottomOffSetScreenPosition;
                    StartSlideAnimation(playOpenAnimation, posToGive);
                    break;
                default:
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Exception while running dotWeen animation " + e);
        }
    }

    private void StartSlideAnimation(bool openingAnimation, Vector3 position)
    {
        try
        {
            if (openingAnimation)
            {
                transform.DOMove(position, animationTime).SetEase(OpeningEase)
                    .OnComplete(() => transform.localPosition = position);
                if (thisPanelCanvasGroup != null)
                    thisPanelCanvasGroup.DOFade(1, animationTime);
            }
            else
            {
                if (thisPanelCanvasGroup != null)
                    thisPanelCanvasGroup.DOFade(0, animationTime);
                transform.DOMove(position, animationTime).SetEase(ClosingEase);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Exception throw on panel sliding animation. " + e);
        }
    }

    private void OnEnable()
    {

        if (!positionSet)
        {
            //Set the positions of all sides of the screen
            UpdateAllSideScreenPositions();

            //update the offset position of the scree as per the size of the panel
            SetOffScreenPositionForPanel();
        }

        //set the alpha value of the canvas group to 0 for its initial value
        if (thisPanelCanvasGroup != null)
            thisPanelCanvasGroup.alpha = 0;

        //play the animation
        PlayAnimation(true);
    }
}
