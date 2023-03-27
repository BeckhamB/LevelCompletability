using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPowerUp : MonoBehaviour
{
    public GameObject newGamObj;
    public LayerMask ground;
    private GameObject hitObj;
    RaycastHit[] hitsF;
    RaycastHit[] hitsR;
    RaycastHit[] hitsB;
    RaycastHit[] hitsL;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            CrossSpawner();
        }
    }
    private void CrossSpawner()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, 30f, ground))
        {
            hitObj = hit.transform.gameObject;

            hitsF = Physics.RaycastAll(hit.transform.position, hit.transform.TransformDirection(Vector3.forward), 30f, ground);
            foreach (RaycastHit ray in hitsF)
            {
                hitObj = ray.transform.gameObject;
                Instantiate(newGamObj, ray.transform.position, ray.transform.rotation);
                hitObj.SetActive(false);
            }
            hitsR = Physics.RaycastAll(hit.transform.position, hit.transform.TransformDirection(Vector3.right), 30f, ground);
            foreach (RaycastHit ray in hitsR)
            {
                hitObj = ray.transform.gameObject;
                Instantiate(newGamObj, ray.transform.position, ray.transform.rotation);
                hitObj.SetActive(false);
            }
            hitsB = Physics.RaycastAll(hit.transform.position, hit.transform.TransformDirection(Vector3.back), 30f, ground);
            foreach (RaycastHit ray in hitsB)
            {
                hitObj = ray.transform.gameObject;
                Instantiate(newGamObj, ray.transform.position, ray.transform.rotation);
                hitObj.SetActive(false);
            }
            hitsL = Physics.RaycastAll(hit.transform.position, hit.transform.TransformDirection(Vector3.left), 30f, ground);
            foreach (RaycastHit ray in hitsL)
            {
                hitObj = ray.transform.gameObject;
                Instantiate(newGamObj, ray.transform.position, ray.transform.rotation);
                hitObj.SetActive(false);
            }
        }
        this.gameObject.SetActive(false);
    }
}

