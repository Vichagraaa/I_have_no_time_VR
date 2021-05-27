using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Autohand.Demo{
    public enum CommonButton {
        gripButton,
        menuButton,
        primaryButton,
        secondaryButton,
        triggerButton,
        primary2DAxisClick,
        primary2DAxisPress,
        secondary2DAxisClick,
        secondary2DAxisPress,
        primaryTouch,
        secondaryTouch,
    }

    public enum CommonAxis {
        trigger,
        grip
    }

    public class XRHandControllerLink : MonoBehaviour{
        public Hand hand;
        public CommonButton grabButton;
        public CommonAxis grabAxis;
        public CommonButton squeezeButton;

        XRNode role;
        bool squeezing;
        bool grabbing;
        InputDevice device;
        List<InputDevice> devices;

        private void Start(){
            if(hand.left)
                role = XRNode.LeftHand;
            else
                role = XRNode.RightHand;
            devices = new List<InputDevice>();
        }

        void Update(){
            InputDevices.GetDevicesAtXRNode(role, devices);
            if(devices.Count > 0)
                device = devices[0];

            if(device != null && device.isValid){
                //Sets hand fingers wrap
                if(device.TryGetFeatureValue(GetCommonAxis(grabAxis), out float triggerOffset)) {
                    hand.SetGrip(triggerOffset);
                }

                //Grip input
                if(device.TryGetFeatureValue(GetCommonButton(grabButton), out bool grip)) {
                    if(grabbing && !grip){
                        hand.Release();
                        hand.gripOffset -= 0.8f;
                        grabbing = false;
                    }
                    else if(!grabbing && grip){
                        hand.Grab();
                        hand.gripOffset += 0.8f;
                        grabbing = true;
                    }
                }
                //Grip input
                if(device.TryGetFeatureValue(GetCommonButton(squeezeButton), out bool squeeze)) {
                    if(squeezing && !squeeze){
                        hand.Unsqueeze();
                        squeezing = false;
                    }
                    else if(!squeezing && squeeze){
                        hand.Squeeze();
                        squeezing = true;
                    }
                }
            }
        }

        public static InputFeatureUsage<bool> GetCommonButton(CommonButton button) {
            if(button == CommonButton.gripButton)
                return CommonUsages.gripButton;
            if(button == CommonButton.menuButton)
                return CommonUsages.menuButton;
            if(button == CommonButton.primary2DAxisClick)
                return CommonUsages.primary2DAxisClick;
            if(button == CommonButton.primary2DAxisPress)
                return CommonUsages.primary2DAxisTouch;
            if(button == CommonButton.primaryButton)
                return CommonUsages.primaryButton;
            if(button == CommonButton.primaryTouch)
                return CommonUsages.primaryTouch;
            if(button == CommonButton.secondary2DAxisClick)
                return CommonUsages.secondary2DAxisClick;
            if(button == CommonButton.secondary2DAxisPress)
                return CommonUsages.secondary2DAxisTouch;
            if(button == CommonButton.secondaryButton)
                return CommonUsages.secondaryButton;
            if(button == CommonButton.secondaryTouch)
                return CommonUsages.secondaryTouch;
            
            return CommonUsages.triggerButton;
        }

        public static InputFeatureUsage<float> GetCommonAxis(CommonAxis axis) {
            if(axis == CommonAxis.grip)
                return CommonUsages.grip;
            else
                return CommonUsages.trigger;
        }
    }
}
