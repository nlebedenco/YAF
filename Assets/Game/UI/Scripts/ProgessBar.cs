using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class ProgessBar: MonoBehaviour
{
    [SerializeField]
    private Image indicator;

    public AsyncOperation operation;
    public AsyncOperation Operation
    {
        get { return operation;  }
        set
        {
            operation = value;
        }
    }


    public float Value
    {
        get {  return indicator.fillAmount;  }
        set { indicator.fillAmount = value; }
    }

    #region MonoBehviour 

    void Update()
    {
        if (operation != null)
            Value = operation.progress;
    }

    #endregion
}
