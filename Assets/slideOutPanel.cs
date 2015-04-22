using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using Assets.Scripts;

public class slideOutPanel : MonoBehaviour {
	private Camera mainCamera;
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

	//will be colored red if we cannot place a bunny there
	private GameObject tempBackgroundBehindPath;
	//type of bunnies we'll create
	public GameObject[] ObjectPrefab;
	//the starting object for the drag
	bool isDragging = false;
	private GameObject newObject;

	public void objectButtonClicked(int buttonNumber) {
		//Debug.Log (buttonNumber);
		if (buttonNumber > 0){
			//Set grid highlight colour to default
			ResetTempBackgroundColor();
			//Set location to mouse location
			Vector2 location = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			//Set asset is selected
			isDragging = true;
			//Instantiate new object
			newObject = Instantiate(ObjectPrefab[buttonNumber], Input.mousePosition, Quaternion.identity) as GameObject;
		}
		//Close Tabs
		anim.enabled = true;
		anim.Play ("slideOutRight");
		isOpen = false;
	}

	public void suggestionAlert() {
		suggestionIcon.GetComponent<Image> ().sprite = suggestionsAlertSprite;
	}

	void Update()
	{
		if (isDragging)
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
		//we're stopping dragging
		if (Input.GetMouseButtonDown(0) && isDragging)
		//(ObjectGenerator.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(location, 1 << LayerMask.NameToLayer("BunnyGenerator")))
		{
			ResetTempBackgroundColor();
			//check if we can leave the bunny here
			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			
			RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);

			Debug.Log (hits.Where(x => x.collider.gameObject.tag == "Asset").Count());
			//in order to place it, we must have a background and no other bunnies
			if (hits.Where(x => x.collider.gameObject.tag == "Background").Count() > 0
			    && hits.Where(x => x.collider.gameObject.tag == "Tower").Count() == 0
			    && hits.Where(x => x.collider.gameObject.tag == "Asset").Count() == 1)
			
			{
				//we can leave a bunny here, so decrease money and activate it
				newObject.transform.position = 
					hits.Where(x => x.collider.gameObject.tag == "Background")
						.First().collider.gameObject.transform.position;
			}
			else
			{
				//we can't leave a bunny here, so destroy the temp one
				Destroy(newObject);
			}
			isDragging = false;
			//Open Tabs
			anim.enabled = true;
			anim.Play ("slideInRight");
			isOpen = true;
		}
	}

	//make background sprite appear as it is
	private void ResetTempBackgroundColor()
	{
		if (tempBackgroundBehindPath != null)
			tempBackgroundBehindPath.GetComponent<SpriteRenderer>().color = Constants.BlackColor;
	}

}
