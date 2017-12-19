﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	float gameLength = 60;

	int gridX = 8;

	int gridY = 5; 

	float turnLength = 2.0f;

	int turns = 1;

	public GameObject cubePrefab;

	GameObject[,] grid;

	GameObject nextCube;

	Vector3 cubePos;

	Vector3 nextCubePos = new Vector3 (7, 10, 0);

	public Text scoreText;

	Color[] myColors = { Color.red, Color.blue, Color.yellow, Color.green, Color.magenta };

	public Text nextCubeText;

	bool gameOver = false;

	GameObject activeCube = null;






	// Use this for initialization
	void Start () {

		CreateGrid ();

	}

	void CreateGrid(){

		grid = new GameObject[gridX, gridY];

		for (int y = 0; y < gridY; y++) {
			for (int x = 0; x < gridX; x++) {
				
				cubePos = new Vector3 (x * 2, y * 2, 0);
				grid[x,y] = Instantiate (cubePrefab, cubePos, Quaternion.identity);
				grid [x, y].GetComponent<CubeController> ().myX = x;
				grid [x, y].GetComponent<CubeController> ().myY = y;

			}
		}
	}

	void CreateNextCube () {
		
		nextCube = Instantiate (cubePrefab, nextCubePos, Quaternion.identity);
		nextCube.GetComponent<Renderer> ().material.color = myColors [ Random.Range (0, myColors.Length) ];
		nextCube.GetComponent<CubeController> ().nextCube = true;

	}

	void EndGame (bool win){
		
		//end game 
		if (win) {
		
			// players win!
			nextCubeText.text = "You Win!";
		
		} else {
	
			// players lose!
			nextCubeText.text = "You Lose. Try again!";
		}

		// next cube exist in case the timing 

		Destroy (nextCube);
		nextCube = null;


		// disable all the cubes in the grid
		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {
				grid [x, y].GetComponent<CubeController> ().nextCube = true;
			}
		}

		gameOver = true;

	}

	GameObject PickWhiteCube (List<GameObject>whiteCubes){
	
		//no white cubes in a row
		if (whiteCubes.Count == 0) {
			
			//This is the error value				
			return null;
		
		}

			//Pick a random white cube
			return whiteCubes[ Random.Range(0, whiteCubes.Count) ];
	}

	GameObject FindAvailableCube (int y){

		List<GameObject> whiteCubes = new List<GameObject> ();

		// make a list of white cubes
		for (int x = 0; x < gridX; x++) {
			
			if (grid [x, y].GetComponent<Renderer> ().material.color == Color.white) {

				whiteCubes.Add(grid[x,y]);

			}
		}

		return PickWhiteCube (whiteCubes);
	}

	GameObject FindAvailableCube(){

		List<GameObject> whiteCubes = new List<GameObject> ();

		// make a list of white cubes
		for (int y = 0; y < gridY; y++) {
			
			for (int x = 0; x < gridX; x++) {
				
				if (grid [x, y].GetComponent<Renderer> ().material.color == Color.white) {

					whiteCubes.Add (grid [x, y]);
				}
			}
		}
			
		return PickWhiteCube (whiteCubes);
	}

	//It's going to set the color of cubes
	void SetCubeColor (GameObject myCube, Color color){
	
		// no available cube in that row
		if (myCube == null) {
			
			EndGame (false);

		} else {
		
			// assign the nextCube's color to the chosen cube
			myCube.GetComponent<Renderer> ().material.color = color;
			Destroy (nextCube);
			nextCube = null;
		
		}
	}

	void PlaceNextCube (int y) {

		GameObject whiteCube = FindAvailableCube (y);

		SetCubeColor (whiteCube, nextCube.GetComponent<Renderer> ().material.color);

	}

	void AddBlackCube(){

		GameObject whiteCube = FindAvailableCube ();

		// use a color value that is beyond the max
		SetCubeColor (whiteCube, Color.black);

	}

	void ProccessKeyboardInput(){
	
		int numKeyPressed = 0;

		if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.Keypad1)) {
			numKeyPressed = 1;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown (KeyCode.Keypad2)) {
			numKeyPressed = 2;
		}
		if (Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown (KeyCode.Keypad3)) {
			numKeyPressed = 3;
		}
		if (Input.GetKeyDown (KeyCode.Alpha4) || Input.GetKeyDown (KeyCode.Keypad4)) {
			numKeyPressed = 4;
		}
		if (Input.GetKeyDown (KeyCode.Alpha5) || Input.GetKeyDown (KeyCode.Keypad5)) {
			numKeyPressed = 5;
		}	

		// if we still have a next cube and the player pressed a valid number key
		if (nextCube != null && numKeyPressed != 0) {

			// put it in the specified row, subtracting 1 since the grid array has a 0-based index
			PlaceNextCube (numKeyPressed - 1);
		}
	} 

	public void ProcessClick (GameObject clickedCube, int x, int y, Color cubeColor, bool active){
	
		// did we click on a colored cube?
		if (cubeColor != Color.white && cubeColor != Color.black) {

			//  if that cube was active
			if (active) {
				
				// deactivate it
				clickedCube.transform.localScale /= 1.5f;
				clickedCube.GetComponent<CubeController> ().active = false;
				activeCube = null;

			} else {
			
				// deactivate any previously active cube
				if (activeCube != null) {

					activeCube.transform.localScale /= 1.5f;
					activeCube.GetComponent<CubeController> ().active = false;

				}

				// activate the newly clicked cube
				clickedCube.transform.localScale *= 1.5f;
				clickedCube.GetComponent<CubeController> ().active = true;
				activeCube = clickedCube;
			}

		} else if (cubeColor == Color.white && activeCube != null) {
		
			int xDist = clickedCube.GetComponent<CubeController> ().myX - activeCube.GetComponent<CubeController> ().myX;
			int yDist = clickedCube.GetComponent<CubeController> ().myY - activeCube.GetComponent<CubeController> ().myY;
		
			// if we are within 1 including diagonals
			if (Mathf.Abs (yDist) <= 1 && Mathf.Abs (xDist) <= 1){

				// set the clicked cube to be active
				clickedCube.GetComponent<Renderer> ().material.color = activeCube.GetComponent<Renderer> ().material.color;
				clickedCube.transform.localScale *= 1.5f;
				clickedCube.GetComponent<CubeController> ().active = true;

				// set the old active cube to be white and not active
				activeCube.GetComponent<Renderer> ().material.color = Color.white;
				activeCube.transform.localScale /= 1.5f;
				activeCube.GetComponent<CubeController> ().active = false;

				// keep track of the new active cube
				activeCube = clickedCube;

			}

		}
	
	}

	bool IsRainbowPlus (int x, int y) {
		
		Color a = grid [x, y].GetComponent<Renderer> ().material.color;
		Color b = grid [x + 1, y].GetComponent<Renderer> ().material.color;
		Color c = grid [x - 1, y].GetComponent<Renderer> ().material.color;
		Color d = grid [x, y + 1].GetComponent<Renderer> ().material.color;
		Color e = grid [x, y - 1].GetComponent<Renderer> ().material.color;

		// if any of the colors are white or black, there's no rainbow plus
		if (a == Color.white || a == Color.black ||
		    b == Color.white || b == Color.black ||
		    c == Color.white || c == Color.black ||
		    d == Color.white || d == Color.black ||
		    e == Color.white || e == Color.black) {
			return false;
		}

		// ensure that every color is different from every other color
		if (a != b && a != c && a != d && a != e &&
		    b != c && b != d && b != e &&
		    c != d && c != e &&
		    d != e) {
			return true;

		} else {
		
			return false;
		
		}
	}

	bool IsSameColorPlus (int x, int y) {
		
		if (grid [x, y].GetComponent<Renderer> ().material.color != Color.white &&
			grid [x, y].GetComponent<Renderer> ().material.color != Color.black &&
			grid [x, y].GetComponent<Renderer> ().material.color == grid [x + 1, y].GetComponent<Renderer> ().material.color &&
			grid [x, y].GetComponent<Renderer> ().material.color == grid [x - 1, y].GetComponent<Renderer> ().material.color &&
			grid [x, y].GetComponent<Renderer> ().material.color == grid [x, y + 1].GetComponent<Renderer> ().material.color &&
			grid [x, y].GetComponent<Renderer> ().material.color == grid [x, y - 1].GetComponent<Renderer> ().material.color) {
			return true;

		} else {
			
			return false;
		}
	}

	void MakeBlackPlus (int x, int y) {
		
		// this is an error check to ensure that the x and y aren't on the edge of the grid
		if (x == 0 || y == 0 || x == gridX - 1 || y == gridY - 1) {
			return;
		}

		grid [x, y].GetComponent<Renderer> ().material.color = Color.black;
		grid [x+1, y].GetComponent<Renderer> ().material.color = Color.black;
		grid [x-1, y].GetComponent<Renderer> ().material.color = Color.black;
		grid [x, y+1].GetComponent<Renderer> ().material.color = Color.black;
		grid [x, y-1].GetComponent<Renderer> ().material.color = Color.black;

		// if we had an active cube and it was involved in the plus
		if (activeCube != null && activeCube.GetComponent<Renderer>().material.color == Color.black) {
			// deactivate it
			activeCube.transform.localScale /= 1.5f;
			activeCube.GetComponent<CubeController> ().active = false;
			activeCube = null;
		}
	}


	// Update is called once per frame
	void Update () {

		//ProcessKeyboardInput ();


		//take a turn every turnLength seconds 
		if (Time.time > turnLength * turns) {
			
			turns++;
			//SpawnNewCube ();

		}


	}
}
