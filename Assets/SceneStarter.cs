using System.Collections;
using System.Collections.Generic;
using CoffeeJitters.DataStore;
using UnityEngine;
using UnityEngine.Serialization;

public class SceneStarter : MonoBehaviour
{
    public Barista barista;
    public Canvas canvas;

    public InputRemapping inputRemapping;

    public CoffeeManager coffeeManager;

    [FormerlySerializedAs("orderUI")] public GameObject orderViewer;
    public GameDataStore gameDataStore;
    // Start is called before the first frame update
    void Start()
    {
        DaddyManager.instance.DaddyStart(canvas,barista, inputRemapping, coffeeManager, gameDataStore, orderViewer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
