using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcgAnimation : MonoBehaviour
{
    //private TrailRenderer trailRenderer;
    private Transform activeTrailObject;
    private Transform[] trailObjects;
    private TrailRenderer[] trailRenderers;
    private int currentTrailObject = 0;

    public float currentPanic;
    public bool active = true;

    [SerializeField] private float amplitudeResting;
    [SerializeField] private float amplitudePanic;
    [SerializeField] private float coordSpeedResting;
    [SerializeField] private float coordSpeedPanic;
    [SerializeField] private float horizSpeedResting;
    [SerializeField] private float horizSpeedPanic;
    [SerializeField] private float graphWidth;

    [SerializeField] private Gradient colorGradient;

    [SerializeField, Tooltip("0-distance between, 1-height")] 
    private List<float[]> ecgCoords = new List<float[]>();
    private int currentCoord = 0;
    private float coordInterpolation = 0f;
    void Start()
    {
        activeTrailObject = trailObjects[currentTrailObject];
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

            var localPos = GetCurrentPosition();
            activeTrailObject.localPosition = localPos;
            // TODO: set the colour of the line
        }
    }

    private Vector2 GetCurrentPosition()
    {
        float[] fromCoord = ecgCoords[currentCoord];
        float[] toCoord   = ecgCoords[(currentCoord + 1) % ecgCoords.Count];
        // start with linear interpolation, later update to sine
        float height = Mathf.Lerp(fromCoord[1], toCoord[1], coordInterpolation);
        float outHeight = height * Mathf.Lerp(amplitudeResting, amplitudePanic, currentPanic);

        // calculate actual x axis distance
        float actualHoriz = activeTrailObject.localPosition.x;
        actualHoriz += Mathf.Lerp(horizSpeedResting, horizSpeedPanic, currentPanic) * Time.deltaTime;
        if (actualHoriz >= graphWidth)
        {
            // fade out current trail object, fade in other from x=0. 
            if (trailRenderers[currentTrailObject] == null)
                trailRenderers[currentTrailObject] = trailObjects[currentTrailObject].GetComponent<TrailRenderer>();
            trailRenderers[currentTrailObject].emitting = false;

            currentTrailObject = (currentTrailObject + 1) % trailObjects.Length;
            trailRenderers[currentTrailObject].emitting = true;
            actualHoriz = 0f;
        }

        return Vector2.right * actualHoriz + Vector2.up * outHeight;
    }
}
