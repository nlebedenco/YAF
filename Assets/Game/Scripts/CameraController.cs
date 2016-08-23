using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private float smoothTime = 1.0f;

    [SerializeField]
    private bool lockHorizontalMovement = true;

    [SerializeField]
    private float moveOffset = 3.0f;

    private Vector3 targetPosition = Vector3.zero;
    private TargetController targetController;
    private Vector3 initialCameraPosition;

    #region MonoBehaviour

    void Awake()
    {
        targetController = GetComponent<TargetController>();
    }

    void Start() 
    {
        initialCameraPosition = Camera.main.transform.position;
    }

    void LateUpdate () 
    {
        // Lock the target controllers based on Z axis.
        targetController.cameraLock = (transform.position.z <= Camera.main.transform.position.z);

        targetPosition = initialCameraPosition;

        if (!lockHorizontalMovement)
            targetPosition.x = transform.position.x;

        targetPosition.z = Camera.main.transform.position.z;

        if (transform.position.z > (targetPosition.z + moveOffset))
            targetPosition.z = transform.position.z - moveOffset;

        Camera.main.transform.position = targetPosition;
    }

    #endregion
}
