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
}
