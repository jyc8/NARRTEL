using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class StoryEventTrigger : MonoBehaviour {

	//the event that the trigger belongs to
	public StoryEvent evt;

	//the prefab for the tooltip
	public GameObject tooltipPrefab;

	//the tooltip object
	private GameObject toolTip;

	public void SetEvent(StoryEvent e) {
		evt = e;
	}

	public void CreateTooltip() {
		if (evt != null) {
			toolTip = (GameObject)Instantiate (tooltipPrefab, new Vector2(Input.mousePosition.x + 150, Input.mousePosition.y - 100), Quaternion.identity);
			toolTip.transform.SetParent(GameObject.Find ("Canvas").transform);
			toolTip.transform.Find ("PanelManager").GetComponent<TooltipPanel> ().SetEvent (evt);
		}
	}

	public void DestroyTooltip() {
		if (toolTip != null) {
			GameObject.Destroy (toolTip);
		}
	}
}
