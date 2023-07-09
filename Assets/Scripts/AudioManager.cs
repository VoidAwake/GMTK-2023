using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(GameObject.Find("FMOD_StudioSystem"));
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
