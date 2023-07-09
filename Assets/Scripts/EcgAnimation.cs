using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcgAnimation : MonoBehaviour
{
    //private TrailRenderer trailRenderer;
    private Transform activeTrailObject;
    private List<Transform> trailObjects = new List<Transform>();
    private List<TrailRenderer> trailRenderers = new List<TrailRenderer>();
    private int currentTrailObject = 0;

    /// <summary>
    /// Please set this [0,1] for the current stress of the player. 0 indicates no stress, and 1 is game ending catastrophic stress. 
    /// </summary>
    [Range(0,1)] public float currentPanic;
    private float prevPanic = 0f;
    public bool active = true;

    [SerializeField] private float amplitudeResting;
    [SerializeField] private float amplitudePanic;
    [SerializeField] private float coordSpeedResting;
    [SerializeField] private float coordSpeedPanic;
    [SerializeField] private float horizSpeedResting;
    [SerializeField] private float horizSpeedPanic;
    [SerializeField] private float trailTimeResting;
    [SerializeField] private float trailTimePanic;
    [SerializeField] private float graphWidth;

    [SerializeField] private Gradient colorGradient;

    [SerializeField, Tooltip("0-distance between, 1-height")] 
    private List<Vector2> ecgCoords = new List<Vector2>();
    private int currentCoord = 0;
    private float coordInterpolation = 0f;
    void Start()
    {
        for (int i=0; i < transform.childCount; i++)
        {
            try
            {
                TrailRenderer newTrailRenderer = transform.GetChild(i).GetComponent<TrailRenderer>();
                if (newTrailRenderer != null)
                {
                    trailRenderers.Add(newTrailRenderer);
                    trailObjects.Add(transform.GetChild(i));
                }
            }
            catch { print($"{name} child [{i}] does not have a trail renderer."); }
        }
        activeTrailObject = trailObjects[0];
        Begin(); // here for debug purposes
        //print($"Number of trail renderers: {trailRenderers.Count}");
    }

    void Update()
    {
        if (active)
        {
            float coordDistance = ecgCoords[currentCoord][0];
            coordInterpolation += Time.deltaTime * (Mathf.Lerp(coordSpeedResting, coordSpeedPanic, currentPanic) / coordDistance);
            // something, prop to speed, inv-prop to coord distance
            if (coordInterpolation >= 1f)
            {
                currentCoord = (currentCoord + 1) % ecgCoords.Count;
                coordInterpolation -= 1f;
            }

            trailRenderers[currentTrailObject].time = Mathf.Lerp(trailTimeResting, trailTimePanic, currentPanic);
            var localPos = GetCurrentPosition();
            activeTrailObject.localPosition = localPos;
            // TODO: set the colour of the line
            if (prevPanic != currentPanic)
            {
                SetTrailColour(trailRenderers[currentTrailObject]);
            }

            prevPanic = currentPanic;
        }
    }

    private Vector2 GetCurrentPosition()
    {
        var fromCoord = ecgCoords[currentCoord];
        var toCoord   = ecgCoords[(currentCoord + 1) % ecgCoords.Count];
        // start with linear interpolation, later update to sine
        float height = Mathf.Lerp(fromCoord.y, toCoord.y, coordInterpolation);
        float outHeight = height * Mathf.Lerp(amplitudeResting, amplitudePanic, currentPanic);

        // calculate actual x axis distance
        float actualHoriz = activeTrailObject.localPosition.x;
        actualHoriz += Mathf.Lerp(horizSpeedResting, horizSpeedPanic, currentPanic) * Time.deltaTime;
        if (actualHoriz >= graphWidth)
        {
            //print("Switching active trail renderer from: " + currentTrailObject);
            // fade out current trail object, fade in other from x=0. 
            trailRenderers[currentTrailObject].emitting = false;

            currentTrailObject = (currentTrailObject + 1) % trailObjects.Count;
            activeTrailObject = trailObjects[currentTrailObject];
            UpdateTrailProperties(trailRenderers[currentTrailObject]);
            
            actualHoriz = 0f;
            StartCoroutine(ClearTrailNextFrame(currentTrailObject));
        }

        return Vector2.right * actualHoriz + Vector2.up * outHeight;
    }

    public void Begin()
    {
        activeTrailObject = trailObjects[currentTrailObject];
        foreach (var trail in trailRenderers)
            UpdateTrailProperties(trail);
    }

    private void UpdateTrailProperties(TrailRenderer trailRenderer)
    {
        trailRenderers[currentTrailObject].Clear();
        trailRenderers[currentTrailObject].emitting = true;
        SetTrailColour(trailRenderer);
        trailRenderer.time = Mathf.Lerp(trailTimeResting, trailTimePanic, currentPanic);
    }

    private void SetTrailColour(TrailRenderer renderer)
    {
        Color targetColor = colorGradient.Evaluate(currentPanic);
        Gradient newGrad = new Gradient();
        newGrad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(targetColor, 0.0f), new GradientColorKey(targetColor, 1.0f) },
            renderer.colorGradient.alphaKeys
        );
        renderer.colorGradient = newGrad;
    }

    private IEnumerator ClearTrailNextFrame(int trailIndex)
    {
        yield return new WaitForEndOfFrame();
        trailRenderers[trailIndex].Clear();
    }
}
