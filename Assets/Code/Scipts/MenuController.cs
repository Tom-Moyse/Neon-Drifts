using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MenuController : MonoBehaviour
{
    private int activeMenu = 0;
    private int selectedLevel = 0;

    public DataManager dm;
    public GameObject[] buttons;
    public GameObject infoDisplay;
    public GameObject infoScores;

    [SerializeField]
    private Sprite[] medals;
    public void enableMenu(int id){
        int count = 0;
        foreach(Transform c in transform){
            if (count++ == id){
                c.gameObject.SetActive(true);
                activeMenu = id;
            }
            else{
                c.gameObject.SetActive(false);
            }
        }
        
    }
    public void Start(){
        dm.readFile();

        for (int i = 0; i < dm.levelCount; i++){
            if (!dm.leveldata[i].unlocked){
                buttons[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    public void loadLevel(bool hasGhost){
        SceneManager.LoadScene("Level"+selectedLevel);
        PlayerPrefs.SetInt("ghostEnabled", hasGhost ? 1 : 0);
    }

    public void selectLevel(int levelID){
        selectedLevel = levelID;
        infoDisplay.SetActive(true);

        LevelInfo levelInfo = dm.leveldata[selectedLevel];
        
        infoDisplay.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Level " + (levelID + 1);
        foreach (Transform child in infoScores.transform){
            GameObject.Destroy(child.gameObject);
        }
        
        foreach ((string name, int score) in levelInfo.scores){
            GameObject newText = new GameObject("Text");
            newText.AddComponent<TextMeshProUGUI>();
            newText.GetComponent<TMP_Text>().text = name + ": " + score;
            GameObject.Instantiate(newText, infoScores.transform);
        }

        if (levelInfo.medalAchieved == 0){
            infoDisplay.transform.GetChild(5).GetComponent<Image>().sprite = medals[0];
            infoDisplay.transform.GetChild(5).GetComponent<Image>().color = new Color32(0,0,0,80);
        }
        else{
            infoDisplay.transform.GetChild(5).GetComponent<Image>().color = new Color32(255,255,255,255);
            infoDisplay.transform.GetChild(5).GetComponent<Image>().sprite = medals[levelInfo.medalAchieved - 1];
        }
        
        infoDisplay.transform.GetChild(4).GetComponent<Button>().interactable = levelInfo.ghostAvailable;
    }
}
