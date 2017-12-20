using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {

	public int myX, myY;

	public bool nextCube = false;

	public bool active = false;

	GameController myGameController;



	// Use this for initialization
	void Start () {

		myGameController = GameObject.Find ("GameObject").GetComponent<GameController> ();
		
	}
	

	void OnMouseDown () {

		if (!nextCube) {
			
			myGameController.Click (gameObject, myX, myY, gameObject.GetComponent<Renderer> ().material.color, active);

		}
	}


}
