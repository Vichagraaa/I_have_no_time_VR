using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
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

    void Update()
    {
        
        if (isPaused == false && gameFinish == false) 
        {
            if (Input.GetButtonDown("Jump"))
            {
                pauseGame();
            }
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
            if (Input.GetButtonDown("Fire2") && restando == false)
            {
                countIra++;
                nIra.GetComponent<Text>().text = "" + countIra;
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
                normalizeMusic();
            }
        }
        if (isPaused == true && gameFinish == false) 
        {
            if (Input.GetButtonDown("Fire1"))
            {
                resumeGame();

            }
        }

            
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


    // Voids pausar y despausar el juego
    public void pauseGame()
    {
        slowMusic();
        StopAllCoroutines();
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void resumeGame()
    {
        normalizeMusic();
       if (gameBegin == true) 
        {
            StartCoroutine(TimerTake());
        }
       else 
        {
            StartCoroutine(InitialTimerTake());
        }
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    //Voids para cambio de escena y quitar la app

    public void nextScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void loadMenu() 
    {
        SceneManager.LoadScene(0);
    }

    public void quitGame() 
    {
        Application.Quit();
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
}
