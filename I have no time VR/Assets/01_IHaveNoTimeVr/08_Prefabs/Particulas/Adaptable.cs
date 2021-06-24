using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adaptable : MonoBehaviour
{
    
    public GameObject prefab1;
    public bool part1 = false;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Particula1");
        
    }

   

    void OnCollisionEnter(Collision collision)
    {
        if(part1==true)
        {
            // 1st point of contact
            ContactPoint contactPoint = collision.contacts[0];
            // Create the prefab at the contact point of the collision
            Instantiate(prefab1, contactPoint.point, transform.rotation);
            StartCoroutine("Particula1");
        }
    }
  
    IEnumerator Particula1()
    {
        yield return new WaitForSeconds(0.5f);
        part1= true;
    }

    
}
