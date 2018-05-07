using UnityEngine;

namespace GlowinBallsNetwork
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
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
        }
        #endregion
 
 
 
        #region Public Functions
        
        #endregion
 
 
 
        #region Private Functions
        
        #endregion
 
 
 
        #region Coroutines
        
        #endregion
    }
}