using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{   
    public DataManager dm;
    public GameObject resultscreen;
    public GameObject gui;
    public KeyframeRecorder kr;
    private int levelid;

    [SerializeField]
    private Sprite[] medals;

    [SerializeField]
    private GameObject ghost;

    public void Start(){
        dm.readFile();
        StartCoroutine(kr.recordKeyframes());
        string name = SceneManager.GetActiveScene().name;
        int levelid = name[name.Length - 1] - '0';

        if (PlayerPrefs.GetInt("ghostEnabled") != 0){
            GameObject g = Instantiate(ghost);
            g.GetComponent<GhostController>().Initialize(dm.leveldata[levelid].replayGhost);
        }
    }
    public void showSummary(int score){
        StopCoroutine(kr.recordKeyframes());
        gui.SetActive(false);
        resultscreen.SetActive(true);
        resultscreen.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Score: " + score;

        // Set medal here
        resultscreen.transform.GetChild(0).GetChild(1).GetComponent<Image>().color = new Color32(255,255,255,255);
        Debug.Log(dm.leveldata.Length);
        if (score >= dm.leveldata[levelid].scoreReqs[2]){
            resultscreen.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = medals[2];
        }
        else if(score >= dm.leveldata[levelid].scoreReqs[1]){
            resultscreen.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = medals[1];
        }
        else if(score >= dm.leveldata[levelid].scoreReqs[0]){
            resultscreen.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = medals[0];
        }
        else{
            resultscreen.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = medals[0];
            resultscreen.transform.GetChild(0).GetChild(1).GetComponent<Image>().color = new Color32(0,0,0,80);
        }
        
        saveScore(score);
        dm.writeFile();
    }

    public void retryLevel(){
        Time.timeScale = 1;
        SceneManager.LoadScene("Level"+levelid);
    }

    public void loadMenu(){
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    private void saveScore(int newScore){
        dm.leveldata[levelid].scores.Add(("Me", newScore));
        dm.leveldata[levelid].scores.Sort((x, y) => y.Item2.CompareTo(x.Item2));

        if (newScore == dm.leveldata[levelid].scores[0].Item2){
            dm.leveldata[levelid].replayGhost = kr.getKeyframes();
            dm.leveldata[levelid].ghostAvailable = true;
        }

        if (newScore >= dm.leveldata[levelid].scoreReqs[2]){
            dm.leveldata[levelid].medalAchieved = 3;
        }
        else if (newScore >= dm.leveldata[levelid].scoreReqs[1] && dm.leveldata[levelid].medalAchieved <= 2){
            dm.leveldata[levelid].medalAchieved = 2;
        }
        else if (newScore >= dm.leveldata[levelid].scoreReqs[0] && dm.leveldata[levelid].medalAchieved <= 1){
            dm.leveldata[levelid].medalAchieved = 1;
        }

        // Unlock next level
        if (dm.leveldata[levelid].medalAchieved != 0 && dm.levelCount - 1 > levelid){
            dm.leveldata[levelid + 1].unlocked = true;
        }
    }
}
