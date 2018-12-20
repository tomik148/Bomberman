using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class saveScore : MonoBehaviour {

    public TextMeshProUGUI TextMeshPro;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    public void Save()
    {
        File.AppendAllLines(Application.persistentDataPath + "/scores.lsv",new string[] { $"{score.AktualScore}|{TextMeshPro.text}" });
        Menu();
    }


    public void Menu()
    {
        score.AktualScore = 0;
        SceneManager.LoadScene("Menu");
    }
}
