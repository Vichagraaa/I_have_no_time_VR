using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIpoDeObjeto : MonoBehaviour
{
    public enum TamanoOjeto 
    {
        Grande,
        Mediano,
        Chico
    }
    public TamanoOjeto tamanoObjeto;
    public int iraPoints = 0;
    public bool sumeIra = false;
    // Start is called before the first frame update
    void Start()
    {
        if (tamanoObjeto == TamanoOjeto.Grande) 
        {
            iraPoints = 3;
        }

        if (tamanoObjeto == TamanoOjeto.Mediano)
        {
            iraPoints = 2;
        }

        if (tamanoObjeto == TamanoOjeto.Chico)
        {
            iraPoints = 1;
        }
    }

    public void OnTriggerEnter(Collider collision) 
    {
        if (collision.CompareTag("PLayer") && sumeIra == false) 
        {
            GameManager gamemanager = collision.GetComponent<GameManager>();
            gamemanager.addIraPoints(iraPoints);
            sumeIra = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
