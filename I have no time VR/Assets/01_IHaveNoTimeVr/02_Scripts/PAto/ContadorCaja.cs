using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContadorCaja : MonoBehaviour
{
    int contador;
    public Text objetos;
    public int objetosTotal;
    public Text win;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider caja)
    {
        if(caja.CompareTag("Caja"))
        {
            contador = contador + 1;
            actualizar();

        }
        if(contador==objetosTotal)
        {
            win.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider caja)
    {
        if (caja.CompareTag("Caja"))
        {
            contador = contador - 1;
            actualizar();

        }
    }
    private void actualizar()
    {
        objetos.text = "Objeto:" + contador + " / " + objetosTotal;
    }

    private void Awake()
    {
        contador = 0;
        actualizar();
        win.gameObject.SetActive(false);
    }
}
