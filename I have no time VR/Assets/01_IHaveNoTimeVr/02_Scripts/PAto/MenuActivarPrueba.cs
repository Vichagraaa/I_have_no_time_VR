using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SecondaryButtonEvent : UnityEvent<bool> { }

public class MenuActivarPrueba : MonoBehaviour
{
    //Duracion del nivel 
    public int levelDuration = 86;

    //Estados del juego
    public bool gameBegin = false;
    public bool isPaused = false;
    public bool gameFinish = false;

    //Contador inicial 
    public bool initialTakingAway = false;
    public int initialTimerDuration = 3;
    public GameObject contadorInicial;


    //Contador de nivel
    public GameObject contadorNivel;
    public bool takingAway = false;
    public int levelCount;


    //Post Proccesing
    public Volume m_volumen;
    public float m_speed = 2.0f;


    //Musica 
    public AudioSource levelMusic;
    public int pitchMusic = 1;
    public int rapidMusic;
    public float accelerationMusic = 1.0f;


    //Menu de pausa;
    public GameObject pauseMenu;


    //Menu final
    public GameObject finalCanvas;

    //Ira
    public GameObject nIra;
    public int countIra = 0;
    public int limitIra = 10;
    public bool restando = false;
    public bool full = false;

    public bool activar;
    public GameObject menu;

    public SecondaryButtonEvent secondaryButtonPress;

    private bool lastButtonState = false;
    private List<InputDevice> devicesWithSecondaryButton;

    public static InputFeatureUsage<bool> secondaryButton;

    // Start is called before the first frame update
    void Start()
    {
        hidePostProcessing();
        pauseMenu.SetActive(false);
        finalCanvas.SetActive(false);
        rapidMusic = (levelDuration / 4);
        contadorNivel.GetComponent<Text>().text = "00:" + levelDuration;
        contadorInicial.GetComponent<Text>().text = "00:" + initialTimerDuration;
        levelMusic.GetComponent<AudioSource>().pitch = pitchMusic;
        countIra = 0;
        nIra.GetComponent<Text>().text = "" + countIra;
    }

    // Update is called once per frame
    void Update()
    {
        if (initialTakingAway == false && initialTimerDuration >= 0)
        {
            //Debug.Log("Hola");
            StartCoroutine(InitialTimerTake());
        }
        if (initialTakingAway == false && initialTimerDuration <= 0)
        {
            gameBegin = true;
            if (takingAway == false && levelDuration > 0)
            {
                contadorInicial.SetActive(false);
                StartCoroutine(TimerTake());
            }

            if (levelCount == (levelDuration - rapidMusic))
            {
                accelerateMusic();
            }

            if (levelDuration <= 0)
            {
                normalizeMusic();
                finalCanvas.SetActive(true);
                gameFinish = true;
            }
        }

        if (countIra == limitIra)
        {
            full = true;
            showPostProcessing();
            accelerateMusic();

        }

        if (full == true && restando == false)
        {
            StartCoroutine(TakeDown());
        }


        if (countIra <= 0)
        {
            full = false;
            StopCoroutine(TakeDown());
            restando = false;
            hidePostProcessing();
            
        }

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
        slowMusic();

        menu.SetActive(true);
        activar = true;
    }
    void quitarPausa()
    {
        normalizeMusic();
        
        menu.SetActive(false);
        activar = false;
    }

    //Voids Activar y desactivar Post proccesing
    void showPostProcessing()
    {
        DOTween.To(() => m_volumen.weight, x => m_volumen.weight = x, 1, m_speed).SetDelay(1).SetEase(Ease.Linear);
    }

    void hidePostProcessing()
    {
        DOTween.To(() => m_volumen.weight, x => m_volumen.weight = x, 0, m_speed).SetDelay(1).SetEase(Ease.Linear);
    }


    //Voids Acelerar y desacelerar la musica
    void accelerateMusic()
    {
        DOTween.To(() => levelMusic.pitch, x => levelMusic.pitch = x, 2, accelerationMusic).SetDelay(1).SetEase(Ease.Linear);
    }

    void slowMusic()
    {
        DOTween.To(() => levelMusic.pitch, x => levelMusic.pitch = x, 0.2f, accelerationMusic).SetDelay(1).SetEase(Ease.Linear);
    }

    void normalizeMusic()
    {
        DOTween.To(() => levelMusic.pitch, x => levelMusic.pitch = x, 1, accelerationMusic).SetDelay(1).SetEase(Ease.Linear);
    }


    //Void sumar punto de ira 
    public void addIraPoints(int iraPoints)
    {
        countIra += iraPoints;
    }



    //Corrutina Contador inicial 
    IEnumerator InitialTimerTake()
    {
        initialTakingAway = true;
        yield return new WaitForSeconds(1);
        initialTimerDuration--;
        contadorInicial.GetComponent<Text>().text = "00:" + initialTimerDuration;
        initialTakingAway = false;
    }


    //Corrutina Contador de nivel 
    IEnumerator TimerTake()
    {
        takingAway = true;
        yield return new WaitForSeconds(1);
        levelDuration--;
        contadorNivel.GetComponent<Text>().text = "00:" + levelDuration;
        takingAway = false;
    }

    //Corrutina Ira
    IEnumerator TakeDown()
    {

        restando = true;
        yield return new WaitForSeconds(1);
        countIra--;
        nIra.GetComponent<Text>().text = "" + countIra;
        restando = false;
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
