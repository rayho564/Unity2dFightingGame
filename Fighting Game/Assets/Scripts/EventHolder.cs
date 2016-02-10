using UnityEngine;
using System.Collections;

public class EventHolder : MonoBehaviour {

    PlayerControl pl;

	void Start () {
        pl = transform.root.GetComponent<PlayerControl>();
	}
	
	public void ThrowProjectile()
    {
        pl.specialAttack = true;
    }
}
