using UnityEngine;
using System.Collections;

public class InfoBubble : MonoBehaviour
{
    //------------------------------------
    // Exposed Fields
    //------------------------------------
    public Vector3 WobbleAxis = Vector3.one;
    public float WobbleFrequency = 1;
    public float WobbleAmplitude = 0.25f;
    public Transform TrackTarget;

    //------------------------------------
    // Fields
    //------------------------------------

    private Vector3 startOffsetTarget;

    //------------------------------------
    // Methods
    //------------------------------------

    private void Start()
    {
        startOffsetTarget = transform.position - TrackTarget.position;
    }

    private void Update()
    {
        Vector3 angularWobble = Mathf.Sin(WobbleFrequency * Time.timeSinceLevelLoad) * WobbleAxis * WobbleAmplitude;

        transform.Rotate(angularWobble);
        transform.position = TrackTarget.position + startOffsetTarget;
    }
}
