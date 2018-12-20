using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContorler : MonoBehaviour {

    public Vector3Int playerStartingPos;
    public Vector3Int enemyStartingPos;
    public AI ai;

	// Use this for initialization
	void Start () {
        ai.doStaff = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
