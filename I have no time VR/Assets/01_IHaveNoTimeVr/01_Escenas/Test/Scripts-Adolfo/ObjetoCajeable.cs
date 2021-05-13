using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoCajeable : MonoBehaviour
{
    // Objeto instanciable, la cajita.
    public GameObject cajita;
    // Tamaño que tendrá el objeto instanciado, la base es 100, por lo que tiene que ser mayor a 100.
    public float tamano;

    // Booleanos de reconocimiento de camion
    bool enCamion = false;
    


    // Revisará cuando entre en contacto con el interior del camión.
    // El interior del camión tiene que tener el tag de "camion".
    // El booleano bloqueará el que se instancien más cajas.
    private void OnTriggerEnter(Collider camion)
    {
        if (camion.CompareTag("camion"))
        {
            if(enCamion == false)
            {
                enCamion = true; // booleano para bloquear la instanciación.
                Destroy(gameObject, 3f); // Destruye el objeto a los 3 segundos.
            }
        }
    }

    // Al destruirse, instancia la cajita
    private void OnDestroy()
    {
        cajita.transform.localScale = new Vector3(tamano, tamano, tamano);  // Escala el prefab para que tenga el mismo tamaño que el objeto que quedará dentro.
        Instantiate(cajita, transform.position, transform.rotation);  // Instancia finalmente la caja.
    }
}
