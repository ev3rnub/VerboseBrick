//Course: GPE104 
//Prof: Matthew Henry 
//Student: Chad V

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic; // for List<>
using TMPro;
using System;

public class GameManager : MonoBehaviour
{   
    [Header("GameObjects")]
    public static GameManager someGameManager;
    public ControllerBall controllerBall; // get controller ref for player entity.//


    void Start()
    {
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

    public void BrickHit()
    {
        Debug.Log("BOOM");
    }

    public void BottomHit()
    {
        Debug.Log("BOTTOM");
    }
}