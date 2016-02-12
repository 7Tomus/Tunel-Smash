using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour {

	private int hitPoints;
	public int ironOre;
	public int iron;
	public int carryLimit;
	private int playerNumber;
	private Text ironOreText,hpText;
	public bool isInSmelter = false;
	private bool isSmelting = false;
	private Text ironText;
	private int oldPositionX = 0;
	private int oldPositionZ = 0;
	private GameObject mapSpawn;
	private List<GameObject> buildCubeList;
	private float buildCubeHeight;
	GameObject cubeN, cubeS, cubeW, cubeE;
	public int startX, startY;
	public int numberOfDeaths = 0;
	private Color startColor;

	void Start () {
		hitPoints = 10;
		iron = 0;
		ironOre = 0;
		carryLimit = 30;
		playerNumber = gameObject.GetComponent<PlayerController>().playerNumber;
		ironOreText = GameObject.Find("IronOre" + playerNumber).GetComponent<Text>();
		ironText = GameObject.Find("Iron" + playerNumber).GetComponent<Text>();
		mapSpawn = GameObject.FindGameObjectWithTag("MapSpawn");
		buildCubeHeight = mapSpawn.GetComponent<MapGenerator>().cubeHeight;
		startColor = gameObject.GetComponent<Renderer>().material.color;
		hpText = GameObject.Find("Hp" + playerNumber).GetComponent<Text>();
	}

	void Update () {
		if(isInSmelter){
			if(!isSmelting){
				isSmelting = true;
				StartCoroutine("Smelting");
			}
		}
	}

	public void setStartingPosition(int x,int y){
		startX = x;
		startY = y;
	}

	IEnumerator respawn(){
		GameObject emperium = GameObject.Find("Emperium"+playerNumber);
		if(emperium.GetComponent<EmperiumController>().isAlive){
			yield return new WaitForSeconds(5*numberOfDeaths+10);
			numberOfDeaths += 1;
			gameObject.transform.position = new Vector3(startX, 0.5f, startY);
			yield return new WaitForSeconds(1);
			gameObject.GetComponent<Renderer>().material.color = startColor;
			hitPoints = 10;
			hpText.text = hitPoints.ToString();
			gameObject.GetComponent<PlayerController>().enabled = true;
		}
	}

	public int getPlayerNumber(){
		return playerNumber;
	}

	public int changeHitPoints(int change){
		hitPoints += change;
		if(hitPoints == 0){
			gameObject.GetComponent<Renderer>().material.color = Color.black;
			gameObject.GetComponent<PlayerController>().enabled = false;
			StartCoroutine(respawn());
		}
		return hitPoints;
	}

	public int changeIronOre(int change){
		ironOre += change;
		if(ironOre>carryLimit){
			ironOre = carryLimit;
		}
		ironOreText.text = ironOre.ToString();

		return ironOre;
	}

	IEnumerator Smelting(){
		yield  return new WaitForSeconds(0.5f);

		if(ironOre>=3 && isInSmelter){
			ironOre -= 3;
			iron += 1;
			ironOreText.text = ironOre.ToString();
			ironText.text = iron.ToString();
		}
		isSmelting =false;
	}

	void PositionPrint(){
		if(Mathf.Round(gameObject.transform.position.x) != oldPositionX || Mathf.Round(gameObject.transform.position.z) != oldPositionZ){
			print(playerNumber + ": " + Mathf.Round(gameObject.transform.position.x) + " " + Mathf.Round(gameObject.transform.position.z));
			oldPositionX = (int)Mathf.Round(gameObject.transform.position.x);
			oldPositionZ = (int)Mathf.Round(gameObject.transform.position.z);

			GameObject cub = mapSpawn.GetComponent<MapGenerator>().cubeList.Find(GameObject => GameObject.transform.position == new Vector3(oldPositionX,mapSpawn.GetComponent<MapGenerator>().cubeHeight/2,oldPositionZ));
			cub.SetActive(true);
			BoxCollider[] boxColliders = cub.GetComponents<BoxCollider>();

			foreach(BoxCollider thisBoxCollider in boxColliders){
				thisBoxCollider.enabled = false;
			}
			cub.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
			cub.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
		}
	}

	public void BuildingMenuOn(){
		gameObject.GetComponent<PlayerController>().movementEnabled = false;
		fillCubeObjectsInCross();
		showBuildingTarget(cubeN);
		showBuildingTarget(cubeS);
		showBuildingTarget(cubeE);
		showBuildingTarget(cubeW);
	}

	public void BuildingMenuOff(){
		gameObject.GetComponent<PlayerController>().movementEnabled = true;
		fillCubeObjectsInCross();
		hideBuildingTarget(cubeN);
		hideBuildingTarget(cubeS);
		hideBuildingTarget(cubeE);
		hideBuildingTarget(cubeW);
	}

	private void fillCubeObjectsInCross(){
		buildCubeList = mapSpawn.GetComponent<MapGenerator>().cubeList;
		int posX = (int)Mathf.Round(gameObject.transform.position.x);
		int posZ = (int)Mathf.Round(gameObject.transform.position.z);
		cubeN = buildCubeList.Find(GameObject => GameObject.transform.position == new Vector3(posX,buildCubeHeight/2,posZ+1));
		cubeS = buildCubeList.Find(GameObject => GameObject.transform.position == new Vector3(posX,buildCubeHeight/2,posZ-1));
		cubeE = buildCubeList.Find(GameObject => GameObject.transform.position == new Vector3(posX+1,buildCubeHeight/2,posZ));
		cubeW = buildCubeList.Find(GameObject => GameObject.transform.position == new Vector3(posX-1,buildCubeHeight/2,posZ));
	}

	private void showBuildingTarget(GameObject cube){
		if(!cube.activeInHierarchy){
			cube.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
			cube.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
			BoxCollider[] boxColliders = cube.GetComponents<BoxCollider>();
			foreach(BoxCollider thisBoxCollider in boxColliders){
				thisBoxCollider.enabled = false;
			}
			cube.tag = "BuildMenu";
			cube.SetActive(true);
		}
	}

	private void hideBuildingTarget(GameObject cube){
		if(cube.activeInHierarchy){
			if(cube.GetComponent<Renderer>().material.color == new Color(1f,0f,0f,0.1f)){
				if(iron>=1){
					iron -= 1;
					ironText.text = iron.ToString();
					cube.GetComponent<TileProperties>().SetIron();
					BoxCollider[] boxColliders = cube.GetComponents<BoxCollider>();
					foreach(BoxCollider thisBoxCollider in boxColliders){
						thisBoxCollider.enabled = true;
					}
				}
				else{
					cube.SetActive(false);
				}
			}
			if(cube.GetComponent<Renderer>().material.color == new Color(1f,1f,1f,0.1f)){
				cube.SetActive(false);
			}
		}
	}

	public void sideChoice(int choice){
		switch(choice){
			case 0: {
				if(cubeW.tag == "BuildMenu"){
					cubeW.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				if(cubeS.tag == "BuildMenu"){
					cubeS.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				if(cubeE.tag == "BuildMenu"){
					cubeE.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				if(cubeN.tag == "BuildMenu"){
					cubeN.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				break;
				}
		case -1: {
				if(cubeW.tag == "BuildMenu"){
					cubeW.GetComponent<Renderer>().material.color = new Color(1f,0f,0f,0.1f);
				}
				if(cubeS.tag == "BuildMenu"){
					cubeS.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				if(cubeE.tag == "BuildMenu"){
					cubeE.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				if(cubeN.tag == "BuildMenu"){
					cubeN.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				break;
			}
		case 1: {
				if(cubeW.tag == "BuildMenu"){
					cubeW.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				if(cubeS.tag == "BuildMenu"){
					cubeS.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				if(cubeE.tag == "BuildMenu"){
					cubeE.GetComponent<Renderer>().material.color = new Color(1f,0f,0f,0.1f);
				}if(cubeN.tag == "BuildMenu"){
					cubeN.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				break;
			}
		case 2: {
				if(cubeW.tag == "BuildMenu"){
					cubeW.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				if(cubeS.tag == "BuildMenu"){
					cubeS.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				if(cubeE.tag == "BuildMenu"){
					cubeE.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				if(cubeN.tag == "BuildMenu"){
					cubeN.GetComponent<Renderer>().material.color = new Color(1f,0f,0f,0.1f);
				}
				break;
			}
		case -2: {
				if(cubeW.tag == "BuildMenu"){
					cubeW.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				if(cubeS.tag == "BuildMenu"){
					cubeS.GetComponent<Renderer>().material.color = new Color(1f,0f,0f,0.1f);
				}
				if(cubeE.tag == "BuildMenu"){
					cubeE.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				if(cubeN.tag == "BuildMenu"){
					cubeN.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0.1f);
				}
				break;
			}
		}
	}
}