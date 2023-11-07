using System;

[Serializable]
public class HighscoreElement
{
    public string playerName;
    public float score;

    public HighscoreElement (string name, float score) {
        playerName = name;
        this.score = score;
    }

}
