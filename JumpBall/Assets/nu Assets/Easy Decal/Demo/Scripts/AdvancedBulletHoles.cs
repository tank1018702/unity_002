using ch.sycoforge.Decal;
using UnityEngine;

namespace ch.sycoforge.Decal.Demo
{
    /// <summary>
    /// Demo class for advanced runtime decal placement.
    /// </summary>
    public class AdvancedBulletHoles : MonoBehaviour
    {
        //------------------------------------
        // Exposed Fields
        //------------------------------------
        public EasyDecal DecalPrefab;
        public GameObject ImpactParticles;
        public float CastRadius = 0.25f;

        //------------------------------------
        // Methods
        //------------------------------------

        private void Start()
        {
            if (DecalPrefab == null)
            {
                Debug.LogError("The AdvancedBulletHoles script has no decal prefab attached.");
            }
            EasyDecal.HideMesh = false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                // Shoot a mouseRay thru the camera starting at the mouse's current screen space position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit initialHit;

                // Check if mouseRay hit something
                if (Physics.Raycast(ray, out initialHit, 200))
                {
                    // Set the first hit as parent of the decal
                    GameObject parent = initialHit.collider.gameObject;
                    Vector3 position = initialHit.point;

                    RaycastHit[] hits = Physics.SphereCastAll(ray, CastRadius, Vector3.Distance(Camera.main.transform.position, position) + 2);
                    Vector3 averageNormal = initialHit.normal;

                    // Check if sphere cast hit something
                    if (hits.Length > 0)
                    {

                        foreach (RaycastHit hit in hits)
                        {
                            // Draw a line along the projection axis for visalizing the projection process.
                            Debug.DrawLine(ray.origin, hit.point, Color.red);

                            // Sum all collison point normals
                            averageNormal += hit.normal;
                        }
                    }

                    // Normalize normal
                    averageNormal /= hits.Length + 1;

                    // Instantiate the decal prefab according the hit normal
                    EasyDecal.ProjectAt(DecalPrefab.gameObject, parent, position, averageNormal);

                    if(ImpactParticles != null)
                    {
                        Quaternion rot = Quaternion.FromToRotation(Vector3.up, averageNormal);

                        GameObject.Instantiate(ImpactParticles, position, rot);
                    }
                }
            }
        }
    }
}
