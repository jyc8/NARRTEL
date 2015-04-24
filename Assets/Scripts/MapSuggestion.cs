using UnityEngine;
using System.Collections;

public class MapSuggestion : MonoBehaviour {
	
	public GameObject thisObj;
	public GameObject map;

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

	public void applySuggestion(){
		while (thisObj.transform.GetChild(4).transform.childCount > 0) {
			thisObj.transform.GetChild(4).transform.GetChild(0).parent = map.transform;
		}
		Destroy (thisObj);
	}
}
