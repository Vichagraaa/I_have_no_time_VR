using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    // Canvas de termino de nivel.
    public Canvas cFinal;
    public GameObject Estrella1;
    public GameObject Estrella2;
    public GameObject Estrella3;

    // Puntos de jugador
    public int puntos;

    // Puntos para obtener estrellas.
    public int niunaEstrella;
    public int unaEstrella;
    public int dosEstrellas;
    public int tresEstrellas;

    // Bool para saber cuando termina el nivel.
    public bool recuento = false;


    // Hay que volver verdadero el recuento pa' que revise cuántas estrellas sacó el jugador.
    void Update()
    {
        if(recuento == true)
        {
            if (puntos < unaEstrella)
            {
                NiunaEstrella();
            }

            if (puntos >= unaEstrella )
            {
                UnaEstrella();
            }

            if (puntos >= dosEstrellas)
            {
                DosEstrella();
            }

            if (puntos >= tresEstrellas)
            {
                TresEstrella();
            }
        }
    }

    void NiunaEstrella()
    {
        Estrella1.SetActive(false);
        Estrella2.SetActive(false);
        Estrella3.SetActive(false);

    }

    void UnaEstrella()
    {
        Estrella1.SetActive(true); Estrella2.SetActive(false); Estrella3.SetActive(false);
    }

    void DosEstrella()
    {
        Estrella1.SetActive(true); Estrella2.SetActive(true); Estrella3.SetActive(false);
    }

    void TresEstrella()
    {
        Estrella1.SetActive(true); Estrella2.SetActive(true); Estrella3.SetActive(true);
    }
}
