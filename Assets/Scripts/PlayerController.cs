using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

	private CharacterController pController;
	private Camera cam;
	private Quaternion targetRotation;
	private float rotationSpeed = 360;
	private float walkSpeed = 4;
	private float runSpeed = 6;
	private float acceleration = 5;
	private Vector3 currentVelocityModifier;
	public int playerNumber;
	private PlayerInventory playerInventory;
	public Gun gun;
	public bool movementEnabled = true;
	public bool buildingMenu = false;


	void Start () {
		pController = GetComponent<CharacterController>();
		cam = Camera.main;
		playerInventory = gameObject.GetComponent<PlayerInventory>();
	}

	void Update () {
		if(movementEnabled){
			ControlWASD();
			//ControlMouse();
			if(Input.GetButtonDown("Dig"+playerNumber)){
				gun.Dig();
			}

			if(Input.GetButtonDown("Shoot"+playerNumber)){
				gun.Shoot();
			}

			if(Input.GetButtonDown("Build"+playerNumber)){
				buildingMenu = true;
				playerInventory.BuildingMenuOn();
			}
		}

		if(!movementEnabled){
			int horiz = (int)Input.GetAxisRaw("Horizontal"+playerNumber);
			int verti = (int)Input.GetAxisRaw("Vertical"+playerNumber);
			if(horiz == 0 && verti == 0){
				playerInventory.sideChoice(0);
			}else{
				if(horiz == -1 && verti == 0){playerInventory.sideChoice(-1);}
				if(horiz == 1 && verti == 0){playerInventory.sideChoice(1);}
				if(verti == -1 && horiz == 0){playerInventory.sideChoice(-2);}
				if(verti == 1 && horiz == 0){playerInventory.sideChoice(2);}
			}
		}

		if(Input.GetButtonUp("Build"+playerNumber)){
			playerInventory.BuildingMenuOff();
			buildingMenu = false;
		}
	}

	void ControlMouse(){
		Vector3 mousePos = Input.mousePosition;
		mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));
		targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x, 0, transform.position.z));
		transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"+playerNumber), 0, Input.GetAxisRaw("Vertical"+playerNumber));
		currentVelocityModifier = Vector3.MoveTowards(currentVelocityModifier, input ,acceleration*Time.deltaTime);
		Vector3 motion = currentVelocityModifier;
		motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1)? 0.7f : 1.0f;
		motion *= (Input.GetButton("Run"+playerNumber))?runSpeed:walkSpeed;
		motion += Vector3.up * -8;
		pController.Move(motion * Time.deltaTime);

	}

	void ControlWASD(){
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"+playerNumber),0,Input.GetAxisRaw("Vertical"+playerNumber));
		if(input != Vector3.zero){
			targetRotation = Quaternion.LookRotation(input);
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,targetRotation.eulerAngles.y,rotationSpeed * Time.deltaTime);
		}
		currentVelocityModifier = Vector3.MoveTowards(currentVelocityModifier,input,acceleration*Time.deltaTime);
		Vector3 motion = currentVelocityModifier;
		motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1)? .7f : 1;
		motion *= (Input.GetButton("Run"+playerNumber))?runSpeed:walkSpeed;
		motion += Vector3.up * -8;
		pController.Move(motion * Time.deltaTime);
	}
}
