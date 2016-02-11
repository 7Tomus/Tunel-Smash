using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Projectile : MonoBehaviour {

	private GameObject player0,player1,player2,player3;
	private Text Hp0,Hp1,HP2,Hp3;

	void Start(){
		player0 = GameObject.FindWithTag("Player0");
		player1 = GameObject.FindWithTag("Player1");
		player2 = GameObject.FindWithTag("Player2");
		player3 = GameObject.FindWithTag("Player3");
		Hp0 = GameObject.Find("Hp0").GetComponent<Text>();
		Hp1 = GameObject.Find("Hp1").GetComponent<Text>();
		HP2 = GameObject.Find("Hp2").GetComponent<Text>();
		Hp3 = GameObject.Find("Hp3").GetComponent<Text>();

		StartCoroutine("Fade");
	}

	void OnTriggerEnter(Collider c){
		switch(c.tag){
			case "Border":
			gameObject.GetComponent<Rigidbody>().Sleep();
			gameObject.GetComponent<BoxCollider>().enabled = false;
				break;
			case "TileDirt":
			gameObject.GetComponent<Rigidbody>().Sleep();
			gameObject.GetComponent<BoxCollider>().enabled = false;
				break;
			case "TileRock":
			gameObject.GetComponent<Rigidbody>().Sleep();
			gameObject.GetComponent<BoxCollider>().enabled = false;
				break;
			case "TileIronOre":
			gameObject.GetComponent<Rigidbody>().Sleep();
			gameObject.GetComponent<BoxCollider>().enabled = false;
				break;
			case "TileIron":
			gameObject.GetComponent<Rigidbody>().Sleep();
			gameObject.GetComponent<BoxCollider>().enabled = false;
				break;
			case "Player0":
				Hp0.text = player0.GetComponent<PlayerInventory>().changeHitPoints(-1).ToString();
				Destroy(gameObject);
				break;
			case "Player1":
				Hp1.text = player1.GetComponent<PlayerInventory>().changeHitPoints(-1).ToString();
				Destroy(gameObject);
				break;
			case "Player2":
				HP2.text = player2.GetComponent<PlayerInventory>().changeHitPoints(-1).ToString();
				Destroy(gameObject);
				break;
			case "Player3":
				Hp3.text = player3.GetComponent<PlayerInventory>().changeHitPoints(-1).ToString();
				Destroy(gameObject);
				break;
		}
	}

	IEnumerator Fade(){
		yield return new WaitForSeconds(30);
		Destroy(gameObject);
	}
}
