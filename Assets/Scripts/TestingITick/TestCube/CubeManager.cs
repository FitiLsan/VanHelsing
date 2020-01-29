using UnityEngine;


public class CubeManager : MonoBehaviour
{

    public CubeController controller { get; private set; }

    public CubeScriptableObj _cubdata;

    #region UnityMetods
    private void Awake()
    {
        CubeLoad();
    }
    #endregion

    #region Metods
    private void CubeLoad()
    {     
        controller = new CubeController(_cubdata);
        controller.OnAwake();
    }
    #endregion 
}
