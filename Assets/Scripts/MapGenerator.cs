using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapGenerator : MonoBehaviour {

	[Range(32,128)]
	public int width =  64, height = 64; //Defines size of map
	[Range(1,2)]
	public float cubeHeight = 1.2f; //Defines height of tiles, >1 for disabling jumping out of bounds
	public GameObject tile, groundQuad;
	public List<GameObject> cubeList = new List<GameObject>(), playerList = new List<GameObject>();
	[Range(46,50)]
	public int rockFillPercent = 48;
	[Range(40,55)]
	public int ironOreFillPercent = 44;
	[Range(1,6)]
	public int numberOfTeams = 4;
	private GameObject playerModel;

	private int[] proximityX, proximityY; //used for spacing bases
	private int[,] rockMap, ironOreMap; //Random maps for map generation
	private System.Random pseudoRandom = new System.Random();

	void Start () {
		proximityX = new int[numberOfTeams];
		proximityY = new int[numberOfTeams];
		rockMap = new int[width,height];
		ironOreMap = new int[width,height];
		GenerateMap(rockMap,rockFillPercent,5);
		StartCoroutine(DelayedMethods());
	}

	private void GenerateMap(int[,]map, int fillPercent, int smoothAmmount){
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {				
				map[x,y] = (pseudoRandom.Next(0,100) < fillPercent)? 1: 0;
			}
		}
		for (int i = 0; i < smoothAmmount; i ++) {
			SmoothMap(map);
		}
	}

	private void SmoothMap(int[,] map){
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int neighbourWallTiles = GetSurroundingWallCount(x,y,map);
				if (neighbourWallTiles > 4) map[x,y] = 1;
				else if (neighbourWallTiles < 4) map[x,y] = 0;
			}
		}
	}

	int GetSurroundingWallCount(int gridX, int gridY, int[,] map) {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) {
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += map[neighbourX,neighbourY];
					}
				}
				else {
					wallCount ++;
				}
			}
		}
		return wallCount;
	}


	IEnumerator DelayedMethods(){
		yield return new WaitForSeconds(0.01f);
		GenerateMap(ironOreMap,ironOreFillPercent,2);
		FillMap();
		for(int i = 0; i<numberOfTeams; i++){
			playerModel = GameObject.FindWithTag("Player" + i);
			MakeBase(i);		//a tym padom sa nevytvorili na tom istom mieste
			yield return new WaitForSeconds(0.01f);
		}
	}

	private void FillMap(){
		GameObject floor = Instantiate(groundQuad) as GameObject; //floor quad
		floor.name = "Ground";
		floor.transform.position = new Vector3(width/2.0f-0.5f, 0, height/2.0f-0.5f);
		floor.transform.localScale = new Vector3(width,height,1);

		for(int x = 0; x < width;x++){
			for(int y = 0; y < height; y++){
				GameObject newCube = Instantiate(tile) as GameObject;
				newCube.transform.localScale = new Vector3(1f, cubeHeight, 1f);
				newCube.transform.position = new Vector3(x, cubeHeight/2, y);
				if(rockMap[x,y] == 0){
					newCube.GetComponent<TileProperties>().SetDirt(x*height+y);
				}
				else{
					if(rockMap[x,y] == ironOreMap[x,y]){
						newCube.GetComponent<TileProperties>().SetIronOre(x*height+y);
					}
					else{
						newCube.GetComponent<TileProperties>().SetRock(x*height+y);
					}
				}
				if(x == 0 || y == 0 || x == width-1 || y == height-1){
					newCube.GetComponent<TileProperties>().SetBorder(x*height+y);
				}

				cubeList.Add(newCube);
			}
		}
	}

	private void MakeBase(int teamNumber){
		int posX = pseudoRandom.Next(4,width-4); // 4 aby to neslo mimo mapu a aby bolo okolo zakladne aspon 1 policko
		int posY = pseudoRandom.Next(4,height-4);
		while (ProximityCheck(posX,posY) == false){
			posX = pseudoRandom.Next(4,width-4);
			posY = pseudoRandom.Next(4,height-4);
		}
		proximityX[teamNumber] = posX;
		proximityY[teamNumber] = posY;
		for(int x = posX-2; x <= posX+2; x++){
			for(int y = posY-2; y <= posY+2; y++){
				GameObject currentCube = cubeList.Find(GameObject => GameObject.transform.position == new Vector3(x,cubeHeight/2,y));
				currentCube.GetComponent<TileProperties>().SetIron(x*height+y);
			}
		}
		for(int x = posX-1; x <= posX+1; x++){ //vyrobi prazdnu dieru v zakladni
			for(int y = posY-1; y <= posY+1; y++){
				cubeList.Find(GameObject => GameObject.transform.position == new Vector3(x,cubeHeight/2,y)).SetActive(false);
			}
		}
		playerModel.transform.position = new Vector3(posX+1, 0.5f, posY-1);
		playerModel.GetComponent<PlayerInventory>().setStartingPosition(posX,posY);
		playerList.Add(playerModel);


		cubeList.Find(GameObject => GameObject.transform.position == new Vector3(posX,cubeHeight/2,posY+2)).SetActive(false); //make entrances
		cubeList.Find(GameObject => GameObject.transform.position == new Vector3(posX,cubeHeight/2,posY-2)).SetActive(false);

		GameObject smelter = cubeList.Find(GameObject => GameObject.transform.position == new Vector3(posX+1,cubeHeight/2,posY+1)); //smelter
		smelter.transform.position = new Vector3(posX+1,0.1f,posY+1);
		smelter.GetComponent<TileProperties>().SetSmelter(teamNumber);
		GameObject emperium = cubeList.Find(GameObject => GameObject.transform.position == new Vector3(posX,cubeHeight/2,posY)); //emperium
		emperium.GetComponent<TileProperties>().SetEmperium(teamNumber);

	}

	bool ProximityCheck(int posX, int posY){
		int distance = width>height?width/numberOfTeams:height/numberOfTeams;
		if(proximityX == null || proximityY == null){
			return true;
		}
		for(int i = 0; i<numberOfTeams; i++){
			if(Between(proximityX[i]-posX,-distance,distance) && Between(proximityY[i]-posY,-distance,distance)){
				return false;				
			}	
		}
		return true;
	}

	bool Between(int num, int lower, int upper){
		return lower <= num && num <= upper;
	}


}
