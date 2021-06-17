using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    
    public GameObject prefab;
    public bool part = false;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Particula");
        
    }

   

    void OnCollisionEnter(Collision collision)
    {
        if(part==true)
        {
            // 1st point of contact
            ContactPoint contactPoint = collision.contacts[0];
            // Create the prefab at the contact point of the collision
            Instantiate(prefab, contactPoint.point, transform.rotation);
            StartCoroutine("Particula");
        }
    }
  
    IEnumerator Particula()
    {
        yield return new WaitForSeconds(0.5f);
        part = true;
    }

    
}
