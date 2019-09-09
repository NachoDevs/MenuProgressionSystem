using TMPro;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    // Private variables
    private float m_currTime;           // Stores the amount of time elapsed
    private float m_startTime;          // Stores the time when the timer started

    private UIManager m_uiManager;      // Reference to the UI manager used to 
                                        //      comunicate with the UI logic

    // Public variables
    public int levelDifficulty;         // Local storage of the current minigame difficulty

    [Header("UI")]
    public TextMeshProUGUI timerText;   // Reference to the time display

    /// <summary>
    /// Used for geting and checking essential components.
    /// </summary>
    private void Awake()
    {
        m_uiManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIManager>();

        bool shouldQuit = false;

        // Checking if timer object exists
        if (timerText == null)
        {
            Debug.LogError("UIManager: Progression bar object not assigned.");
            shouldQuit = true;
        }

        // Checking if the ui manager exists
        if (m_uiManager == null)
        {
            Debug.LogError("UIManager: Button prefab not assigned.");
            shouldQuit = true;
        }

        // If we have found any error we quit the application
        if (shouldQuit)
        {
            Debug.Log("Application quiting...");
            Application.Quit();
        }
    }

    /// <summary>
    /// Used for initializing variables.
    /// </summary>
    private void Start()
    {
        ResetTimer();
    }

    /// <summary>
    /// Per frame object logic.
    /// </summary>
    private void Update()
    {
        // Update time
        m_currTime += Time.deltaTime;

        // Obtaining the time variables
        int minutes = (int) m_currTime / 60;
        int seconds = (int) m_currTime % 60;

        // Format time in a more readable way
        string timeFormat = string.Format("{0:00.}:{1:00.}", minutes, seconds);

        // Update timer display
        timerText.text = timeFormat;
    }

    /// <summary>
    /// We set the difficulty and we call the reset the timer method for the minigame.
    /// </summary>
    /// <param name="t_difficulty"> This will determine how penalizing is the scoring algorithm</param>
    public void StartMinigame(int t_difficulty)
    {
        levelDifficulty = t_difficulty;

        ResetTimer();
    }

    /// <summary>
    /// We reset the variables involved in the timer.
    /// </summary>
    public void ResetTimer()
    {
        m_startTime = Time.time;
        m_currTime  = 0;
    }

    /// <summary>
    /// Functionality of the Correct button from the minigame.
    /// We evaluate the score and reset the amount of tries.
    /// </summary>
    public void CorrectButton()
    {
        int currentLevel = m_uiManager.selectedLevel;

        // Once the pass the minigame, we evaluate the obtained score
        m_uiManager.EvaluateScore(levelDifficulty, m_currTime - m_startTime);

        // If we pass this level, the amount of tries gets reseted for a future try
        m_uiManager.m_amountOfTries[currentLevel] = 0;
    }

    /// <summary>
    /// Functionality of the Wrong button from the minigame.
    /// We increment the amount of tries for this minigame.
    /// </summary>
    public void WrongButton()
    {
        int currentLevel = m_uiManager.selectedLevel;

        // If we don't pass this level, the amount of tries gets incremented
        ++m_uiManager.m_amountOfTries[currentLevel];
    }
}

