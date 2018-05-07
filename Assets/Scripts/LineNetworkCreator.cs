using UnityEngine;

namespace GlowinBallsNetwork
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    [ExecuteInEditMode]
    public class LineNetworkCreator : MonoBehaviour 
    {
        #region Variable Declarations 
        // Private Serializable

        // Private
        LineRenderer lineRenderer;
        #endregion
 
 
 
        #region Public Properties
        
        #endregion
 
 
 
        #region Unity Event Functions
        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();

            CreateNetwork();
        }
        #endregion
 
 
 
        #region Public Functions
        
        #endregion
 
 
 
        #region Private Functions
        void CreateNetwork()
        {
            GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
            lineRenderer.positionCount = balls.Length;
            for (int i = 0; i < balls.Length; i++)
            {
                lineRenderer.SetPosition(i, balls[i].transform.position);
            }
        }
        #endregion
 
 
 
        #region Coroutines
        
        #endregion
    }
}