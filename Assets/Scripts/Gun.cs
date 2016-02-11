using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public float dpm; //dig per minute
	public float rpm;
	public int projectileSpeed;

	public Transform spawn;
	public Transform digSpawn;
	public MapGenerator mapGenerator;
	public Material ironBroken;
	public Rigidbody projectile;
	public PlayerInventory inventory;
	public Light projectileGlow;

	private float secondsBetweenDig;
	private float secondsBetweenShots;
	private float nextPossibleDigTime;
	private float nextPossibleShotTime;
	private int magazine;
	// Use this for initialization
	void Start () {
		secondsBetweenDig = 60/dpm;
		secondsBetweenShots = 60/rpm;
		magazine = 3;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Dig(){
		if(CanDig()){
			Ray ray = new Ray(digSpawn.position,digSpawn.forward);
			RaycastHit hit;
			float shotDistance = 0.2f;
			if(Physics.Raycast(ray, out hit, shotDistance)){
				shotDistance = hit.distance;
				GameObject cube = mapGenerator.cubeList.Find(GameObject => GameObject.name == hit.collider.name);
				if(cube.tag == "TileIronOre"){
					if(inventory.ironOre >= inventory.carryLimit){
						return;
					}
					inventory.changeIronOre(1);
				}
				cube.GetComponent<TileProperties>().durability -= 1;
				if(cube.GetComponent<TileProperties>().durability == 5){
					cube.GetComponent<Renderer>().material = ironBroken;
				}
				if(cube.GetComponent<TileProperties>().durability <= 0){
					cube.SetActive(false);
				}
				/*
				Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red,0.2f);
				*/
				nextPossibleDigTime = Time.time + secondsBetweenDig;
			}
		} 
	}

	public void Shoot(){
		if(CanShoot()){
			magazine--;
			Rigidbody newProjectile = Instantiate(projectile, spawn.position, spawn.rotation) as Rigidbody;
			switch(inventory.getPlayerNumber()){
			case 0: {newProjectile.GetComponent<Renderer>().material.color = new Color(0f,1f,1f,1f); projectileGlow.color =  new Color(0f,1f,1f); break;}
			case 1: {newProjectile.GetComponent<Renderer>().material.color = new Color(1f,0f,1f,1f); projectileGlow.color =  new Color(1f,0f,1f); break;}
			case 2: {newProjectile.GetComponent<Renderer>().material.color = new Color(1f,1f,0f,1f); projectileGlow.color =  new Color(1f,1f,0f); break;}
			case 3: {newProjectile.GetComponent<Renderer>().material.color = new Color(0f,1f,0f,1f); projectileGlow.color =  new Color(0f,1f,0f); break;}
			}
			newProjectile.AddForce(spawn.forward * projectileSpeed);
			if(magazine<=0){
				magazine = 3;
				nextPossibleShotTime = Time.time + secondsBetweenShots;
			}
		}

	}

	private bool CanShoot(){
		bool canShoot = true;
		if(Time.time<nextPossibleShotTime){
			canShoot = false;
		}
		return canShoot;
	}

	private bool CanDig(){
		bool canDig = true;
		if(Time.time<nextPossibleDigTime){
			canDig = false;
		}
		return canDig;
	}
}
