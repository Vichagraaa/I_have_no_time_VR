using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTransparent : MonoBehaviour
{

    [Header("Materiales")]
    public Material[] mats;


    public Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();

        Invoke("ChangeColorOne", 4f);

    }

    void ChangeColorOne()
    {
        rend.materials[0] = mats[0];
        rend.materials[1] = mats[1];
    }
}
