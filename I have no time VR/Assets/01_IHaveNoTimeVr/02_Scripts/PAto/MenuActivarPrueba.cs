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

    public int minutos = 1;
    public int levelDuration = 30;

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
    public AudioSource rageMusic;
    public int volumeRageMusic;
    public int volumeMusic;
    public int pitchMusic = 1;
    public int rapidMusic;
    public float accelerationMusic = 1.0f;
    public AudioSource Alarma;
  


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
        levelMusic.GetComponent<AudioSource>().volume = volumeMusic;
        rageMusic.GetComponent<AudioSource>().volume = volumeRageMusic;
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

            if (levelCount == (levelDuration - rapidMusic)&& minutos== 0)
            {
                accelerateMusic();
            }
            if (levelDuration<=0 && minutos>0)
            {
                minutos--;
                Alarma.Play();
                levelDuration = 60;

            }
            if (levelDuration <= 0 && minutos ==0)
            {
                normalizeMusic();
                finalCanvas.SetActive(true);
                gameFinish = true;
            }
        }
        if(Time.timeScale==0)
        {
            slowMusic();
        }
       

        if (countIra >= limitIra)
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
            normalizeMusic();
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
        Time.timeScale = 0;
        menu.SetActive(true);
        activar = true;
        
    }
    void quitarPausa()
    {
       
        normalizeMusic();
        Time.timeScale = 1;
        menu.SetActive(false);
        activar = false;
    }

    //Voids Activar y desactivar Post proccesing
    void showPostProcessing()
    {
        DOTween.To(() => m_volumen.weight, x => m_volumen.weight = x, 1, m_speed).SetDelay(1).SetEase(Ease.Linear);
        subirVolumenIra();
    }

    void hidePostProcessing()
    {
        DOTween.To(() => m_volumen.weight, x => m_volumen.weight = x, 0, m_speed).SetDelay(1).SetEase(Ease.Linear);
        bajarVolumenIra();
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

    void subirVolumenIra()
    {
        DOTween.To(() => rageMusic.volume, x => rageMusic.volume = x, 1, volumeRageMusic).SetDelay(1).SetEase(Ease.Linear);
        DOTween.To(() => levelMusic.volume, x => levelMusic.volume = x, 0, volumeMusic).SetDelay(1).SetEase(Ease.Linear);
    }

    void bajarVolumenIra()
    {
        DOTween.To(() => rageMusic.volume, x => rageMusic.volume = x, 0, volumeRageMusic).SetDelay(1).SetEase(Ease.Linear);
        DOTween.To(() => levelMusic.volume, x => levelMusic.volume = x, 1, volumeMusic).SetDelay(1).SetEase(Ease.Linear);
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
        
        if(minutos>=10)
        {
            contadorNivel.GetComponent<Text>().text = minutos + ":" + levelDuration;
        }
        if (minutos >= 10 && levelDuration<=9)
        {
            contadorNivel.GetComponent<Text>().text = minutos + ":0" + levelDuration;
        }
        if(minutos<=9)
        {
            contadorNivel.GetComponent<Text>().text = "0" + minutos + ":" + levelDuration;
        }
        if (minutos<=9 &&levelDuration<=9)
        {
            contadorNivel.GetComponent<Text>().text = "0" + minutos + ":0" + levelDuration;
        }
        takingAway = false;
    }

    //Corrutina Ira
    IEnumerator TakeDown()
    {

        restando = true;
        yield return new WaitForSeconds(1);
        countIra--;
        
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
