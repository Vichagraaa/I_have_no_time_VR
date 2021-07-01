using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    // Canvas de termino de nivel.
    public Canvas cFinal;

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

            if (puntos > unaEstrella && puntos < dosEstrellas)
            {
                UnaEstrella();
            }

            if (puntos > unaEstrella && puntos < tresEstrellas)
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

    }

    void UnaEstrella()
    {

    }

    void DosEstrella()
    {

    }

    void TresEstrella()
    {

    }
}
