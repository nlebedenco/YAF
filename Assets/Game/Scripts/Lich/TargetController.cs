using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class TargetController : MonoBehaviour {

    public enum Mode
    {
        TargetRelative,     // Move relative to target
        ViewportRelative    // Move relative to the viewport
    }

    public enum AxisOption
    {
        Both,               // Use both
        OnlyHorizontal,     // Only horizontal
        OnlyVertical        // Only vertical
    }

    [Serializable]
    private struct Side
    {
        public AxisOption axesToUse;
        public string horizontalAxisName;
        public string verticalAxisName;

        public Side(AxisOption axesToUse, string horizontalAxisName, string verticalAxisName)
        {
            this.axesToUse = axesToUse;
            this.horizontalAxisName = horizontalAxisName;
            this.verticalAxisName = verticalAxisName;
        }
    }
        
    [SerializeField]
    private Side leftController = new Side(AxisOption.Both, "LHorizontal", "LVertical");
    private bool leftUseX;
    private bool leftUseY;

    [SerializeField]
    private Side rightController = new Side(AxisOption.Both, "RHorizontal", "RVertical");
    private bool rightUseX;
    private bool rightUseY;

    [Space(10)]
    [SerializeField]
    private Mode controllerMode = Mode.TargetRelative;

    [SerializeField]
    private float moveSpeed = 5.0f;

    [SerializeField]
    private float rotateSpeed = 10.0f;

    [SerializeField]
    private float aperture = 90.0f;

    private CharacterController characterController;
    private Vector3 leftVirtualAxis = Vector3.zero;
    private Vector3 rightVirtualAxis = Vector3.zero;
    private Vector3 forwardProjection;
    private Vector3 rightProjection;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    #region MonoBehaviour

	void Awake () {
        characterController = GetComponent<CharacterController>();
	}
	
    void Start() {
        leftUseX = (leftController.axesToUse == AxisOption.Both) || (leftController.axesToUse == AxisOption.OnlyHorizontal);
        leftUseY = (leftController.axesToUse == AxisOption.Both) || (leftController.axesToUse == AxisOption.OnlyVertical);
        rightUseX = (rightController.axesToUse == AxisOption.Both) || (rightController.axesToUse == AxisOption.OnlyHorizontal);
        rightUseY = (rightController.axesToUse == AxisOption.Both) || (rightController.axesToUse == AxisOption.OnlyVertical);

        if (controllerMode == Mode.ViewportRelative)
        {
            forwardProjection = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
            rightProjection = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;
            initialRotation = transform.rotation;
        }
    }

	void Update () {
        if (leftUseX)
            leftVirtualAxis.x = CrossPlatformInputManager.GetAxis(leftController.horizontalAxisName);
        if (leftUseY)
            leftVirtualAxis.z = CrossPlatformInputManager.GetAxis(leftController.verticalAxisName);

        if (rightUseX)
            rightVirtualAxis.x = CrossPlatformInputManager.GetAxis(rightController.horizontalAxisName);
        if (rightUseY)
            rightVirtualAxis.z = CrossPlatformInputManager.GetAxis(rightController.verticalAxisName);

        switch (controllerMode)
        {
            case Mode.TargetRelative:
                // Movement
                characterController.Move(moveSpeed * leftVirtualAxis.z * transform.forward * Time.deltaTime);

                // Movement
                transform.Rotate(rotateSpeed * leftVirtualAxis.x * Vector3.up * Time.deltaTime);
                break;

            case Mode.ViewportRelative:
                // Movement
                characterController.Move(moveSpeed * (rightProjection * leftVirtualAxis.x + forwardProjection * leftVirtualAxis.z) * Time.deltaTime);

                // Rotation
                if (rightVirtualAxis.magnitude > 0) 
                {
                    Quaternion q = Quaternion.LookRotation(rightVirtualAxis);
                    if (Quaternion.Angle(q, initialRotation) <= aperture)
                        targetRotation = q;
                    
                    transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                }
                break;

            default:
                Debug.LogError("No controller mode selected!");
                break;
        }
	}

    #endregion
}