using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStarter : MonoBehaviour
{
    public Barista barista;
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        DaddyManager.instance.DaddyStart(canvas,barista);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
