using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Sword_Controller : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag("EnemySword") )
        {
             other.gameObject.GetComponent<Death_Animation>().enabled = true;
             other.gameObject.GetComponent<khatam>().enabled = true;
        }
        if (other.gameObject.transform.CompareTag("DeathGirl"))
        {
            other.gameObject.GetComponent<death_girl>().enabled = true;   
        }
    }
}
