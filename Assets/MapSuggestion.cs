using UnityEngine;
using System.Collections;

public class MapSuggestion : MonoBehaviour {
	
	public GameObject thisObj;

	public void showSuggestion(){
		//Debug.Log ("entered");
		GameObject selection = this.transform.GetChild(4).gameObject;
		selection.SetActive (true);
	}

	public void hideSuggestion(){
		//Debug.Log ("exited");
		GameObject selection = this.transform.GetChild(4).gameObject;
		selection.SetActive (false);
	}

	public void delete(){
		Destroy (thisObj);
	}

}
