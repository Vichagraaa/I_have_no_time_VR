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


    //Contador inicial 
    public bool initialTakingAway = false;
    public int initialTimerDuration = 3;
    public GameObject text2;
    public bool gameBegin = false;


    //Contador de nivel
    public GameObject text1;
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
    public bool isPaused = false;
    void Start()
    {
        hidePostProcessing();
        pauseMenu.SetActive(false);
        rapidMusic = (levelDuration / 4);
        text1.GetComponent<Text>().text = "00:" + levelDuration;
        text2.GetComponent<Text>().text = "00:" + initialTimerDuration;
        levelMusic.GetComponent<AudioSource>().pitch = pitchMusic;

    }

    void Update()
    {
        
        if (isPaused == false) 
        {
            if (Input.GetButtonDown("Jump"))
            {
                pauseGame();
            }
            if (initialTakingAway == false && initialTimerDuration >= 0)
            {
                Debug.Log("Hola");
                StartCoroutine(InitialTimerTake());
            }
            if (initialTakingAway == false && initialTimerDuration <= 0)
            {
                gameBegin = true;
                if (takingAway == false && levelDuration > 0)
                {
                    text2.SetActive(false);
                    StartCoroutine(TimerTake());
                }

                if (levelCount == (levelDuration - rapidMusic))
                {
                    accelerateMusic();
                    showPostProcessing();
                }
            }
        }
        if (isPaused == true) 
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

    //Corrutina Contador inicial 
    IEnumerator InitialTimerTake()
    {
        initialTakingAway = true;
        yield return new WaitForSeconds(1);
        initialTimerDuration--;
        text2.GetComponent<Text>().text = "00:" + initialTimerDuration;
        initialTakingAway = false;
    }
    
    
    //Corrutina Contador de nivel 
    IEnumerator TimerTake()
    {
        takingAway = true;
        yield return new WaitForSeconds(1);
        levelDuration--;
        text1.GetComponent<Text>().text = "00:" + levelDuration;
        takingAway = false;
    }
}
