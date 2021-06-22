using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celular : MonoBehaviour
{
    public bool mano;
    // Start is called before the first frame update
    void Start()
    {
        mano = true;
    }
    private void OnTriggerEnter(Collider Cinturon)
    {
        mano = true;
    }
    private void OnTriggerExit(Collider Cinturon)
    {
        mano = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (mano ==false )
        {
            transform.parent = null;
        }
        else
        {
            return;
        }
    }
}
