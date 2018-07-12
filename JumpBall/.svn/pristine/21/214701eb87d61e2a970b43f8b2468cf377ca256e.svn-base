using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ch.sycoforge.Decal;

#if UNITY_5_5
using UnityEngine.AI;
#endif

namespace ch.sycoforge.Decal.Demo
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(LineRenderer))]
    public class PathAgent : MonoBehaviour
    {
        //------------------------------------
        // Exposed Fields
        //------------------------------------

        //Path thickness in units
        public float PathThickness = 1f;

        [Tooltip("Distance from the ground.")]
        public float NormalPathOffset;

        [Tooltip("Max radius between segments.")]
        [Range(0.001f, 0.5f)]
        public float Radius = 0.25f;

        [Tooltip("Discard segments when their angle is smaller than this value.")]
        public float AngleThreshold = 5;
        public bool DrawGizmos;
        public EasyDecal TargetAimDecal;
        public GameObject TargetPointDecalPrefab;

        //------------------------------------
        // Fields
        //------------------------------------
        private List<Vector3> path = new List<Vector3>();
        private UnityEngine.AI.NavMeshAgent agent;
        private LineRenderer lineRenderer;
        private Vector3 decalOffset = Vector3.up * 0.5f;

        private const int MAXDISTANCE = 50;

        //------------------------------------
        // Methods
        //------------------------------------

        private void Start()
        {
            TargetAimDecal.gameObject.SetActive(false);

            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            CreatePath(ray);
            SetTarget(ray);
        }

        private void SetTarget(Ray mouseRay)
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;

                // Check if mouseRay hit something
                if (Physics.Raycast(mouseRay, out hit, MAXDISTANCE))
                {
                    agent.SetDestination(hit.point);

                    EasyDecal.ProjectAt(TargetPointDecalPrefab, hit.collider.gameObject, hit.point + decalOffset, Quaternion.identity);
                }
            }
        }

        private void CreatePath(Ray mouseRay)
        {
            RaycastHit hit;

            // Check if mouseRay hit something
            if (Physics.Raycast(mouseRay, out hit, MAXDISTANCE))
            {
                Vector3 start = transform.position;
                Vector3 end = hit.point;

                this.path.Clear();

                UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
                if (UnityEngine.AI.NavMesh.CalculatePath(start, end, UnityEngine.AI.NavMesh.AllAreas, path))
                {
                    if (path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
                    {
                        int length = path.corners.Length;



                        Vector3 normal = transform.up;

                        for (int i = 0; i < length; i++)
                        {                           
                            if (i > 0 && NormalPathOffset > 0)
                            {
                                RaycastHit h;

                                if(Physics.Raycast(path.corners[i], Vector3.down, out h, NormalPathOffset * 10))
                                {
                                    normal = hit.normal;
                                }
                            }

                            Vector3 corner = path.corners[i] + normal * NormalPathOffset;

                            this.path.Add(corner);
                        }

                        Vector3[] points = BezierUtil.InterpolatePath(this.path, 10, Radius, AngleThreshold).ToArray();

                        lineRenderer.SetVertexCount(points.Length);
                        #if UNITY_5_2
                        for (int i = 0; i < points.Length; i++ )
                        {
                            lineRenderer.SetPosition(i, points[i]);
                        }
                        #else
                        lineRenderer.SetPositions(points);
                        #endif

                        TargetAimDecal.gameObject.SetActive(true);
                        TargetAimDecal.gameObject.transform.position = path.corners[length - 1] + decalOffset;

                        return;
                    }
                }
            }

            TargetAimDecal.gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if (DrawGizmos)
            {
                Gizmos.color = Color.red;

                foreach (Vector3 p in path)
                {
                    Gizmos.DrawSphere(p, 0.05f);
                }
            }
        }
    }
}