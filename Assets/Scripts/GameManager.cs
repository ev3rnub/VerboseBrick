//Course: GPE104 
//Prof: Matthew Henry 
//Student: Chad V

// GameManager.cs
// The GameManager is simply that, a game manager, if all the bricks are gone, it spawns more. If the ball gets past he player it
// end the game. 


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using System.Collections;
using System.Collections.Generic; // for List<>
using TMPro;
using System;

public class GameManager : MonoBehaviour
{   
    [Header("GameObjects")]
    public static GameManager someGameManager;
    public StatsManager someStatsManager;
    public ControllerPlayer controlPaddle;
    public ControllerBall controllerBall; // get controller ref for player entity.//
    public Health targetHealth;
    public GameObject EnemyBlock;
    private string enemyBlockPrefabPath = "Prefabs/EnemyBlock";
    public GameObject BrickRow;
    private string brickRowPrefabPath = "Prefabs/BrickRow";
    public GameObject ballRecenterWall;
    public GameObject EndGameScreen;
    public TextMeshProUGUI EndGameScore;

    private float sceneStartTime;
    private float lastScenePauseTime;
    private float sceneRuntime;

    [Header("EnemyPaddle Spawn Slot locations")]
    public Vector3 spawnSlot0 = new Vector3(-6.38f, 2.08f, 0f); // far left
    public Vector3 spawnSlot1 = new Vector3(-2.63f, 2.08f, 0f);
    public Vector3 spawnSlot2 = new Vector3(-0.2449999f, 2.08f, 0f);
    public Vector3 spawnSlot3 = new Vector3(4.12f, 2.08f, 0f);
    public Vector3 spawnSlot4 = new Vector3(7.495f, 2.08f, 0f); // far right
    private List<Vector3> slotArray = new List<Vector3>();

    [Header("BrickRow tracking")]
    private List<GameObject> brickRowArray = new List<GameObject>();
    
    [Header("BrickRow Positions")]
    public Vector3 BrickRow0 = new Vector3(-9.25500011f, 4.94999981f, 0f);
    public Vector3 BrickRow1 = new Vector3(-9.25500011f, 4.32499981f, 0f);
    public Vector3 BrickRow2 = new Vector3(-9.25500011f, 3.69999981f, 0f);
    public Vector3 BrickRow3 = new Vector3(-9.30000019f, 0.100000001f, 0f);

    
    [Header("GUI")]
    public TextMeshProUGUI someWave;
    public TextMeshProUGUI someLabelScore;
    public TextMeshProUGUI someBulletsFired;
    public TextMeshProUGUI someHighScoreLabel;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI somePaddleDefends;
    public int someScore = 0;
    public int someHighScore = 0;
    public int bulletsFired = 0;
    public int someHighBulletsFired = 0;
    public int paddleDefends = 0;
    public int someHighPaddleDefends = 0;
    public int failures = 0;
    public int paddleHealth = 0;
    private int currentLevel = 0;
    private int someHighCurrentLevel = 0;
    private int currentRowLevel = 2;
    private int bricksPerRow = 9;
    private int currBrickCnt = 0;
    private int gamePlayCounter = 0;

    [Header("BUTTONs")]
    public Button someQuitBtn;
    public Button someRestartBtn;


    [Header("Block Colors")]
    public Color white = new Color(1f, 1f, 1f, 1f);
    public Color red = new Color(1f, 0f, 0f, 1f);
    public Color blue = new Color(0f, 0f, 1f, 1f);
    public Color purple = new Color(0.5f, 0f, 0.5f, 1f);
    [Header("Block Spawn Offset")]
    public Vector3 someOffset = new Vector3(2f, 0f, 0f);


    public void Awake()
    {

        if (someGameManager == null)
        {
            someGameManager = this;
            Time.timeScale = 1f;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ResetGame()
    {
        // Reset all vars to initial state
        someScore = 0;
        someHighScore = 0;
        bulletsFired = 0;
        someHighBulletsFired = 0;
        paddleDefends = 0;
        someHighPaddleDefends = 0;
        failures = 0;
        paddleHealth = 0;
        currentLevel = 0;
        someHighCurrentLevel = 0;
        currentRowLevel = 0;
        currBrickCnt = 0;
        gamePlayCounter = 0;
        sceneStartTime = Time.time;
        lastScenePauseTime = 0f;
        sceneRuntime = 0f;

        // Re-initialize the slotArray
        slotArray.Clear();
        slotArray.Add(spawnSlot0);
        slotArray.Add(spawnSlot1);
        slotArray.Add(spawnSlot2);
        slotArray.Add(spawnSlot3);
        slotArray.Add(spawnSlot4);

        // Clear any old enemies or brickrows.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyPaddle");
        foreach (GameObject enemy in enemies){
            Destroy(enemy);
        } 

        GameObject[] brickRows = GameObject.FindGameObjectsWithTag("BrickRow");
        foreach (GameObject brickrow in brickRows)
        {
            Destroy(brickrow);
        }


        //reset player
        controlPaddle.transform.position = new Vector3(1.5309999f, -3.66000009f, 0f);
        //reset ball to origin
        controllerBall.transform.position = new Vector3(0f, 0f, 0f);
        //Read adjust ball velocity.
        Rigidbody2D rb = controllerBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float speed = 5f; // Adjust this value to change how fast it moves down
            rb.linearVelocity = Vector2.down * speed;
        }
        if (EndGameScreen != null)
        {
            EndGameScreen.SetActive(false);
        }


        // Update UI
        if (someLabelScore == null)
        {
            Debug.Log("someLabelScore is NULL attempting to set");
            someLabelScore = GameObject.Find("Score")?.GetComponent<TextMeshProUGUI>();
            if (someLabelScore == null)
            {
                someLabelScore = Resources.Load<TextMeshProUGUI>("Prefabs/ScoreLabel");
            }

        }
        someLabelScore.text = $"Score: {someScore}";
        someWave.text = $"Wave: {currentLevel}";
        someBulletsFired.text = $"Bullets Fired: {bulletsFired}";
        EndGameScore.text = $"Wave: {currentLevel}\nScore: {someScore}";
        SpawnRow(0);
        SpawnEnemy(1, EnemyBlock);
        Time.timeScale = 1f;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // This runs after scene reload
        if (scene.name == "DefaultLevel")
        {
            if (this == someGameManager)
            {
                Debug.Log("OnSceneLoaded ========");

                // Load prefabs if they are still null
                if (controlPaddle == null)
                {
                   controlPaddle = FindObjectOfType<ControllerPlayer>();
                }

                if (controllerBall == null)
                {
                    controllerBall = FindObjectOfType<ControllerBall>();
                }

                if (targetHealth == null)
                {
                    targetHealth = FindObjectOfType<Health>();    
                }

                if (ballRecenterWall == null)
                {
                    ballRecenterWall = GameObject.FindGameObjectWithTag("BallRecenterWall");    
                }

                if (EndGameScreen == null)
                {
                    EndGameScreen = GameObject.FindGameObjectWithTag("EndGameScreen");    
                }

                if (someWave == null)
                {
                    someWave = GameObject.Find("WaveLabel")?.GetComponent<TextMeshProUGUI>();    
                }

                if (someLabelScore == null)
                {
                    someLabelScore = GameObject.Find("ScoreLabel")?.GetComponent<TextMeshProUGUI>();    
                }

                if (someBulletsFired == null)
                {
                    someBulletsFired = GameObject.Find("BulletLabel")?.GetComponent<TextMeshProUGUI>();    
                }

                if (someHighScoreLabel == null)
                {
                    someHighScoreLabel = GameObject.Find("HighScoreLabel")?.GetComponent<TextMeshProUGUI>();            
                }

                if (timeText == null)
                {
                    timeText = GameObject.Find("SceneTimeLabel")?.GetComponent<TextMeshProUGUI>();    
                }

                if (somePaddleDefends == null)
                {
                    somePaddleDefends = GameObject.Find("DefendsLabel")?.GetComponent<TextMeshProUGUI>();            
                }

                if (someQuitBtn == null)
                {
                    someQuitBtn = GameObject.Find("QuitButton")?.GetComponent<Button>();            
                }

                if (someRestartBtn == null)
                {
                    someRestartBtn = GameObject.Find("RestartButton")?.GetComponent<Button>();            
                }

                if (PlayerPrefs.HasKey("HighScore"))
                {
                    someHighScoreLabel.text = $"HighScore: {PlayerPrefs.GetInt("HighScore", 0)}";
                }                
            }           
        }
        
    }

    void Start()
    {
        ResetGame();
        sceneStartTime = Time.time;
        Time.timeScale = 1f;
        if (PlayerPrefs.HasKey("HighScore"))
        {
            someHighScoreLabel.text = $"HighScore: {PlayerPrefs.GetInt("HighScore", 0)}";
        }
        // Ensure references are valid after scene reload
        if (EnemyBlock == null)
        {
            EnemyBlock = Resources.Load<GameObject>(enemyBlockPrefabPath);
            if (EnemyBlock == null)
            {
               EnemyBlock = GameObject.FindGameObjectWithTag("EnemyPaddle");
            }
        }

        if (BrickRow == null)
        {
            BrickRow = Resources.Load<GameObject>(brickRowPrefabPath);
            if (BrickRow == null)
            {
            BrickRow = GameObject.FindGameObjectWithTag("BrickRow");
            }
        }

        // Setup button listeners
        if (someQuitBtn != null)
        {
            someQuitBtn.onClick.AddListener(OnQuitBtnPressed);
        }
        if (someRestartBtn != null)
        {
            someRestartBtn.onClick.AddListener(OnRestartBtnPressed);
        }
    }

    public void Update()
    {
        sceneRuntime = Time.time - sceneStartTime;
        lastScenePauseTime = sceneRuntime;
        if (timeText != null)
        {
            timeText.text = $"Time: {sceneRuntime:F2}s";
        }
        
    }

    void OnDestroy()
    {
        // Setup button listeners
        if (someQuitBtn != null)
        {
            someQuitBtn.onClick.RemoveListener(OnQuitBtnPressed);
        }
        if (someRestartBtn != null)
        {
            someRestartBtn.onClick.RemoveListener(OnRestartBtnPressed);
        }        
    }

    void OnEnable()
    {
        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Always unsubscribe when disabled or destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnRestartBtnPressed()
    {
        ResetGame();
    }

    public void OnQuitBtnPressed()
    {
        Debug.Log("Exiting to Main Menu...");
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }    

    public void SomeGameObjectDied(GameObject someGameObject)
    {

        if (someGameObject.CompareTag("EnemyPaddle"))
        {
            if (currentLevel != 0)
            {
                UpdateScoreLabel(100 * currentLevel);    
            }
            else
            {
                UpdateScoreLabel(100);
            }
            
        }

        if (someGameObject.CompareTag("Brick"))
        {
            if (currentLevel != 0)
            {
                UpdateScoreLabel(50 * currentLevel);
            }
            else
            {
                UpdateScoreLabel(50);
            }
        }

        if (someGameObject.CompareTag("Brick"))
        {
            currBrickCnt-= 1;
            
            Debug.Log($"BRICKCNT: {currBrickCnt}");
            if (currBrickCnt <= 0)
            {
                currBrickCnt = 0;
                currentLevel += 1;
                controllerBall.transform.position = new Vector3(0f, 0f, 0f);
                Debug.Log("ACTIVE============");
                ballRecenterWall.SetActive(true);
                
                // this starts a coroutine which disabls a edge collider that blocks the players bottom side;
                // this prevents the ball from ending the game when spawning in gameobjects/resetting ball. 

                StartCoroutine(DestroyAfterTime());

                // simple wave/level spawning system.
                if (currentLevel == 1)
                {
                    SpawnRow(0);
                    SpawnRow(1);
                    SpawnEnemy(1, EnemyBlock);
                }
                // else if (currentLevel <= 2)
                // {
                //     SpawnRow(0);
                //     SpawnRow(1);
                //     SpawnEnemy(1, EnemyBlock);
                //     SpawnEnemy(1, EnemyBlock);
                // }
                // else if (currentLevel >= 3)
                // {
                //     SpawnRow(0);
                //     SpawnRow(1);
                //     SpawnRow(2);
                //     SpawnEnemy(1, EnemyBlock);
                //     SpawnEnemy(1, EnemyBlock);
                //     SpawnEnemy(1, EnemyBlock);
                // }
                // else if (currentLevel >= 4)
                // {
                //     SpawnRow(0);
                //     SpawnRow(1);
                //     SpawnRow(2);
                //     SpawnRow(3);
                //     SpawnEnemy(1, EnemyBlock);
                //     SpawnEnemy(1, EnemyBlock);
                //     SpawnEnemy(1, EnemyBlock);
                //     SpawnEnemy(1, EnemyBlock);
                // }
                // else if (currentLevel >= 5)
                // {
                //     SpawnRow(0);
                //     SpawnRow(1);
                //     SpawnRow(2);
                //     SpawnRow(3);
                //     SpawnEnemy(1, EnemyBlock);
                //     SpawnEnemy(1, EnemyBlock);
                //     SpawnEnemy(1, EnemyBlock);
                //     SpawnEnemy(1, EnemyBlock);
                // }
                else
                {
                    var randRowAmt = UnityEngine.Random.Range(0, 4);
                    var randEnemyCnt = UnityEngine.Random.Range(0, 5);
                    var fourthRow = UnityEngine.Random.Range(0,1);
                    if (randRowAmt == 4)
                    {
                        SpawnRow(0);
                        SpawnRow(1);
                        SpawnRow(2);
                        SpawnRow(3);
                    }
                    else if (randRowAmt == 3)
                    {
                        SpawnRow(0);
                        SpawnRow(1);
                        SpawnRow(2);
                        if (fourthRow == 1)
                        {
                            SpawnRow(3);
                        }
                    }
                    else if (randRowAmt == 2)
                    {
                        SpawnRow(0);
                        if (fourthRow == 1)
                        {
                            SpawnRow(3);
                        }
                        else
                        {
                            SpawnRow(1);
                        }
                    }
                    else
                    {
                        SpawnRow(0);
                        if (fourthRow == 1)
                        {
                            SpawnRow(1);
                            SpawnRow(2);
                            SpawnRow(3);
                        }
                    }

                    for (var i = 0; i < randEnemyCnt; i++)
                    {
                        SpawnEnemy(1, EnemyBlock);
                    }

                }
            }
        }

        // this keep the ball from going into the players side on reset.
        IEnumerator DestroyAfterTime()
        {
            yield return new WaitForSeconds(0.5f);
            ballRecenterWall.SetActive(false);
            someBulletsFired.text = $"Bullets Fired: {bulletsFired}";
        }

    }

    public void UpdateScoreLabel(int someAmt)
    {
        someScore += someAmt;
        if (someLabelScore != null)
        {
            someLabelScore.text = $"Score: {someScore}";
            //putting this hear for now will need to make an updateShotsFired    
        }
    }

    public void Defend()
    {
        paddleDefends+=1;
        somePaddleDefends.text = $"Defends: {paddleDefends}";
    }

    public void BulletHit()
    {
        //some fx
        //some sfx
        //somelogic

        Debug.Log("THUMP + 1 pts");
        if (targetHealth != null)
        {
            var someInt = targetHealth.SubHealth(5);
            UpdateScoreLabel(1);
        }
        
    }

    public void BrickHit()
    {
        //some fx
        //some sfx
        //somelogic

        Debug.Log("BOOM");
        // eventually this goes to a health manager but gotta start somewhere.
        var someInt = targetHealth.SubHealth(25);
    }

    public void BottomHit()
    {
        var highScore = false;
        var highDefend = false;
        var highLevel = false;
        var highTime = false;


        // this needs to be its own function.
        Debug.Log("BOTTOM");
        Debug.Log("=====!!GAME OVER!!======");
        if (PlayerPrefs.HasKey("HighScore"))
        {
            int currHighScore = PlayerPrefs.GetInt("HighScore", 0);
            if (currHighScore < someScore)
            {
                PlayerPrefs.SetInt("HighScore", someScore);
                highScore = true;
                if (highScore)
                {
                    someStatsManager.SetHighScore(someScore);
                }                
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", someScore);
            highScore = true;
            if (highScore)
            {
                someStatsManager.SetHighScore(someScore);
            }            
        }

        if (PlayerPrefs.HasKey("HighPaddleDefends"))
        {
            int currHighPaddleDefend = PlayerPrefs.GetInt("HighPaddleDefends", 0);
            if (currHighPaddleDefend < paddleDefends)
            {
                PlayerPrefs.SetInt("HighPaddleDefends", paddleDefends);
                highDefend = true;
                if (highDefend)
                {
                    someStatsManager.SetHighDefendCount(paddleDefends);
                }                
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighPaddleDefends", paddleDefends);
            highDefend = true;
            if (highDefend)
            {
                someStatsManager.SetHighDefendCount(paddleDefends);
            }            
        }

        if (PlayerPrefs.HasKey("HighLevel"))
        {
            int currHighLevel = PlayerPrefs.GetInt("HighLevel", 0);
            if (currHighLevel < currentLevel)
            {
                PlayerPrefs.SetInt("HighLevel", currentLevel);
                highLevel = true;
                if (highLevel)
                {
                    someStatsManager.SetHighLevelCount(currentLevel);
                }                
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighLevel", currentLevel);
            highLevel = true;
            if (highLevel)
            {
                someStatsManager.SetHighLevelCount(currentLevel);
            }            
        }
        if (PlayerPrefs.HasKey("HighTime"))
        {
            float currHighTime = PlayerPrefs.GetFloat("HighTime", 0f);
            if (currHighTime < lastScenePauseTime)
            {
                PlayerPrefs.SetFloat("HighTime", lastScenePauseTime);
                highTime = true;

                if (highTime)
                {
                    someStatsManager.SetHighPlayTime(lastScenePauseTime);
                }
            }
        }
        else
        {
            PlayerPrefs.SetFloat("HighTime", lastScenePauseTime);
            highTime = true;
            if (highTime)
            {
                someStatsManager.SetHighPlayTime(lastScenePauseTime);
            }            
        }
        var highScoreString = "";
        if (highScore)
        {
            highScoreString = $"High Score: {someScore}\n";
        }
        else
        {
            highScoreString = $"Score: {someScore}\n";
        }
        var highDefendString = "";
        if (highDefend)
        {
            highDefendString = $"High Defend: {paddleDefends}\n";
        }
        else
        {
            highDefendString = $"Defends: {paddleDefends}\n";
        }
        var highLevelString = "";
        if (highLevel)
        {
            highLevelString = $"High Wave: {currentLevel}\n";
        }
        else
        {
            highLevelString = $"Wave: {currentLevel}\n";
        }
        var highTimeString = "";
        if (highTime)
        {
            highTimeString = $"High Time: {lastScenePauseTime}\n";
        }
        else
        {
            highTimeString = $"Time: {lastScenePauseTime}\n";
        }
        EndGameScore.text = $"Good Job!\n{highLevelString}{highScoreString}{highDefendString}{highTimeString}";




        Time.timeScale = 0f;
        EndGameScreen.SetActive(true);
        
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }    

    public void SpawnEnemy(int someAmt, GameObject someGameObject)
    {
        //set level
        //currentLevel += 1;
        someWave.text = $"Wave: {currentLevel}";
        //spawn enemies
        GameObject spawned = Instantiate(someGameObject, slotArray[currentLevel], Quaternion.identity); 
        //rotate it, for some reason unity loses prefab rotation
        spawned.transform.Rotate(0f, 0f, -180f);
        targetHealth = someGameObject.GetComponent<Health>();
        //define healh
        targetHealth.AddHealth(100 * currentLevel);
    }

    //row spawning
    public void SpawnRow(int rowToSpawn)
    {
        switch (rowToSpawn)
        {
            case 0:
            {
                GameObject someBrickRow = Instantiate(BrickRow, BrickRow0, Quaternion.identity);
                currBrickCnt += 9;
                brickRowArray.Add(someBrickRow);
                Debug.Log($"SET currBrickCnt: {currBrickCnt}");
                break;
            }
            case 1:
            {
                GameObject someBrickRow = Instantiate(BrickRow, BrickRow1, Quaternion.identity);
                brickRowArray.Add(someBrickRow);
                //get child bricks to change color
                Transform brickRowTransform = someBrickRow.transform;

                for (var i = 0; i < brickRowTransform.childCount; i++)
                {
                    GameObject someBrick = brickRowTransform.GetChild(i).gameObject;
                    SpriteShapeRenderer spriteRenderer = someBrick.GetComponent<SpriteShapeRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = blue;
                    }
                }
                currBrickCnt += 9;
                Debug.Log($"SET currBrickCnt: {currBrickCnt}");
                break; 
            }
            case 2:
            {
                GameObject someBrickRow = Instantiate(BrickRow, BrickRow2, Quaternion.identity);
                brickRowArray.Add(someBrickRow);
                //get child bricks to change color
                Transform brickRowTransform = someBrickRow.transform;

                for (var i = 0; i < brickRowTransform.childCount; i++)
                {
                    GameObject someBrick = brickRowTransform.GetChild(i).gameObject;
                    SpriteShapeRenderer spriteRenderer = someBrick.GetComponent<SpriteShapeRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = purple;
                    }
                }
                currBrickCnt += 9;
                Debug.Log($"SET currBrickCnt: {currBrickCnt}");
                break;
            }
            case 3:
            {
                GameObject someBrickRow = Instantiate(BrickRow, BrickRow3, Quaternion.identity);
                brickRowArray.Add(someBrickRow);
                //get child bricks to change color
                Transform brickRowTransform = someBrickRow.transform;

                for (var i = 0; i < brickRowTransform.childCount; i++)
                {
                    GameObject someBrick = brickRowTransform.GetChild(i).gameObject;
                    SpriteShapeRenderer spriteRenderer = someBrick.GetComponent<SpriteShapeRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = white;
                    }
                }
                currBrickCnt += 9;
                Debug.Log($"SET currBrickCnt: {currBrickCnt}");
                break;
            }            
            default:
            {
                Debug.Log($"Unknown value: {rowToSpawn}");
                break;
            }
        }
    }
}