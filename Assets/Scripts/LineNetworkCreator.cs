using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlowinBallsNetwork
{
    /// <summary>
    /// 
    /// </summary>
    public class LineNetworkCreator : MonoBehaviour 
    {
        #region Variable Declarations 
        // Private Serializable
        [Space]
        [SerializeField] float maxConnectionDistance = 5f;
        [SerializeField] int maxConnectionsPerBall = 6;
        [SerializeField] float waitTime = 1f;
        [SerializeField] float lineDrawTime = 1f;
        [Range(1f, 5f)]
        [SerializeField] float ballHighlightAmount = 2f;
        [SerializeField] bool randomStartBall;

        [Header("References")]
        [SerializeField] Transform startBall;
        [SerializeField] GameObject lineRendererPrefab;
        [SerializeField] GameObject colliderPrefab;

        [Space]
        [SerializeField] bool debug = true;

        // Private
        List<Transform> freeBalls = new List<Transform>();
        List<Transform> allBalls = new List<Transform>();
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Start()
        {
            // Add all balls in the scene to the freeBalls
            GameObject[] ballsGO = GameObject.FindGameObjectsWithTag("Ball");
            for (int i = 0; i < ballsGO.Length; i++)
            {
                freeBalls.Add(ballsGO[i].transform);
                allBalls.Add(ballsGO[i].transform);
            }

            // Start the action
            CreateNetwork();
        }
        #endregion
 
 
 
        #region Public Functions
        
        #endregion
 
 
 
        #region Private Functions
        void CreateNetwork()
        {
            if (!randomStartBall) ConnectBall(startBall);
            else ConnectBall(allBalls[Random.Range(0, allBalls.Count)]);
        }


        void ConnectBall(Transform currentBall)
        {
            // Make the currentBall glow
            Material material = currentBall.GetComponent<Renderer>().material;
            Color emissionColor = material.GetColor("_EmissionColor");

            LeanTween.value(gameObject, emissionColor, emissionColor * ballHighlightAmount, waitTime / 2).setLoopPingPong(1).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((Color value) =>
            {
                material.SetColor("_EmissionColor", value);
            });

            // Find all balls in the allowed distane to the current ball
            List<Transform> connectableBalls = new List<Transform>();
            for (int i = 0; i < freeBalls.Count; i++)
            {
                if (Vector3.Distance(freeBalls[i].position, currentBall.position) <= maxConnectionDistance)
                {
                    Ray ray = new Ray(currentBall.position, freeBalls[i].position - currentBall.position);
                    RaycastHit hitInfo = new RaycastHit();
                    if (debug) Debug.DrawRay(currentBall.position, freeBalls[i].position - currentBall.position, Color.yellow, waitTime);
                    Physics.Raycast(ray, out hitInfo, maxConnectionDistance);
                    if (hitInfo.collider != null && hitInfo.collider.transform == freeBalls[i])
                    {
                        connectableBalls.Add(freeBalls[i]);
                    }
                }
            }

            // Randomly select balls from the calculated list to connect to
            List<Transform> connectedBalls = new List<Transform>(maxConnectionsPerBall);
            float ballsNeeded = maxConnectionsPerBall;
            float ballsLeft = connectableBalls.Count;
            for (int i = 0; i < connectableBalls.Count; i++)
            {
                float random = Random.Range(0f, 1f);
                if (random <= ballsNeeded / ballsLeft)
                {
                    connectedBalls.Add(connectableBalls[i]);
                    ballsNeeded--;
                }
                ballsLeft--;
            }

            // Spawn the lineRenderer for the currentBall
            GameObject go = Instantiate(lineRendererPrefab, currentBall);
            LineRenderer lineRenderer = go.GetComponent<LineRenderer>();

            // Connect balls
            lineRenderer.positionCount += connectedBalls.Count * 2;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                if (i == 0) lineRenderer.SetPosition(0, currentBall.position);

                else if (i % 2 == 1)
                {
                    // Set the lineRenderer position until the tween kicks in (else you get a nasty flickering line)
                    lineRenderer.SetPosition(i, currentBall.position);

                    // Draw the line
                    int num = i;
                    LeanTween.value(gameObject, currentBall.position, connectedBalls[num / 2].position, lineDrawTime).setOnUpdate((Vector3 value) =>
                    {
                        lineRenderer.SetPosition(num, value);
                    });

                    // Set the collider for this line
                    Transform collider = Instantiate(colliderPrefab, lineRenderer.transform).transform;
                    collider.forward = connectedBalls[i / 2].position - currentBall.position;
                    Vector3 newScale = collider.localScale;
                    newScale.z = Vector3.Distance(connectedBalls[i / 2].position, currentBall.position) - 0.5f;
                    collider.localScale = newScale;

                }
                else if (i % 2 == 0) lineRenderer.SetPosition(i, currentBall.position);
            }

            // Remove the currentBall from the free balls
            freeBalls.Remove(currentBall);
            
            // Wait and start the next ball connection
            StartCoroutine(Wait(waitTime, () =>
            {
                // Start all connected balls
                if (freeBalls.Count > 0)
                {
                    for (int i = 0; i < connectedBalls.Count; i++)
                    {
                        if (freeBalls.Contains(connectedBalls[i]))
                        {
                            ConnectBall(connectedBalls[i]);
                        }
                    }
                }
            }));
        }
        #endregion
 
 
 
        #region Coroutines
        IEnumerator Wait(float duration, System.Action onComplete)
        {
            yield return new WaitForSeconds(duration);
            onComplete.Invoke();
        }
        #endregion
    }
}