using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EditEvent : MonoBehaviour {

	private StoryEvent evt;

	private int art = 0;

	//set the story event that this panel is editing
	public void SetEvent(StoryEvent e) {
		evt = e;
		this.transform.parent.Find ("Title").gameObject.GetComponent<InputField> ().text = evt.eventName;
		this.transform.parent.Find ("Details").gameObject.GetComponent<InputField> ().text = evt.eventDetails;
	}

	//add concept art
	//totally fake right now, doesn't save and just adds a random image
	public void addArt() {

		//roll a random integer for concept art
		int rng = Random.Range (1, 22);

		if (art == 0) {
			this.transform.parent.Find ("Image").gameObject.GetComponent<Image>().color = Color.white;
			this.transform.parent.Find ("Image").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite> ("conceptart/" + rng.ToString ());
		} else if (art == 1) {
			this.transform.parent.Find ("Image 1").gameObject.GetComponent<Image>().color = Color.white;
			this.transform.parent.Find ("Image 1").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite> ("conceptart/" + rng.ToString ());
		} else if (art == 2) {
			this.transform.parent.Find ("Image 2").gameObject.GetComponent<Image>().color = Color.white;
			this.transform.parent.Find ("Image 2").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite> ("conceptart/" + rng.ToString ());
		}

		art++;
		if (art > 2) {
			art = 0;
		}
	}

	//save changes to the event
	public void SaveEvent() {
		evt.eventName = this.transform.parent.Find("Title").gameObject.GetComponent<InputField> ().text;
		evt.eventDetails = this.transform.parent.Find("Details").gameObject.GetComponent<InputField> ().text;
		evt.Refresh ();
		ClosePanel ();
	}

	public void ClosePanel() {
		GameObject.Destroy(this.gameObject.transform.parent.gameObject);
	}
}
