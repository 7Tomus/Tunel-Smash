using UnityEngine;
using System.Collections;

public class DayNightCyle : MonoBehaviour {
	private float time = 12;

	// Use this for initialization
	void Start () {
		StartCoroutine(timeProgress());
	}
	
	// Update is called once per frame
	void Update () {
		updateLight();
	}

	IEnumerator timeProgress(){
		while(true){
			yield return new WaitForSeconds(20);
			time ++;
			if(time>24){
				time = 1;
			}
		}
	}

	private void updateLight(){
		if(time == 21){
			gameObject.GetComponent<Light>().intensity = 0f;
		}
		if(Between(time,4,8)){
			gameObject.GetComponent<Light>().intensity = (time-3f)/(6f);
		}
		if(time == 9){
			gameObject.GetComponent<Light>().intensity = 1f;
		}
		if(Between(time,16,20)){
			gameObject.GetComponent<Light>().intensity = 1f-((time-15f)/(6f));
		}
	}

	bool Between(int num, int lower, int upper){
		return lower <= num && num <= upper;
	}

	bool Between(float num, float lower, float upper){
		return lower <= num && num <= upper;
	}
}
