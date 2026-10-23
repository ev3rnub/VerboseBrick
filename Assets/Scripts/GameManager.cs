//Course: GPE104 
//Prof: Matthew Henry 
//Student: Chad V

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
    public PaddlePawn somePaddlePawn;
    public ControllerBall controllerBall; // get controller ref for player entity.//
    public Health targetHealth;
    public GameObject EnemyBlock;
    public GameObject BrickRow;
    
    [Header("Spawn Slot locations")]
    public Vector3 spawnSlot0 = new Vector3(-6.38f, 2.08f, 0f); // far left
    public Vector3 spawnSlot1 = new Vector3(-2.63f, 2.08f, 0f);
    public Vector3 spawnSlot2 = new Vector3(-0.2449999f, 2.08f, 0f);
    public Vector3 spawnSlot3 = new Vector3(4.12f, 2.08f, 0f);
    public Vector3 spawnSlot4 = new Vector3(7.495f, 2.08f, 0f); // far right
    private List<Vector3> slotArray = new List<Vector3>();

    public Vector3 BrickRow0 = new Vector3(-9.25500011f, 4.94999981f, 0f);
    public Vector3 BrickRow1 = new Vector3(-9.25500011f, 4.32499981f, 0f);
    public Vector3 BrickRow2 = new Vector3(-9.25500011f, 3.69999981f, 0f);

    
    [Header("GUI")]
    public TextMeshProUGUI someWave;
    public TextMeshProUGUI someLabelScore;
    public int someScore = 0;
    public int shotsFired = 0;
    public int defends = 0;
    public int failures = 0;

    private int currentLevel = 0;
    private int currentRowLevel = 2;
    private int bricksPerRow = 9;
    private int currBrickCnt = 0;


    [Header("Block Colors")]
    public Color white = new Color(1f, 1f, 1f, 1f);
    public Color red = new Color(1f, 0f, 0f, 1f);
    public Color blue = new Color(0f, 0f, 1f, 1f);
    public Color purple = new Color(0.5f, 0f, 0.5f, 1f);
    [Header("Block Spawn Offset")]
    public Vector3 someOffset = new Vector3(2f, 0f, 0f);


    void Start()
    {
        currentLevel = 1;
        // fill slotArray with vector3 spawn locations for EnemyBlock
        slotArray.Add(spawnSlot0);
        slotArray.Add(spawnSlot1);
        slotArray.Add(spawnSlot2);
        slotArray.Add(spawnSlot3);
        slotArray.Add(spawnSlot4);
        SpawnRow(0);
        SpawnEnemy(1, EnemyBlock);
    }

    public void Awake()
    {

        if (someGameManager == null)
        {
            someGameManager = this;
            DontDestroyOnLoad(gameObject);
            Time.timeScale = 1f;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SomeGameObjectDied(GameObject someGameObject)
    {

        if (someGameObject.CompareTag("EnemyPaddle"))
        {
            UpdateScore(100);
        }

        if (someGameObject.CompareTag("Brick"))
        {
            currBrickCnt-= 1;
            UpdateScore(50 * currentLevel);
            Debug.Log($"BRICKCNT: {currBrickCnt}");
            if (currBrickCnt <= 0)
            {
                currBrickCnt = 0;
                currentLevel += 1;
                if (currentLevel == 1)
                {
                    SpawnRow(0);
                    SpawnEnemy(1, EnemyBlock);
                }
                else if (currentLevel == 2)
                {
                    SpawnRow(0);
                    SpawnRow(1);
                    SpawnEnemy(1, EnemyBlock);
                }
                else
                {
                    SpawnRow(0);
                    SpawnRow(1);
                    SpawnRow(2);
                    SpawnEnemy(1, EnemyBlock);
                    SpawnEnemy(1, EnemyBlock);
                }
            }
        }

    }

    public void UpdateScore(int someAmt)
    {
        someScore += someAmt;
        someLabelScore.text = $"Score: {someScore}";
    }

    public void BulletHit()
    {
        //some fx
        //some sfx
        //somelogic

        Debug.Log("THUMP");
        var someInt = targetHealth.SubHealth(5);
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
        Debug.Log("BOTTOM");
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
                Debug.Log($"SET currBrickCnt: {currBrickCnt}");
                break;
            }
            case 1:
            {
                GameObject someBrickRow = Instantiate(BrickRow, BrickRow1, Quaternion.identity);
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
            default:
            {
                Debug.Log($"Unknown value: {rowToSpawn}");
                break;
            }
        }
        
    }    
}