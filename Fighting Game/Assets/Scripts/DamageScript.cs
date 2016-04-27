using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.root != transform.root && col.tag != "Ground" && !col.isTrigger)
        {
            //Can add function of all player attacks so player doesn't take damage when attacking
            if (!col.transform.GetComponent<PlayerControl>().damage)
            {

                col.transform.GetComponent<PlayerControl>().damage = true;

                col.transform.root.GetComponentInChildren<Animator>().SetTrigger("Damage");

                col.transform.GetComponent<PlayerControl>().health -= 10;

               
            }
        }
    }
}
