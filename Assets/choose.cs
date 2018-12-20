using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class choose : MonoBehaviour {

    public GameObject Panel;

    public GameObject P;
    public GameObject R;
    public GameObject W;

    public TextMeshProUGUI uri;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OpenPanel()
    {
        Panel.SetActive(true);
    }


    public void dropChange(int index)
    {
        switch (index)
        {
            case 0: //PRE
                P.SetActive(true);
                R.SetActive(false);
                W.SetActive(false);
                break;
            case 1: //RAND
                P.SetActive(false);
                R.SetActive(true);
                W.SetActive(false);
                break;
            case 2: //WWW
                P.SetActive(false);
                R.SetActive(false);
                W.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ChooseLevel1()
    {
        LevelMaker.LevelToLoad = new int[,]
        {
            {1,1,1,1,1},
            {1,3,0,1,1},
            {1,0,2,0,1},
            {1,1,0,4,1},
            {1,1,1,1,1},

        };
        SceneManager.LoadScene("Level");
    }
    public void ChooseLevel2()
    {
        LevelMaker.LevelToLoad = new int[,]
        {
            {1,1,1,1,1,1,1,1,1},
            {1,3,0,0,1,0,0,0,1},
            {1,0,1,2,1,2,1,0,1},
            {1,0,2,2,1,2,2,0,1},
            {1,0,2,2,2,2,2,0,1},
            {1,0,2,2,1,2,2,0,1},
            {1,0,1,2,1,2,1,0,1},
            {1,0,0,0,1,0,0,4,1},
            {1,1,1,1,1,1,1,1,1},
        };
        SceneManager.LoadScene("Level");
    }
    public void ChooseLevel3()
    {
        LevelMaker.LevelToLoad = new int[,]
        {
            {1,1,1,1,1,1,1,1,1},
            {1,3,0,2,2,2,2,2,1},
            {1,0,1,2,2,2,1,2,1},
            {1,2,2,2,2,2,2,2,1},
            {1,2,2,2,1,2,2,2,1},
            {1,2,2,2,2,2,2,2,1},
            {1,2,1,2,2,2,1,0,1},
            {1,2,2,2,2,2,0,4,1},
            {1,1,1,1,1,1,1,1,1},
        };
        SceneManager.LoadScene("Level");
    }
    public void ChooseLevel4()
    {
        LevelMaker.LevelToLoad = new int[,]
        {
            {1,1,1,1,1,1,1,1,1},
            {1,3,0,0,0,0,0,0,1},
            {1,0,1,2,2,2,1,0,1},
            {1,0,2,2,2,2,2,0,1},
            {1,0,2,2,1,2,2,0,1},
            {1,0,2,2,2,2,2,0,1},
            {1,0,1,2,2,2,1,0,1},
            {1,0,0,0,0,0,0,4,1},
            {1,1,1,1,1,1,1,1,1},
        };
        SceneManager.LoadScene("Level");
    }

    public void ChooseWWW()
    {
        StartCoroutine(ChooseWWWLevel());
    }

    public IEnumerator ChooseWWWLevel()
    {
        using (WWW www = new WWW(uri.text))
        {
            yield return www;
            string a = www.text;
        }
    }
}
