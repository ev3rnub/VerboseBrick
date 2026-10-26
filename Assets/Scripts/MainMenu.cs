//Course: GPE104 
//Prof: Matthew Henry 
//Student: Chad V

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuScreen : MonoBehaviour
{

    [Header("StatsManager")]
    public StatsManager someStatsManager;

    [Header("Main Menu Button Ref's")]
    public Button somePlayBtn;
    public Button someOptionsBtn;
    public Button someQuitBtn;
    public Button someCreditsButton;

    [Header("Options Screen ref")]
    public GameObject optionsGUI;
    public GameObject someCredits;

    public bool optionsGUIvisible = false;
    public bool creditsVisible = false;
    
    [Header("Options Screen HotKey")]
    public Key options = Key.O;

    [Header("Play HotKey")]
    public Key playEnter = Key.Enter;

    [Header("Quit Game Hotkey")]
    public Key quitGame = Key.Escape;

    [Header("HighScore")]
    public TextMeshProUGUI HIGH_SCORE;
    public TextMeshProUGUI HIGH_LEVEL;
    public TextMeshProUGUI HIGH_DEFEND;
    public TextMeshProUGUI HIGH_TIME;


    void Awake()
    {
        SetHighScores();
    }

    void Start()
    {
        Time.timeScale = 1f;
        // Setup button listeners
        if (somePlayBtn != null)
        {
            somePlayBtn.onClick.AddListener(OnPlayBtnPressed);
        }
            
        if (someOptionsBtn != null)
        {
            someOptionsBtn.onClick.AddListener(OnOptionsBtnPressed);
        }
            
        if (someQuitBtn != null)
        {
            someQuitBtn.onClick.AddListener(OnExitBtnPressed);
        }
        if (someCreditsButton != null)
        {
            someCreditsButton.onClick.AddListener(OnCreditsBtnPressed);
        }
    }

    void OnDestroy()
    {
        // Setup button listeners
        if (somePlayBtn != null)
        {
            somePlayBtn.onClick.RemoveListener(OnPlayBtnPressed);
        }
            
        if (someOptionsBtn != null)
        {
            someOptionsBtn.onClick.RemoveListener(OnOptionsBtnPressed);
        }
            
        if (someQuitBtn != null)
        {
            someQuitBtn.onClick.RemoveListener(OnExitBtnPressed);
        }
        if (someCreditsButton != null)
        {
            someCreditsButton.onClick.RemoveListener(OnCreditsBtnPressed);
        }        
    }

    void Update()
    {
        // Check for quit key (Escape)
        if (Keyboard.current[quitGame].wasPressedThisFrame)
        {
            QuitGame();
        }
        
        // Check for options key (O)
        if (Keyboard.current[options].wasPressedThisFrame)
        {
            OnOptionsBtnPressed();
        }

        // check if enter key is pressed key(Enter)

        if (Keyboard.current[playEnter].wasPressedThisFrame)
        {
            OnPlayBtnPressed();
        }
        SetHighScores();
    }

    public void SetHighScores()
    {
        HIGH_SCORE.text = $"HighScore: {someStatsManager.GetHighScore()}";
        HIGH_LEVEL.text = $"HighLevel: {someStatsManager.GetHighLevelCount()}";
        HIGH_TIME.text =  $"HighTime: {someStatsManager.GetHighPlayTime()}";
        HIGH_DEFEND.text = $"HighDefend: {someStatsManager.GetHighDefendCount()}";

    }

    public void OnPlayBtnPressed()
    {
        Debug.Log("Loading First Level, Good LUCK!");
        // Load scene by name (you can specify the scene name here)
        SceneManager.LoadScene("DefaultLevel"); // Replace with your actual scene name
    }

    public void OnOptionsBtnPressed()
    {
        if (optionsGUIvisible == true)
        {
            optionsGUI.SetActive(false);
            optionsGUIvisible = false;
        }
        else
        {
            optionsGUI.SetActive(true);
            optionsGUIvisible = true;
        }
    }

    public void OnExitBtnPressed()
    {
        Debug.Log("Exiting Game...");
        QuitGame();
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void OnCreditsBtnPressed()
    {
        if (creditsVisible == true)
        {
            someCredits.SetActive(false);
            creditsVisible = false;
        }
        else
        {
            someCredits.SetActive(true);
            creditsVisible = true;
        }
    }
}
