﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoCajeable : MonoBehaviour
{
    // Objeto instanciable, la cajita.
    public GameObject cajita;
    // Tamaño que tendrá el objeto instanciado, la base es 100, por lo que tiene que ser mayor a 100.
    public float tamano;

    // Componente Mesh Renderer para poder desactivarlo al entrar en el camión.
    MeshRenderer mr;


    // En el start se asigna el mesh renderer.
    private void Start()
    {
        mr = gameObject.GetComponent<MeshRenderer>();
    }


    // Revisará cuando entre en contacto con el interior del camión.
    // El interior del camión tiene que tener el tag de "camion".
    private void OnTriggerEnter(Collider camion)
    {
        if (camion.CompareTag("camion"))
        { 
            Invoke("ConvertirEnCaja", 3f);  // Invocará la función de instanciación de la cajita.
        }
    }

    // Función para instanciar la cajita.
    void ConvertirEnCaja()
    {
        mr.enabled = false;  // Desactiva el mesh renderer del objeto.
        cajita.transform.localScale = new Vector3(tamano, tamano, tamano);  // Escala el prefab para que tenga el mismo tamaño que el objeto que quedará dentro.
        Instantiate(cajita, transform.position, transform.rotation);  // Instancia finalmente la caja.
    }
}
