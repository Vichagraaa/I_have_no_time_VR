using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class SecondaryButtonEvent : UnityEvent<bool> { }

public class MenuActivarPrueba : MonoBehaviour
{
    public bool activar;
    public GameObject menu;

    public SecondaryButtonEvent secondaryButtonPress;

    private bool lastButtonState = false;
    private List<InputDevice> devicesWithSecondaryButton;

    public static InputFeatureUsage<bool> secondaryButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool tempState = false;
        foreach (var device in devicesWithSecondaryButton)
        {
            bool secondaryButtonState = false;
            tempState = device.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonState) // did get a value
                        && secondaryButtonState // the value we got
                        || tempState; // cumulative result from other controllers

            if(secondaryButtonState==true)
            {
                if(activar==true)
                {
                    quitarPausa();
                }
                else
                {
                    pausa();
                }
               
            }

            
        }

        if (tempState != lastButtonState) // Button state changed since last frame
        {
            secondaryButtonPress.Invoke(tempState);
            lastButtonState = tempState;
        }

    }
    void pausa()
    {
        menu.SetActive(true);
        activar = true;
    }
    void quitarPausa()
    {
        menu.SetActive(false);
        activar = false;
    }

    





    private void InputDevices_deviceConnected(InputDevice device)
    {
        bool discardedValue;
        if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out discardedValue))
        {
            devicesWithSecondaryButton.Add(device); // Add any devices that have a primary button.
        }
    }

    private void InputDevices_deviceDisconnected(InputDevice device)
    {
        if (devicesWithSecondaryButton.Contains(device))
            devicesWithSecondaryButton.Remove(device);
    }

    void OnEnable()
    {
        List<InputDevice> allDevices = new List<InputDevice>();
        InputDevices.GetDevices(allDevices);
        foreach (InputDevice device in allDevices)
            InputDevices_deviceConnected(device);

        InputDevices.deviceConnected += InputDevices_deviceConnected;
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;
    }

    private void OnDisable()
    {
        InputDevices.deviceConnected -= InputDevices_deviceConnected;
        InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected;
        devicesWithSecondaryButton.Clear();
    }

    private void Awake()
    {
        if (secondaryButtonPress == null)
        {
            secondaryButtonPress = new SecondaryButtonEvent();
        }

        devicesWithSecondaryButton = new List<InputDevice>();
    }
}
