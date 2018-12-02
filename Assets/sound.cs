using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sound : MonoBehaviour {

    public Sprite on;
    public Sprite off;
    public Image i;

    public static bool IsSoundOn = true;

    public void TogleSound()
    {
        if (IsSoundOn)
        {
            IsSoundOn = false;
            i.sprite = off;
        }
        else
        {
            IsSoundOn = true;
            i.sprite = on;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
