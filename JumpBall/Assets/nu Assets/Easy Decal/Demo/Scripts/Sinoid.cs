using UnityEngine;
using System.Collections;
using ch.sycoforge.Decal;

namespace ch.sycoforge.Decal.Demo
{
    public class Sinoid : MonoBehaviour
    {
        //------------------------------------
        // Exposed Fields
        //------------------------------------
        public float AngularVelocity = 2f;
        public float SineFreq = 0.2f;
        public float Amplitude = 0.25f;


        //------------------------------------
        // Fields
        //------------------------------------
        private float accuTime = 0;
        private Vector3 startPos;

        //------------------------------------
        // Methods
        //------------------------------------
        private void Start()
        {
            startPos = transform.position;
        }

        private void Update()
        {
            accuTime += Time.deltaTime;

            transform.position = startPos + Vector3.up * Amplitude * Mathf.Sin(accuTime * 2 * Mathf.PI * SineFreq);
            transform.Rotate((Vector3.up + Vector3.forward) * AngularVelocity * Time.deltaTime);
        }
    }
}
