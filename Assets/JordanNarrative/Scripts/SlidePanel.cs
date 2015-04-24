using UnityEngine;
using System.Collections;

public class SlidePanel : MonoBehaviour {

	public GameObject slidingPanel;
	private Animator anim;
	private bool slidIn;

	// Use this for initialization
	void Start () {
		anim = slidingPanel.GetComponent<Animator>();
		slidIn = false;
	}

	//slide the panel out if it's not, otherwise slide it back in
	public void Slide() {
		if (slidIn == false) {
			anim.Play("SlideInLeft");
			slidIn = true;
		} else {
			anim.Play ("SlideOutLeft");
			slidIn = false;
		}
	}
}
