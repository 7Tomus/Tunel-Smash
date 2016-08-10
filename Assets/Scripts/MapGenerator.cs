using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

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
	[Range(2,4)]
	public int numberOfTeams;
	[Range(2,4)]
	public int numberOfPlayers;
	private GameObject playerModel;
	private int posX, posY;
	private int playersPositioned;

	private int[] proximityX, proximityY; //used for spacing bases
	private int[,] rockMap, ironOreMap; //Random maps for map generation
	private System.Random pseudoRandom = new System.Random();

	void Start () {
		SplitScreen();
		proximityX = new int[numberOfTeams];
		proximityY = new int[numberOfTeams];
		rockMap = new int[width,height];
		ironOreMap = new int[width,height];
		GenerateMap(rockMap,rockFillPercent,5);
		DistributePlayersIntoTeams();
		StartCoroutine(DelayedMethods());
	}

	void SplitScreen(){
		Camera C0,C1,C2,C3;
		RectTransform Hp0,Hp1,Hp2,Hp3,canvas,IronOre0,IronOre1,IronOre2,IronOre3,Iron0,Iron1,Iron2,Iron3;
		canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();

		C0 = GameObject.Find("Player0Cam").GetComponent<Camera>();
		C1 = GameObject.Find("Player1Cam").GetComponent<Camera>();
		C2 = GameObject.Find("Player2Cam").GetComponent<Camera>();
		C3 = GameObject.Find("Player3Cam").GetComponent<Camera>();

		Hp0 = GameObject.Find("Hp0").GetComponent<RectTransform>();
		Hp1 = GameObject.Find("Hp1").GetComponent<RectTransform>();
		Hp2 = GameObject.Find("Hp2").GetComponent<RectTransform>();
		Hp3 = GameObject.Find("Hp3").GetComponent<RectTransform>();

		IronOre0 = GameObject.Find("IronOre0").GetComponent<RectTransform>();
		IronOre1 = GameObject.Find("IronOre1").GetComponent<RectTransform>();
		IronOre2 = GameObject.Find("IronOre2").GetComponent<RectTransform>();
		IronOre3 = GameObject.Find("IronOre3").GetComponent<RectTransform>();

		Iron0 = GameObject.Find("Iron0").GetComponent<RectTransform>();
		Iron1 = GameObject.Find("Iron1").GetComponent<RectTransform>();
		Iron2 = GameObject.Find("Iron2").GetComponent<RectTransform>();
		Iron3 = GameObject.Find("Iron3").GetComponent<RectTransform>();			

		for(int i = 3; i>=numberOfPlayers;i--){
			GameObject.Find("P" + i).SetActive(false);
		}

		switch(numberOfPlayers){
		case 2: {C0.rect = new Rect(0,0,0.5f,1); C1.rect = new Rect(0.5f,0,0.5f,1);break;}
		case 3: {C0.rect = new Rect(0,0,1f/3f,1);C1.rect = new Rect(1f/3f,0,1f/3f,1);C2.rect = new Rect(2f/3f,0,1f/3f,1);break;}
		case 4: {C0.rect = new Rect(0,0,0.5f,0.5f);C1.rect = new Rect(0.5f,0,0.5f,0.5f);C2.rect = new Rect(0,0.5f,0.5f,0.5f);C3.rect = new Rect(0.5f,0.5f,0.5f,0.5f);break;}
		}
		//TODO totally fucked up
		switch(numberOfPlayers){
		case 2: {Hp0.anchorMax = new Vector2(0f,0f); Hp0.anchorMin = new Vector2(0f,0f); Hp0.anchoredPosition = new Vector2(15,15);
				IronOre0.anchorMax = new Vector2(0f,0f); IronOre0.anchorMin = new Vector2(0f,0f); IronOre0.anchoredPosition = new Vector2(15,45);
				Iron0.anchorMax = new Vector2(0f,0f); Iron0.anchorMin = new Vector2(0f,0f); Iron0.anchoredPosition = new Vector2(45,15);

				Hp1.anchorMax = new Vector2(1f,0f); Hp1.anchorMin = new Vector2(1f,0f); Hp1.anchoredPosition = new Vector2(-15,15);
				IronOre1.anchorMax = new Vector2(1f,0f); IronOre1.anchorMin = new Vector2(1f,0f); IronOre1.anchoredPosition = new Vector2(-15,45);
				Iron1.anchorMax = new Vector2(1f,0f); Iron1.anchorMin = new Vector2(1f,0f); Iron1.anchoredPosition = new Vector2(-45,15);
				break;}
		case 3: {Hp0.anchorMax = new Vector2(0f,0f); Hp0.anchorMin = new Vector2(0f,0f); Hp0.anchoredPosition = new Vector2(15,15);
				IronOre0.anchorMax = new Vector2(0f,0f); IronOre0.anchorMin = new Vector2(0f,0f); IronOre0.anchoredPosition = new Vector2(15,45);
				Iron0.anchorMax = new Vector2(0f,0f); Iron0.anchorMin = new Vector2(0f,0f); Iron0.anchoredPosition = new Vector2(45,15);

				Hp1.anchorMax = new Vector2(0.5f,0f); Hp1.anchorMin = new Vector2(0.5f,0f); Hp1.anchoredPosition = new Vector2(0,15);
				IronOre1.anchorMax = new Vector2(0.5f,0f); IronOre1.anchorMin = new Vector2(0.5f,0f); IronOre1.anchoredPosition = new Vector2(-45,15);
				Iron1.anchorMax = new Vector2(0.5f,0f); Iron1.anchorMin = new Vector2(0.5f,0f); Iron1.anchoredPosition = new Vector2(45,15);

				Hp2.anchorMax = new Vector2(1f,0f); Hp2.anchorMin = new Vector2(1f,0f); Hp2.anchoredPosition = new Vector2(-15,15);
				IronOre2.anchorMax = new Vector2(1f,0f); IronOre2.anchorMin = new Vector2(1f,0f); IronOre2.anchoredPosition = new Vector2(-15,45);
				Iron2.anchorMax = new Vector2(1f,0f); Iron2.anchorMin = new Vector2(1f,0f); Iron2.anchoredPosition = new Vector2(-45,15);
				break;}
		case 4: {Hp0.anchorMax = new Vector2(0f,0f); Hp0.anchorMin = new Vector2(0f,0f); Hp0.anchoredPosition = new Vector2(15,15);
				IronOre0.anchorMax = new Vector2(0f,0f); IronOre0.anchorMin = new Vector2(0f,0f); IronOre0.anchoredPosition = new Vector2(15,45);
				Iron0.anchorMax = new Vector2(0f,0f); Iron0.anchorMin = new Vector2(0f,0f); Iron0.anchoredPosition = new Vector2(45,15);

				Hp1.anchorMax = new Vector2(1f,0f); Hp1.anchorMin = new Vector2(1f,0f); Hp1.anchoredPosition = new Vector2(-15,15);
				IronOre1.anchorMax = new Vector2(1f,0f); IronOre1.anchorMin = new Vector2(1f,0f); IronOre1.anchoredPosition = new Vector2(-15,45);
				Iron1.anchorMax = new Vector2(1f,0f); Iron1.anchorMin = new Vector2(1f,0f); Iron1.anchoredPosition = new Vector2(-45,15);

				Hp2.anchorMax = new Vector2(0f,1f); Hp2.anchorMin = new Vector2(0f,1f); Hp2.anchoredPosition = new Vector2(15,-15);
				IronOre2.anchorMax = new Vector2(0f,1f); IronOre2.anchorMin = new Vector2(0f,1f); IronOre2.anchoredPosition = new Vector2(15,-45);
				Iron2.anchorMax = new Vector2(0f,1f); Iron2.anchorMin = new Vector2(0f,1f); Iron2.anchoredPosition = new Vector2(45,-15);

				Hp3.anchorMax = new Vector2(1f,1f); Hp3.anchorMin = new Vector2(1f,1f); Hp3.anchoredPosition = new Vector2(-15,-15);
				IronOre3.anchorMax = new Vector2(1f,1f); IronOre3.anchorMin = new Vector2(1f,1f); IronOre3.anchoredPosition = new Vector2(-15,-45);
				Iron3.anchorMax = new Vector2(1f,1f); Iron3.anchorMin = new Vector2(1f,1f); Iron3.anchoredPosition = new Vector2(-45,-15);
				break;}
		}
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

	void DistributePlayersIntoTeams(){
		for(int i = 0;i<numberOfPlayers;i++){
			playerModel = GameObject.FindWithTag("Player" + i);
			playerModel.GetComponent<PlayerInventory>().teamNumber = i%numberOfTeams;
			if(numberOfPlayers>numberOfTeams){
				int variance = 80;
				int defColorBase = 255;
				switch(playerModel.GetComponent<PlayerInventory>().teamNumber){
				case 0: {playerModel.GetComponent<Renderer>().material.color = new Color32(plusMinus(variance,0),plusMinus(variance,defColorBase),plusMinus(variance,defColorBase),255);break;}
				case 1: {playerModel.GetComponent<Renderer>().material.color = new Color32(plusMinus(variance,defColorBase),plusMinus(variance,0),plusMinus(variance,defColorBase),255);break;}
				case 2: {playerModel.GetComponent<Renderer>().material.color = new Color32(plusMinus(variance,defColorBase),plusMinus(variance,defColorBase),plusMinus(variance,0),255);break;}
				case 3: {playerModel.GetComponent<Renderer>().material.color = new Color32(plusMinus(variance,0),plusMinus(variance,defColorBase),plusMinus(variance,0),255);break;}
				}
			}
		}
	}

	byte plusMinus(int ammount, int defaultColor){
		int temp = 0;
		if(defaultColor == 255){
			temp = pseudoRandom.Next(-ammount,-32);
		}
		if(defaultColor == 0){
			temp = pseudoRandom.Next(32,ammount);
		}
		return (byte)(temp + defaultColor);
	}


	IEnumerator DelayedMethods(){ //make this shit work
		yield return new WaitForSeconds(0.01f);
		GenerateMap(ironOreMap,ironOreFillPercent,2);
		FillMap();
		for(int i = 0;i<numberOfTeams;i++){
			MakeBase(i);
			for(int y = 0;y<numberOfPlayers;y++){
				playerModel = GameObject.FindWithTag("Player" + y);
				if(playerModel.GetComponent<PlayerInventory>().teamNumber == i){
					positionPlayer();
				}
			}
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
	 	posX = pseudoRandom.Next(4,width-4); // 4 aby to neslo mimo mapu a aby bolo okolo zakladne aspon 1 policko
		posY = pseudoRandom.Next(4,height-4);
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
		/*
		playerModel.transform.position = new Vector3(posX+1, 0.5f, posY-1);
		playerModel.GetComponent<PlayerInventory>().setStartingPosition(posX,posY);
		playerList.Add(playerModel);
		*/


		cubeList.Find(GameObject => GameObject.transform.position == new Vector3(posX,cubeHeight/2,posY+2)).SetActive(false); //make entrances
		cubeList.Find(GameObject => GameObject.transform.position == new Vector3(posX,cubeHeight/2,posY-2)).SetActive(false);

		GameObject smelter = cubeList.Find(GameObject => GameObject.transform.position == new Vector3(posX+1,cubeHeight/2,posY+1)); //smelter
		//smelter.transform.position = new Vector3(posX+1,0.1f,posY+1);
		smelter.GetComponent<TileProperties>().SetSmelter(teamNumber);
		GameObject emperium = cubeList.Find(GameObject => GameObject.transform.position == new Vector3(posX,cubeHeight/2,posY)); //emperium
		emperium.GetComponent<TileProperties>().SetEmperium(teamNumber);

	}

	void positionPlayer(){
		//playerModel = GameObject.FindWithTag("Player" + i);
		switch(playerModel.GetComponent<PlayerInventory>().getPlayerNumber()%numberOfPlayers/numberOfTeams){
		case 0: playerModel.transform.position = new Vector3(posX+1, 0.5f, posY-1); break;
		case 1: playerModel.transform.position = new Vector3(posX-1, 0.5f, posY-1); break;
		case 2: playerModel.transform.position = new Vector3(posX+1, 0.5f, posY); break;
		case 3: playerModel.transform.position = new Vector3(posX-1, 0.5f, posY); break;
		}
		playerModel.GetComponent<PlayerInventory>().setStartingPosition(posX,posY);
		playerList.Add(playerModel);
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
