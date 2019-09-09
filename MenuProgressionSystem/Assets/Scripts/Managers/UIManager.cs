using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Private variables
    private List<LevelButton> m_levels;     // Used to store all the level butons (for unlocking purposes)

    // Public variables
    public int numberOfLevels;              // Amount of levels to instantiate
    public int selectedLevel;               // ID of the level that is being played

    public int[] m_amountOfTries;           // Stores the amount of tries for each level

    [Header("Prefabs")]
    public GameObject buttonLevelPrefab;    // Prefab of the button to instantiate

    [Header("UI")]
    public Transform levelPanel;            // Level button parent
    public Slider progressionBar;           // Level progression bar 

    [Header("Minigame-Testing")]
    public Transform minigamePanel;         // Instead of a proper minigame scene we 
                                            //      just show a panel
    public Minigame minigame;               // This is for testing puposes

    /// <summary>
    /// Used for geting and checking essential components.
    /// </summary>
    private void Awake()
    {
        bool shouldQuit = false;
        // Checking if the parent of the levels exists
        if (progressionBar == null)
        {
            Debug.LogError("UIManager: Progression bar object not assigned.");
            shouldQuit = true;
        }

        // Checking if the parent of the levels exists
        if (levelPanel == null)
        {
            Debug.LogError("UIManager: Level parent not assigned.");
            shouldQuit = true;
        }

        // Checking if the parent of the levels exists
        if (buttonLevelPrefab == null)
        {
            Debug.LogError("UIManager: Button prefab not assigned.");
            shouldQuit = true;
        }

        // Checking if the number of the levels is bigger than zero
        if (numberOfLevels < 1)
        {
            Debug.LogError("UIManager: The number of levels should be bigger than 0.");
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
        m_levels        = new List<LevelButton>();

        m_amountOfTries = new int[numberOfLevels];

        progressionBar.maxValue = numberOfLevels - 1;

        // Instantiating the buttons
        for (int i = 0; i < numberOfLevels; ++i)
        {
            m_levels.Add(Instantiate(buttonLevelPrefab, levelPanel).GetComponent<LevelButton>());
        }
    }

    /// <summary>
    /// Algorithm that gives a score based on the difficulty and the time spent on the minigame.
    /// </summary>
    /// <param name="t_levelDifficulty"> Determines the current level difficulty and how penalizing is the algorithm</param>
    /// <param name="t_timeSpent"> Determines the time spent solving the minigame</param>
    public void EvaluateScore(int t_levelDifficulty, float t_timeSpent)
    {
        float score = 10;

        float difficultyModifier = (float) t_levelDifficulty / 10;

        // The amount of tries penalize the score
        score -= (float)(m_amountOfTries[selectedLevel]) * 5 * difficultyModifier;

        // The more ammount of time spent the worst the score recived
        score -= (t_timeSpent % 10) * .5f;  // Every 10 seconds we lose half a point

        // If we have passed the test we can unlock the next level
        if(score > 5)
        {
            // If we scored more than a 9 we can unlock two levels since 
            //      the las level was very easy for the student
            UnlockNextLevels((score > 9) ? 2 : 1);
        }

        print(score);
    }

    /// <summary>
    /// Unlocks the next levels based on how good was the scored obtained.
    /// </summary>
    /// <param name="t_numOfLevelsToUnlock"> Amount of levels to unlock</param>
    private void UnlockNextLevels(int t_numOfLevelsToUnlock)
    {
        // Unlock the corresponding levels
        for (int i = 0; i < t_numOfLevelsToUnlock + 1; ++i)
        {
            m_levels[selectedLevel + i].ToggleLocked(true);

            // If we are advancing two levels but there is only one remaining this
            //      will stop from looking into an unexisting element of the list
            if(selectedLevel + i + 2 > m_levels.Count)
            {
                t_numOfLevelsToUnlock -= 1;
                break;
            }
        }

        // Update the level slider
        progressionBar.value = selectedLevel + t_numOfLevelsToUnlock;
    }
}
