using UnityEngine;
using System.Collections;

public class CameraController: MonoBehaviour {

	private MapGenerator mapGen;

	private Vector3 cameraTarget;
	private Transform target;
	//public GameObject player_model;
	public string playerNumber;


	void Start(){
		target = GameObject.Find("Player" + playerNumber).transform;
		/*
		GameObject player = Instantiate(player_model) as GameObject;
		player.transform.position = new Vector3(-1,-1,-1);
		player.name = "Player";
		target = player.transform;
		*/


	}

	void Update(){
		cameraTarget = new Vector3(target.position.x, transform.position.y, target.position.z);
		transform.position = Vector3.Lerp(transform.position, cameraTarget, Time.deltaTime*8);
	}



	/*
	void Start(){
		target = GameObject.Find("Player").transform;
		mapGen = GameObject.Find("MapSpawn").GetComponent<MapGenerator>();
		float cameraLiftRatio = 0.5f;
		transform.position = new Vector3(mapGen.width / 2.0f, (mapGen.width >= mapGen.height) ? mapGen.width*cameraLiftRatio : mapGen.height*cameraLiftRatio , mapGen.height / 2.0f);
	}
	*/

}
