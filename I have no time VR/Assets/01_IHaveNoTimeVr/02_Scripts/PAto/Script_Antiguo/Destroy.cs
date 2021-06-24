using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float throwedForce = 1.5f;
    public Rigidbody rb;
    public GameObject platoRoto;
    
    public Vector3 velocidadDelPlatoXd;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude >= throwedForce)
        {
            Instantiate(platoRoto, transform.position, transform.rotation);
            
            //platoRoto.GetComponentInChildren<Rigidbody>().velocity = collision.relativeVelocity;
            Destroy(gameObject);

        }
    }
}
