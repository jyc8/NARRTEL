using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class slideOutPanel : MonoBehaviour {

	private GameObject slidePanelObject;
	private GameObject suggestionIcon;
	public Sprite suggestionsAlertSprite;

	private Image suggestionsAlertImage;

	//animator reference
	private Animator anim;

	//variable for checking if the menu panel is open 
	private bool isOpen = true;
	private int lastTab = 0;

	// Use this for initialization
	void Start () {
		slidePanelObject = GameObject.Find ("slideOutPanel");
		suggestionIcon = GameObject.Find ("suggestionIcon");
		suggestionsAlertSprite = Resources.Load <Sprite> ("suggestionsAlert");

		anim = slidePanelObject.GetComponent<Animator>();

		//disable it on start to stop it from playing the default animation
		anim.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void tabClicked(int tabIndex) {
//		Vector3 temp = new Vector3(7.0f,0,0);
//		slidePanelObject.transform.position = temp;

		if (isOpen == false) {
			
			//enable the animator component
			anim.enabled = true;
			//play the Slidein animation
			anim.Play ("slideInRight");
			
			isOpen = true;
		}

		if (isOpen == true) {

			if (lastTab == tabIndex) {
			//enable the animator component
			anim.enabled = true;
			//play the Slidein animation
			anim.Play ("slideOutRight");

			isOpen = false;
			
			}
		}
		lastTab = tabIndex;
	}

	public void objectButtonClicked(int buttonNumber) {
		Debug.Log (buttonNumber);
	}

	public void suggestionAlert() {
		suggestionIcon.GetComponent<Image> ().sprite = suggestionsAlertSprite;
	}
}
