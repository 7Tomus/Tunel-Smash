using UnityEngine;
using System.Collections;

public class SmelterController : MonoBehaviour {

	void OnTriggerEnter(Collider c){
		if(c.tag == "Projectile"){
			return;
		}
		c.GetComponent<PlayerInventory>().isInSmelter = true;
	}

	void OnTriggerExit(Collider c){
		if(c.tag == "Projectile"){
			return;
		}
		c.GetComponent<PlayerInventory>().isInSmelter = false;
	}
}
