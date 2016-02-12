using UnityEngine;
using System.Collections;

public class TileProperties : MonoBehaviour {

	public int durability;
	public Material material_dirt, material_rock, material_ironOre, material_iron;

	void Start () {
	}

	void Update () {
	}

	public void SetDirt(int position){
		gameObject.name = "Dirt"+position;
		gameObject.tag = "TileDirt";
		gameObject.GetComponent<Renderer>().material = material_dirt;
		durability = 1;
	}

	public void SetRock(int position){
		gameObject.name = "Rock"+position;
		gameObject.tag = "TileRock";
		gameObject.GetComponent<Renderer>().material = material_rock;
		durability = 3;
	}

	public void SetIronOre(int position){
		gameObject.name = "IronOre"+position;
		gameObject.tag = "TileIronOre";
		gameObject.GetComponent<Renderer>().material = material_ironOre;
		durability = 5;
	}

	public void SetIron(int position){
		gameObject.name = "Iron"+position;
		gameObject.tag = "TileIron";
		gameObject.GetComponent<Renderer>().material = material_iron;
		durability = 20;
	}

	public void SetIron(){
		gameObject.tag = "TileIron";
		gameObject.GetComponent<Renderer>().material = material_iron;
		durability = 20;
	}


	public void SetSmelter(int teamNumber){
		gameObject.name = "Smelter" + teamNumber;
		gameObject.tag = "Smelter";
		gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
		gameObject.transform.localScale = new Vector3(1,0.2f,1);
		switch(teamNumber){
		case 0: {gameObject.GetComponent<Renderer>().material.color = new Color(0f,1f,1f,0.1f);break;}
		case 1: {gameObject.GetComponent<Renderer>().material.color = new Color(1f,0,1f,0.1f);break;}
		case 2: {gameObject.GetComponent<Renderer>().material.color = new Color(1f,1f,0f,0.1f);break;}
		case 3: {gameObject.GetComponent<Renderer>().material.color = new Color(0f,1f,0f,0.1f);break;}
		}
		gameObject.AddComponent<SmelterController>();
		BoxCollider[] boxColliders = gameObject.GetComponents<BoxCollider>();
		foreach(BoxCollider thisBoxCollider in boxColliders){
			if (thisBoxCollider.isTrigger == false){
				thisBoxCollider.enabled = false;
			}

		}
		durability = 20;
		gameObject.SetActive(true);
	}

	public void SetEmperium(int teamNumber){
		gameObject.name = "Emperium" + teamNumber;
		gameObject.tag = "Emperium";
		gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
		switch(teamNumber){ //material
		case 0: {gameObject.GetComponent<Renderer>().material.color = new Color(0f,1f,1f,0.1f);break;}
		case 1: {gameObject.GetComponent<Renderer>().material.color = new Color(1f,0,1f,0.1f);break;}
		case 2: {gameObject.GetComponent<Renderer>().material.color = new Color(1f,1f,0f,0.1f);break;}
		case 3: {gameObject.GetComponent<Renderer>().material.color = new Color(0f,1f,0f,0.1f);break;}
		}
		switch(teamNumber){ //light
		case 0: {gameObject.AddComponent<Light>().color =  new Color(0f,1f,1f); break;}
		case 1: {gameObject.AddComponent<Light>().color =  new Color(1f,0f,1f); break;}
		case 2: {gameObject.AddComponent<Light>().color =  new Color(1f,1f,0f); break;}
		case 3: {gameObject.AddComponent<Light>().color =  new Color(0f,1f,0f); break;}
		}
		BoxCollider[] boxColliders = gameObject.GetComponents<BoxCollider>();
		foreach(BoxCollider thisBoxCollider in boxColliders){
			if (thisBoxCollider.isTrigger == false){
				thisBoxCollider.enabled = false;
			}

		}
		gameObject.AddComponent<EmperiumController>();
		durability = 20;
		gameObject.SetActive(true);

	}

	public void SetBorder(int position){
		gameObject.name = "Border"+position;
		gameObject.tag = "Border";
		gameObject.GetComponent<Renderer>().material.color -= new Color(0.05f,0.05f,0.05f,0f);
		durability = 10000;
	}

}
