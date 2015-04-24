using UnityEngine;
using System.Collections;

public class SaveStateManager : MonoBehaviour {
	
	public GameObject thisObj;
	public GameObject map;
	
	public void showState(){
		//Debug.Log ("entered");
		GameObject state = this.transform.GetChild(4).gameObject;
		state.SetActive (true);
		map.SetActive(false);
	}
	
	public void hideState(){
		//Debug.Log ("exited");
		GameObject state = this.transform.GetChild(4).gameObject;
		state.SetActive (false);
		map.SetActive(true);
	}
	
	public void delete(){
		Destroy (thisObj);
	}
	
}