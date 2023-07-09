using UnityEngine;
using UnityEngine.EventSystems;

public class OrderViewerHoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private OrderViewer orderViewer;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        orderViewer.OnHoverEnter();
        //Debug.Log("trigger enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        orderViewer.OnHoverExit();
        //Debug.Log("trigger exit");
    }
}