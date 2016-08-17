using UnityEngine;

public class Shop : MonoBehaviour
{
    #region MonoBehaviour

    void Awake()
    {
        GameSystem.RegisterModule(this);
    }

    void OnDestroy()
    {

    }

    #endregion
}
