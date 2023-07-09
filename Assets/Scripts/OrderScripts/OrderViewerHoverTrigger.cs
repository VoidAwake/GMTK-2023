using UnityEngine;
using UnityEngine.EventSystems;

public class OrderViewerHoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private OrderViewer orderViewer;
    public bool enableCollision = true;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(enableCollision)
            orderViewer.OnHoverEnter();
        //Debug.Log("trigger enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        orderViewer.OnHoverExit();
        //Debug.Log("trigger exit");
    }
    
    public void SetCollision(bool newValue)
    {
        enableCollision = newValue;
        
        if (!enableCollision)
        {
            orderViewer.OnHoverExit();
        }
    }
}