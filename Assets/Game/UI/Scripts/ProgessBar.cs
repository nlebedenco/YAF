using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class ProgessBar: MonoBehaviour
{
    [SerializeField]
    private RectTransform progressTransform;

    [SerializeField]
    private Text progressText;

    [SerializeField]
    private float progressWidth;

    private float fillAmount = 0.1f;

    public AsyncOperation operation;
    public AsyncOperation Operation
    {
        get { return operation;  }
        set { operation = value; }
    }
        
    public float Value 
    {
        get { return fillAmount; }
        set 
        {
            if (value < 0.1f)
                value = 0.1f;
            fillAmount = Mathf.Clamp01(value);

            if (progressTransform != null)
            {
                progressTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (progressWidth * fillAmount));
            }
        }
    }

    public string Text
    {
        get { return progressText.text; }
        set { progressText.text = value; }
    }

    #region MonoBehviour 

    void Update()
    {
        if (operation != null)
            Value = operation.progress;
    }
        
    #endregion
}
