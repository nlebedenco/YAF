using System;
using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput.PlatformSpecific
{
    public class StandaloneInput : VirtualInput
    {
        public override float GetAxis(string name, bool raw)
        {
            CrossPlatformInputManager.VirtualAxis axis;
            if (m_VirtualAxes.TryGetValue(name, out axis))
                return axis.GetValue;

            return raw ? Input.GetAxisRaw(name) : Input.GetAxis(name);
        }


        public override bool GetButton(string name)
        {
            CrossPlatformInputManager.VirtualButton button;
            if (m_VirtualButtons.TryGetValue(name, out button))
                return button.GetButton;

            return Input.GetButton(name);
        }


        public override bool GetButtonDown(string name)
        {
            CrossPlatformInputManager.VirtualButton button;
            if (m_VirtualButtons.TryGetValue(name, out button))
                return button.GetButtonDown;

            return Input.GetButtonDown(name);
        }


        public override bool GetButtonUp(string name)
        {
            CrossPlatformInputManager.VirtualButton button;
            if (m_VirtualButtons.TryGetValue(name, out button))
                return button.GetButtonUp;

            return Input.GetButtonUp(name);
        }


        public override void SetButtonDown(string name)
        {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override void SetButtonUp(string name)
        {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override void SetAxisPositive(string name)
        {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override void SetAxisNegative(string name)
        {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override void SetAxisZero(string name)
        {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override void SetAxis(string name, float value)
        {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override Vector3 MousePosition()
        {
            return Input.mousePosition;
        }
    }
}