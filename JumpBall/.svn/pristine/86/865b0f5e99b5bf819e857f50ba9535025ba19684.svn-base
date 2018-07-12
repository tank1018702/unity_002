using UnityEngine;
using System.Collections;
using ch.sycoforge.Decal;
using System.Collections.Generic;

namespace ch.sycoforge.Decal.Demo
{
    /// <summary>
    /// Demo class for runtime decal placement.
    /// </summary>
    public class BasicBulletHoles : MonoBehaviour
    {
        //------------------------------------
        // ExposedFields
        //------------------------------------
        public EasyDecal DecalPrefab;


        //------------------------------------
        // Methods
        //------------------------------------

        public void Start()
        {
            if (DecalPrefab == null)
            {
                Debug.LogError("The DynamicDemo script has no decal prefab attached.");
            }
        }
        bool t = false;
        public void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                // Shoot a mouseRay thru the camera starting at the mouse's current screen space position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Check if mouseRay hit something
                if (Physics.Raycast(ray, out hit, 200))
                {
                    // Draw a line along the projection axis for visalizing the projection process.
                    Debug.DrawLine(ray.origin, hit.point, Color.red);

                    // Instantiate the decal prefab according the mouseRay direction
                    //EasyDecal.ProjectAt(DecalPrefab.gameObject, hit.collider.gameObject, hit.point, -mouseRay.direction.normalized);

                    // Instantiate the decal prefab according the hit normal
                    EasyDecal decal = EasyDecal.ProjectAt(DecalPrefab.gameObject, hit.collider.gameObject, hit.point, hit.normal);

                    t = !t;

                    if(t)
                    {
                        decal.CancelFade();                            
                    }
                }
            }
        }
    }
}
