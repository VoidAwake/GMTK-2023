using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuitButton : MonoBehaviour
{
    private void Awake()
    {
        #if UNITY_WEBGL
            gameObject.SetActive(false);
        #else
            GetComponent<Button>().onClick.AddListener(Application.Quit);
        #endif
    }
}