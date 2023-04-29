using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelInfo
{
    public int level;
    
    public List<(string name, int score)> scores;

    public bool ghostAvailable;
    public bool unlocked;
    public int medalAchieved;
    public int[] scoreReqs;

    public ReplayKeyframe[] replayGhost;
}
