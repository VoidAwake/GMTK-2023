using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnim : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI textObject;
    void Start()
    {
        textObject = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void setText(string text)
    {
        
        textObject.text = text;
        
    }

    IEnumerator TextAnimator()
    {
        
        yield return null;
    }
}
