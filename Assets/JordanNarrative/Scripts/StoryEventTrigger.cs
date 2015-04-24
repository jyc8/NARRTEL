using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic; 

public class StoryEventTrigger : MonoBehaviour {

	//the event that the trigger belongs to
	public StoryEvent evt;
	private GameObject lineManager;
	public bool placed = false;
	private bool drawing = false;

	//the prefab for the tooltip
	public GameObject tooltipPrefab;

	//the tooltip object
	private GameObject toolTip;

	public void SetEvent(StoryEvent e) {
		evt = e;
		lineManager = GameObject.Find ("LineManager");
		placed = true;
	}

	private void OnMouseEnter(){
		//Debug.Log ("tooltip on");
		if (evt != null) {
			toolTip = (GameObject)Instantiate (tooltipPrefab, new Vector2(Input.mousePosition.x + 150, Input.mousePosition.y - 100), Quaternion.identity);
			toolTip.transform.SetParent(GameObject.Find ("Narrative Canvas").transform);
			toolTip.transform.Find ("PanelManager").GetComponent<TooltipPanel> ().SetEvent (evt);
		}
	}

	private void OnMouseExit(){
		if (toolTip != null) {
			GameObject.Destroy (toolTip);
		}
	}

	private void OnMouseDown(){
		if (placed){
			drawing = true;
			Debug.Log ("Clicked " + drawing);
		}
	}
}
