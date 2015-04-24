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
		map.SetActive(true);
	}

	public void applyState(){

		foreach (Transform childTransform in map.transform){
			Destroy (childTransform.gameObject);
		}

		//while(map.transform.childCount > 0){
		//	Destroy (map.transform.GetChild(0));
		//}

		while(thisObj.transform.GetChild(4).transform.childCount > 0){
			thisObj.transform.GetChild(4).transform.GetChild(0).parent = map.transform;
		}
		map.SetActive(true);
		Destroy (thisObj);

	}
	
}