using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuroRomper : MonoBehaviour
{
    public GameObject muroFragmentos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Hand")
        {
            Instantiate(muroFragmentos, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
