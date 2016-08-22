using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private float smoothTime = 0.5f;

    [SerializeField]
    private float verticalOffset = 0.0f;

    [SerializeField]
    private float horizontalOffset = 0.0f;

    [SerializeField]
    private bool lockHorizontalMovement = false;

    [SerializeField]
    private float moveOffset = 0.0f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition = Vector3.zero;
    private TargetController targetController;

    private float maxHorizontalPlayerPosition = 0.0f;

    [SerializeField]
    private bool moving = false;

    #region MonoBehaviour

    void Awake()
    {
        targetController = GetComponent<TargetController>();
    }

    void Start() 
    {
        //initialCameraPosition = Camera.main.transform.position;
    }

    void Update () 
    {
        // Lock the target controllers based on Z axis.
        targetController.cameraLock = (transform.position.z <= Camera.main.transform.position.z);

        if ((transform.position.z - Camera.main.transform.position.z) > moveOffset)
        {
            targetPosition = Camera.main.transform.position;
            targetPosition.z = transform.position.z;
            moving = true;
        }

        if (targetPosition.z - Camera.main.transform.position.z < 0.05)
            moving = false;

        if (moving)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, smoothTime * Time.deltaTime);
        }
    }

    #endregion
}
