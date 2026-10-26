using UnityEngine;

public class StatsManager : MonoBehaviour
{

    public static StatsManager someStatsManager;
    public string somePlayerName;
    public int someHighScore;
    public float someHighPlayTime;
    public int someHighDefendCount;
    public int someHighLevelCount;

    public void Awake()
    {

        if (someStatsManager == null)
        {
            someStatsManager = this;
            DontDestroyOnLoad(gameObject);
            Time.timeScale = 1f;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ClearHighScores()
    {
        somePlayerName = "";
        someHighScore = 0;
        someHighPlayTime = 0f;
        someHighDefendCount = 0;
        someHighLevelCount = 0;
    }

    internal void SetCurrPlayerName(string someName)
    {
        if (someName != null)
        {
            somePlayerName = someName;    
        }
        
    }

    // high score
    internal void SetHighScore(int someInt)
    {
        if (someInt != null)
        {
            someHighScore = someInt;    
        }
        
    }

    internal int GetHighScore()
    {
        if (someHighScore > 0)
        {
            return someHighScore;
        }
        else
        {
            return 0;
        }
    }  

    // high playtime.
    internal void SetHighPlayTime(float somePlaytime)
    {
        if (somePlaytime != null)
        {
            someHighPlayTime = somePlaytime;
        }
    }

    internal float GetHighPlayTime()
    {
        if (someHighPlayTime != null)
        {
            return someHighPlayTime;
        }
        else
        {
            return 0;
        }
    }

    // high defend count
    internal void SetHighDefendCount(int someDefend)
    {
        if (someDefend != null)
        {
            someHighDefendCount = someDefend;
        }
    }

    public int GetHighDefendCount()
    {
        if (someHighDefendCount != null)
        {
            return someHighDefendCount;
        }
        else
        {
            return 0;
        }
    }

    // high level count

    internal void SetHighLevelCount(int someLevel)
    {
        if (someLevel != null)
        {
            someHighLevelCount = someLevel;
        }
    }

    internal int GetHighLevelCount()
    {
        if (someHighLevelCount != null)
        {
            return someHighLevelCount;
        }
        else
        {
            return 0;
        }
    }

}
