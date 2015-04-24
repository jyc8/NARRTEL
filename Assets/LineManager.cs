using UnityEngine;
using System.Collections;

public class LineManager : MonoBehaviour {
	private bool drawing;
	public GameObject dot;
	public GameObject map;
	private GameObject evt;

	// Update is called once per frame
	void Update () {
		if (drawing){
			if(Input.GetMouseButtonDown(0)){
				GameObject dots = (GameObject)Instantiate (dot, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
				dots.transform.position = new Vector3(dots.transform.position.x, dots.transform.position.y, 0);
				dots.transform.parent = evt.transform;
			}
		}
	}

	public void startDrawing(GameObject e){
		evt = e;
		if (drawing){
			drawing = false;
		} else{
			drawing = true;
		}
	}
}
