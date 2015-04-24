using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using Assets.Scripts;
using UnityEngine.EventSystems;

public class slideOutPanel : MonoBehaviour {
	private Camera mainCamera;
	private GameObject slidePanelObject;
	private GameObject suggestionIcon;
	public Sprite suggestionsAlertSprite;

	//animator reference
	private Animator anim;

	//variable for checking if the menu panel is open 
	private bool isOpen = true;
	private int lastTab = 0;

	//will be colored red if we cannot place a bunny there
	private GameObject tempBackgroundBehindPath;
	//type of bunnies we'll create
	public GameObject[] ObjectPrefab;
	public GameObject mapSuggestionsFolder;
	public GameObject pathSuggestionsFolder;
	// Draggable inspector reference to the Image GameObject's RectTransform.
	public GameObject selectionCanvas;
	public GameObject selectionBoxObject;
	public GameObject mapSuggestion;

	private GameObject newObject;
	private GameObject newSelection;
	private GameObject newSuggestion;

	// This variable will store the location of wherever we first click before dragging.
	private Vector2 initialClickPosition = Vector2.zero;
	

	private RectTransform selectionBox;

	//Buttons
	private bool assetSelected = false;
	private bool selectionSelected = false;
	private bool selectReady = false;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
		slidePanelObject = GameObject.Find ("slideOutPanel");
		suggestionIcon = GameObject.Find ("suggestionIcon");
		suggestionsAlertSprite = Resources.Load <Sprite> ("suggestionsAlert");

		anim = slidePanelObject.GetComponent<Animator>();

		//disable it on start to stop it from playing the default animation
		anim.enabled = false;
	}

	public void tabClicked(int tabIndex) {
//		Vector3 temp = new Vector3(7.0f,0,0);
//		slidePanelObject.transform.position = temp;

		if (isOpen == false) {
			
			openTabs();
		}

		if (isOpen == true) {

			if (lastTab == tabIndex) {
				closeTabs();
			}
		}
		lastTab = tabIndex;
	}



	public void objectButtonClicked(int buttonNumber) {
		//Debug.Log (buttonNumber);
		if (buttonNumber == 0){
			selectionSelected = true;

			//Destroy all active Selection Boxes
			foreach(GameObject o in GameObject.FindGameObjectsWithTag("Selection")) {
				Destroy(o);
			}

			newSelection = Instantiate(selectionBoxObject, Input.mousePosition, Quaternion.identity) as GameObject;
			newSelection.SetActive(true);
			newSelection.transform.parent = selectionCanvas.transform;
		}
		if (buttonNumber > 0){
			//Set grid highlight colour to default
			ResetTempBackgroundColor();
			//Set location to mouse location
			Vector2 location = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			//Set asset is selected
			assetSelected = true;
			newObject = Instantiate(ObjectPrefab[buttonNumber], Input.mousePosition, Quaternion.identity) as GameObject;
		}
		//Close Tabs
		closeTabs();
	}

	public GameObject Map;

	public void saveChanges(){
		while (newSelection.transform.childCount > 0) {
			newSelection.transform.GetChild(0).parent = Map.transform;
		}
		Destroy (newSelection);
	}

	public void createSuggestion(){
		suggestionIcon.GetComponent<Image>().sprite = suggestionsAlertSprite;

		//create newSelection box
		newSuggestion = Instantiate(mapSuggestion, Input.mousePosition, Quaternion.identity) as GameObject;
		newSuggestion.transform.GetChild(2).GetComponent<Text>().text = "Name: " + "Bob";
		newSuggestion.transform.GetChild(3).GetComponent<Text>().text = "" + System.DateTime.Now;


		//Set Parent and Activate
		newSuggestion.transform.parent = mapSuggestionsFolder.transform;
		newSuggestion.SetActive (true);


		
		newSelection.transform.parent = newSuggestion.transform;
		newSelection.SetActive (false);

		sortMapSuggestions ();
		
	}


	private void sortMapSuggestions(){
		int count = mapSuggestionsFolder.transform.childCount; 
		int i = 0;
		while (count > 0) {
			Transform tmp = mapSuggestionsFolder.transform.GetChild(i).GetChild(4);
			tmp.parent = mapSuggestionsFolder.transform;
			mapSuggestionsFolder.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 200 - 100 * i);
			tmp.parent = mapSuggestionsFolder.transform.GetChild(i).transform;
			count = count - 1;
			i = i + 1;
		}
	}

	void Update()
	{
		//Selection Code
		// Click somewhere in the Game View.
		if (selectionSelected == true){
			selectionBox = newSelection.GetComponent<RectTransform>();
			if (Input.GetMouseButtonDown(0)){
				// Get the initial click position of the mouse. No need to convert to GUI space
				// since we are using the lower left as anchor and pivot.
				initialClickPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				
				// The anchor is set to the same place.
				selectionBox.anchoredPosition = initialClickPosition;
				selectReady = true;
			}
			
			// While we are dragging.
			if (Input.GetMouseButton(0)){

				// Store the current mouse position in screen space.
				Vector2 currentMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				
				// How far have we moved the mouse?
				Vector2 difference = currentMousePosition - initialClickPosition;
				
				// Copy the initial click position to a new variable. Using the original variable will cause
				// the anchor to move around to wherever the current mouse position is,
				// which isn't desirable.
				Vector2 startPoint = initialClickPosition;
				
				// The following code accounts for dragging in various directions.
				if (difference.x < 0)
				{
					startPoint.x = currentMousePosition.x;
					difference.x = -difference.x;
				}
				if (difference.y < 0)
				{
					startPoint.y = currentMousePosition.y;
					difference.y = -difference.y;
				}
				
				// Set the anchor, width and height every frame.
				selectionBox.anchoredPosition = startPoint;
				selectionBox.sizeDelta = difference;
			}
			
			// After we release the mouse button.
			if (Input.GetMouseButtonUp(0) && selectReady == true){
				// Reset
				//initialClickPosition = Vector2.zero;
				//selectionBox.anchoredPosition = Vector2.zero;
				//selectionBox.sizeDelta = Vector2.zero;
				//Debug.Log("Off");
				openTabs();
				selectionSelected = false;
				selectReady = false;
			}
		}		

		//assetSelected
		if (assetSelected)
		{
			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
			if (hits.Length > 0 && hits[0].collider != null)
			{
				newObject.transform.position = hits[0].collider.gameObject.transform.position;
				
				
				if (hits.Where(x => x.collider.gameObject.tag == "Asset").Count() > 1)
				{
					//we cannot place a bunny there
					GameObject backgroundBehindPath = hits.Where
						(x => x.collider.gameObject.tag == "Background").First().collider.gameObject;
					//make the sprite material "more red"
					//to let the user know that we can't place a bunny here
					backgroundBehindPath.GetComponent<SpriteRenderer>().color = Constants.RedColor;
					
					if (tempBackgroundBehindPath != backgroundBehindPath)
						ResetTempBackgroundColor();
					//cache it to revert later
					tempBackgroundBehindPath = backgroundBehindPath;
				}
				else //just reset the color on previously set paths
				{
					ResetTempBackgroundColor();
				}
				
			}
		}

	}

	//make background sprite appear as it is
	private void ResetTempBackgroundColor()
	{
		if (tempBackgroundBehindPath != null)
			tempBackgroundBehindPath.GetComponent<SpriteRenderer>().color = Constants.BlackColor;
	}

	private void openTabs(){
		//Open Tabs
		anim.enabled = true;
		anim.Play ("slideInRight");
		isOpen = true;
	}

	private void closeTabs(){
		//Close Tabs
		anim.enabled = true;
		anim.Play ("slideOutRight");
		isOpen = false;
	}

	public void selectionClicked(){
		Debug.Log("Clicked");
		//we're stopping dragging
		if (assetSelected)
			//(ObjectGenerator.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(location, 1 << LayerMask.NameToLayer("BunnyGenerator")))
		{

			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
			ResetTempBackgroundColor();
			//check if we can leave the bunny here
			ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			
			hits = Physics2D.RaycastAll(ray.origin, ray.direction);

			Debug.Log (hits.Where(x => x.collider.gameObject.tag == "Asset").Count());
			//in order to place it, we must have a background and no other bunnies
			if (hits.Where(x => x.collider.gameObject.tag == "Background").Count() > 0
			    && hits.Where(x => x.collider.gameObject.tag == "Asset").Count() == 1)
				
			{
				//we can leave a bunny here, so decrease money and activate it
				newObject.transform.position = 
					hits.Where(x => x.collider.gameObject.tag == "Background")
						.First().collider.gameObject.transform.position;
				newObject.transform.parent = newSelection.transform;
			}
			else
			{
				//we can't leave a bunny here, so destroy the temp one
				Destroy(newObject);
			}
			assetSelected = false;
			
			//Open Tabs
			openTabs();
		}
	}
}
