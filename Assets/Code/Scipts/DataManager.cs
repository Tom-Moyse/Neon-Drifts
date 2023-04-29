using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    public LevelInfo[] leveldata;
    private string savePath;
    public int levelCount;

    void Awake(){
        savePath = Application.persistentDataPath + "/gamedata.json";
    }

     public void readFile()
    {
        Debug.Log(savePath);
        // Does the file exist?
        if (File.Exists(savePath))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(savePath);

            // Deserialize the JSON data 
            //  into a pattern matching the GameData class.
            leveldata = JsonConvert.DeserializeObject<LevelInfo[]>(fileContents);
        }
        else{
            initFile();
        }
    }

    public void writeFile()
    {
        // Serialize the object into JSON and save string.
        string jsonString = JsonConvert.SerializeObject(leveldata);

        // Write JSON to file.
        File.WriteAllText(savePath, jsonString);
    }

    private void initFile(){
        leveldata = new LevelInfo[levelCount];

        for (int i = 0; i < levelCount; i++){
            LevelInfo LvlInfo = new LevelInfo();
            LvlInfo.level = i;
            LvlInfo.scores = new List<(string, int)>();
            LvlInfo.ghostAvailable = false;
            LvlInfo.unlocked = false;
            LvlInfo.medalAchieved = 0;
            LvlInfo.scoreReqs = new int[]{1000,3000,5000};
            leveldata[i] = LvlInfo;
        }

        leveldata[0].unlocked = true;

        writeFile();
    }
}
