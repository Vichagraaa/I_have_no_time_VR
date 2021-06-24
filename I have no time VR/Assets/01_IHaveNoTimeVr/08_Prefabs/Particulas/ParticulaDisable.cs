using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticulaDisable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("Disable");
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
