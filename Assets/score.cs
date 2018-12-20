using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class score : MonoBehaviour {

    public GameObject Panel;
    public TextMeshProUGUI text;


    public static int AktualScore = 0;



    // Use this for initialization
    void Start () {
        if (File.Exists(Application.persistentDataPath + "/scores.lsv"))
        {
            var scores = File.ReadAllLines(Application.persistentDataPath + "/scores.lsv").OrderByDescending(s=>(int.Parse(s.Split('|')[0]))).Take(10).ToList();
            string t = "";
            int place = 1;
            foreach (var score in scores)
            {
                var sss = score.Split('|');
                t += $"{place++}.{sss[1]}({sss[0]}) \n";
            }
            text.text = t;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OpenPanel()
    {
        Panel.SetActive(true);
    }
}