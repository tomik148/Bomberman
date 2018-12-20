using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class updateScore : MonoBehaviour {

    public TextMeshProUGUI TextMeshPro;

	// Use this for initialization
	void Start () {
        TextMeshPro.text = score.AktualScore.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
