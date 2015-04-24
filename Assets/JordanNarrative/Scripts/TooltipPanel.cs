using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TooltipPanel : MonoBehaviour {

	StoryEvent evt;

	public void SetEvent(StoryEvent e) {
		evt = e;
		this.transform.parent.Find ("Title").gameObject.GetComponent<Text> ().text = evt.eventName;
		this.transform.parent.Find ("Details").gameObject.GetComponent<Text> ().text = evt.eventDetails;

		//roll a random integer for concept art
		int rng = Random.Range (1, 22);

		this.transform.parent.Find ("Image").gameObject.GetComponent<Image> ().color = Color.white;
		this.transform.parent.Find ("Image").gameObject.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("conceptart/" + rng.ToString ());
	}
}
