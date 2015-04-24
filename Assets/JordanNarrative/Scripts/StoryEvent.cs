using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StoryEvent : MonoBehaviour {

	//tooltip
	public GameObject tooltipPrefab;
	private GameObject toolTip;

	//prefab for edit panel
	public GameObject editPanel;
	//prefab for event trigger
	public GameObject triggerPrefab;

	//name of the event
	public string eventName;

	//details of the event
	public string eventDetails;

	//set whether the event is expanded to view subevents
	public bool expanded;

	//set whether the event is visible
	public bool hidden;

	//events nested under the event
	public List<StoryEvent> subEvents;

	//event trigger object
	public GameObject et;

	//set whether we're currently moving the event trigger object
	private bool movingTrigger;

	private GameObject eventsFolder;

	// Use this for initialization
	void Start () {
		eventName = "New Event";
		eventDetails = "Enter event details";
		expanded = false;
		hidden = false;
		movingTrigger = false;
		eventsFolder = GameObject.Find ("Events");
	}
	
	// Update is called once per frame
	void Update () {
		//OH GOD HELP I DON'T KNOW WHAT'S EVEN HAPPENING

		//assetSelected
		if (movingTrigger == true)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
			if (hits.Length > 0 && hits[0].collider != null)
			{
				et.transform.position = hits[0].collider.gameObject.transform.position;
				
			}
			
			//we're stopping dragging
			if (Input.GetMouseButtonDown(0)	)
				//(ObjectGenerator.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(location, 1 << LayerMask.NameToLayer("BunnyGenerator")))
			{
				//check if we can leave the bunny here
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);				
				hits = Physics2D.RaycastAll(ray.origin, ray.direction);
				
				//Debug.Log (hits.Where(x => x.collider.gameObject.tag == "Event").Count());
				//in order to place it, we must have a background and no other bunnies
				if (hits.Where(x => x.collider.gameObject.tag == "Background").Count() > 0
				    && hits.Where(x => x.collider.gameObject.tag == "Event").Count() == 1)
					
				{
					//we can leave a bunny here, so decrease money and activate it
					et.transform.position = 
						hits.Where(x => x.collider.gameObject.tag == "Background")
							.First().collider.gameObject.transform.position;

					//Slide all Panels
					GameObject[] gos;
					gos = GameObject.FindGameObjectsWithTag("Panel");
					foreach (GameObject obj in gos){
						obj.GetComponent<SlidePanel>().Slide();
					}

				}
				else
				{
					//we can't leave a bunny here, so destroy the temp one
					Destroy(et);
				}
				et.GetComponent<StoryEventTrigger>().SetEvent (this);
				movingTrigger = false;
			}
		}
	}

	public void CreateEventTrigger() {
		et = (GameObject)Instantiate (triggerPrefab, Input.mousePosition, Quaternion.identity);
		movingTrigger = true;
		et.transform.parent = eventsFolder.transform;

		//Slide all Panels
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Panel");
		foreach (GameObject obj in gos){
			obj.GetComponent<SlidePanel>().Slide ();
		}
	}

	public void ExpandOrCollapse() {
		if (expanded == false) {
			expanded = true;
			this.gameObject.transform.GetChild (2).transform.Rotate (0f, 0f, -90f);
			//show the subevents
			for(int i = 0; i < subEvents.Count; i++){
				subEvents[i].hidden = false;
			}
		} else {
			expanded = false;
			this.gameObject.transform.GetChild (2).transform.Rotate (0f, 0f, 90f);
			//hide the subevents
			for(int i = 0; i < subEvents.Count; i++){
				subEvents[i].Hide ();
				subEvents[i].hidden = true;
			}
		}
		//display the narrative again
		GameObject.Find ("NarrativePanel").GetComponent<NarrativeManager> ().DisplayNarrative ();
	}

	//hacky way of hiding events, should make this better if time permits
	void Hide(){
		this.gameObject.transform.position = new Vector2 (-1000f, -1000f);
	}

	//Bring up the panel that allows the event to be edited
	public void EditEvent() {
		GameObject ep = (GameObject) Instantiate (editPanel, new Vector2(0f,0f), Quaternion.identity);
		ep.transform.SetParent (GameObject.Find ("Narrative Canvas").transform, false);
		ep.GetComponent<RectTransform> ().offsetMax = new Vector2 (0f, 0f);
		ep.GetComponent<RectTransform> ().offsetMin = new Vector2 (0f, 0f);
		ep.transform.GetChild (0).gameObject.GetComponent<EditEvent> ().SetEvent (this);
	}

	public void Refresh() {
		this.transform.Find ("EventText").GetComponent<Text> ().text = eventName;
	}

	public void CreateTooltip() {
		toolTip = (GameObject)Instantiate (tooltipPrefab, new Vector2(Input.mousePosition.x + 150, Input.mousePosition.y - 100), Quaternion.identity);
		toolTip.transform.SetParent(GameObject.Find ("Narrative Canvas").transform);
		toolTip.transform.Find ("PanelManager").GetComponent<TooltipPanel> ().SetEvent (this);
	}
	
	public void DestroyTooltip() {
		if (toolTip != null) {
			GameObject.Destroy (toolTip);
		}
	}


}
