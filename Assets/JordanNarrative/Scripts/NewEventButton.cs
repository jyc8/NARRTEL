using UnityEngine;
using System.Collections;

public class NewEventButton : MonoBehaviour {

	//extremely messy way of making the onclick work in the prefab
	public void CallStoryEvent() {
		if (this.gameObject.transform.parent.gameObject.name.Equals ("NarrativePanel")) {
			this.gameObject.transform.parent.gameObject.GetComponent<NarrativeManager> ().CreateStoryEvent ();
		} else {
			GameObject.Find ("NarrativePanel").GetComponent<NarrativeManager>().CreateStoryEvent (this.transform.parent.GetComponent<StoryEvent>());
		}
	}

	public void mouseEntered(){
		Debug.Log ("Entered New");

		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Tooltip");

		foreach (GameObject obj in gos){
			Destroy(obj);
		}

		//while (gos.Length > 0){
		//	Destroy(gos[0]);
		//}
	}
}
