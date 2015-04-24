using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NarrativeManager : MonoBehaviour {

	//prefabs
	public GameObject newButton;
	public GameObject storyEvent;

	//new event buttopn
	private List<GameObject> nb = new List<GameObject>();

	private List<StoryEvent> narrativeList = new List<StoryEvent> ();

	// Use this for initialization
	void Start () {


		CreateStoryEvent ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DisplayNarrative() {

		//destroy all the new event buttons to prevent doubling
		for (int i = 0; i < nb.Count; i++) {
			GameObject.Destroy (nb[i]);
		}

		//number of entries displayed on the list
		int numEntries = 0;

		for (int i = 0; i < narrativeList.Count; i++) {
			//place it at the right height/width
			narrativeList[i].gameObject.GetComponent<RectTransform> ().localPosition = new Vector2 (0f, 410f - 60f * numEntries);
			numEntries++;

			//check if the event is expanded and display subevents
			if(narrativeList[i].expanded == true) {
				//increment numEntries by the number of subevents displayed
				numEntries += DisplaySubEvents (narrativeList[i]);
			}
		}
		//create the new event button
		nb.Add((GameObject)Instantiate (newButton, new Vector2 (0f, 0f), Quaternion.identity));
		//set the narrative panel as its parent
		nb[nb.Count - 1].GetComponent<RectTransform>().SetParent(this.gameObject.transform, false);
		//place new button at the right height/width
		nb[nb.Count - 1].GetComponent<RectTransform> ().localPosition = new Vector2 (0f, 410f - 60f * numEntries);
	}

	int DisplaySubEvents(StoryEvent e) {

		int numEntries = 0;

		for (int i = 0; i < e.subEvents.Count; i++) {
			//if event is visible
			if(e.subEvents[i].hidden == false) {
				//place it at the right height/width
				numEntries++;
				e.subEvents[i].gameObject.transform.localPosition = new Vector2 (20f, - 60f * numEntries);
			}
			//check if the subevent is expanded and display its subevents
			if(e.subEvents[i].expanded == true) {
				//increment numEntries by the number of subevents displayed
				numEntries += DisplaySubEvents (e.subEvents[i]);
			}
		}
		numEntries++;
		//create a new event button and place it under the subevents
		//create the new event button
		nb.Add((GameObject)Instantiate (newButton, new Vector2 (0f, 0f), Quaternion.identity));
		//set the event as its parent
		nb[nb.Count - 1].GetComponent<RectTransform>().SetParent(e.gameObject.transform, false);
		//set the correct offsets
		nb[nb.Count - 1].GetComponent<RectTransform> ().offsetMax = new Vector2 (0f, 0f);
		nb[nb.Count - 1].GetComponent<RectTransform> ().offsetMin = new Vector2 (40f, 0f);
		//move the whole thing down from the event as far as int needs to be
		nb [nb.Count - 1].transform.localPosition = new Vector2 (20f, -60f * numEntries);
		return numEntries;
	}

	//create story event as a child of the narrative panel (top level event)
	public void CreateStoryEvent() {

		//put the new story event at the bottom
		GameObject se = (GameObject)Instantiate (storyEvent, new Vector2 (0f, 0f), Quaternion.identity);
		//set the narrative panel as its parent
		se.GetComponent<RectTransform>().SetParent(this.gameObject.transform, false);
		//add the event to the narrative list
		narrativeList.Add (se.GetComponent<StoryEvent>());
		DisplayNarrative ();
	}

	//create story event as a subevent of another
	public void CreateStoryEvent(StoryEvent e) {
		
		//put the new story event at the bottom
		GameObject se = (GameObject)Instantiate (storyEvent, new Vector2 (0f, 0f), Quaternion.identity);
		//set the narrative panel as its parent
		se.GetComponent<RectTransform>().SetParent(e.gameObject.transform, false);
		//set the correct offsets/anchors
		se.GetComponent<RectTransform> ().offsetMax = new Vector2 (0f, 0f);
		se.GetComponent<RectTransform> ().offsetMin = new Vector2 (40f, 0f);
		//add the event to the narrative list
		e.subEvents.Add (se.GetComponent<StoryEvent>());
		DisplayNarrative ();
	}
}
