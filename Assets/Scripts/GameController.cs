using System.Collections;
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
