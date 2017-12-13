using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	float gameLength = 60;

	int gridX = 8;

	int gridY = 5; 

	float turnLength = 2.0f;

	int turns = 1;

	//It will spaw a new cube of a random color 

	void SpawnNewCube ();


	// Use this for initialization
	void Start () {



	}
	
	// Update is called once per frame
	void Update () {

		ProcessKeyboardInput ();


		//take a turn every turnLength
		if (Time.time > turnLength * turns) {
			
			turns++;
			SpawnNewCube ();

		}


	}
}
