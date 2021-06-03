using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ira : MonoBehaviour
{
    public GameObject nIra;
    public int countIra = 0;
    public int limitIra = 10;
    public bool restando = false;
    public bool full = false;
    // Start is called before the first frame update
    void Start()
    {
        countIra = 0;
        nIra.GetComponent<Text>().text = "" +countIra;


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && restando == false) 
        {
            countIra++;
            nIra.GetComponent<Text>().text = "" + countIra;
        }

        if (countIra == limitIra) 
        {
            full = true; 
           
        }

        if (full == true && restando == false) 
        {
            StartCoroutine(TakeDown());
        }


        if (countIra == 0) 
        {
            full = false;
            StopAllCoroutines();
            restando = false;
        }

    }

    IEnumerator TakeDown() 
    {

        restando = true;
        yield return new WaitForSeconds(1);
        countIra--;
        nIra.GetComponent<Text>().text = "" + countIra;
        restando = false; 
    }
}
