using UnityEngine;
using UnityEngine.SceneManagement;

namespace NGlow
{
    /// <summary>
    /// 
    /// </summary>
    public class SceneReloader : MonoBehaviour 
    {
        #region Variable Declarations 
        // Private Serializable
 
        // Private
        
        #endregion
 
 
 
        #region Unity Event Functions
        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                SceneManager.LoadScene(0);
            }
        }
        #endregion
    }
}