using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TextFieldShaker : MonoBehaviour
{
    private RectTransform rectTran;
    private Vector3 basePosition;
    private float trauma = 0f;
    private Vector3 positionOffset = Vector3.zero; 
    [SerializeField] private InputRemapping inputRemapping;

    [SerializeField] private float backspaceTrauma = 0.4f;
    [SerializeField] private float traumaPower;
    [SerializeField] private float shakeDispFactor = 0.1f;
    [SerializeField] private Vector2 noiseDirection = Vector2.one;
    [SerializeField] private float shakeSpeedFactor = 1.5f;

    void Awake()
    {
        rectTran = GetComponent<RectTransform>();
        basePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (trauma <= 0f) return;
        if (trauma > 1f) trauma = 1f;

        ApplyCameraShake();
        trauma = Mathf.Max(trauma - Time.unscaledDeltaTime, 0f);
    }

    private void ApplyCameraShake()
    {
        float effectiveTrauma = Mathf.Pow(trauma, traumaPower);
        positionOffset.x = shakeDispFactor * effectiveTrauma * GetPerlin(noiseDirection, 0f, shakeSpeedFactor);
        positionOffset.y = shakeDispFactor * effectiveTrauma * GetPerlin(noiseDirection, 1f, shakeSpeedFactor);
        //positionOffset.z = shakeDispFactor * effectiveTrauma * GetPerlin(noiseDirection, 2f, shakeSpeedFactor);
        rectTran.position = basePosition + positionOffset;
    }

    public void AddShake()
    {
        print("Add shake called");
        trauma += backspaceTrauma;
    }

    public static float GetPerlin(Vector2 noiseDirection, float offset, float speedFactor)
    {
        Vector2 samplePos = noiseDirection * ((Time.unscaledTime * speedFactor) + offset);
        return Mathf.Clamp((2f * Mathf.PerlinNoise(samplePos.x, samplePos.y)) - 1f, -1f, 1f);
    }

    private void OnEnable()
    {
        //DaddyManager.instance.InputBox.backspaceTyped = AddShake;
        //DaddyManager.instance.InputBox.backspaceTyped.AddListener(() => AddShake());
        inputRemapping.backspaceTyped.AddListener(() => AddShake());
    }

    private void OnDisable()
    {
        //DaddyManager.instance.InputBox.backspaceTyped.RemoveAllListeners();
        inputRemapping.backspaceTyped.RemoveAllListeners();
    }
}
