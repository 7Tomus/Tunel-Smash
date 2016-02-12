using UnityEngine;
using System.Collections;

public class EmperiumController : MonoBehaviour {

	private Vector3 projectileDirection;
	private Vector3 projectilePosition;
	public bool isAlive = true;

	void OnTriggerEnter(Collider c){
		if(c.tag == "Projectile"){
			c.attachedRigidbody.AddForce(c.attachedRigidbody.gameObject.transform.forward * (-2*(c.GetComponent<Projectile>().speed)));
			gameObject.GetComponent<TileProperties>().durability--;
			if(gameObject.GetComponent<TileProperties>().durability <= 0){
				isAlive = false;
				gameObject.GetComponent<Light>().enabled = false;
				gameObject.GetComponent<Renderer>().material.color = new Color(0.3f,0.3f,0.3f,0.1f);
			}
			return;
		}
	}
	/*
	void OnTriggerExit(Collider c){
		if(c.tag == "Projectile"){
			//Destroy(c.attachedRigidbody.gameObject);
			return;
		}
	}
	*/
}
