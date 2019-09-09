using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    // Private variables
    private bool isUnlocked = false;            // To check which sprite should be using and to allow interaction

    private static int idCount = 0;             // Level identifier counter

    private string m_levelBaseName = "Nivel";   // Default level name lable

    private Button m_buttonComponent;           // Button component of the object

    private static Sprite m_lockedSprite;       // Sprite used when the button is not interactable
    private static Sprite m_unlockedSprite;     // Sprite used when the button is interactable

    private UIManager m_uiManager;              // Reference to the UI manager used to 
                                                //      comunicate with the UI logic

    private TextMeshProUGUI m_textComponent;    // Text component of a child of the button

    // Public variables
    public int levelID = 0;                     // Level identifier

    public Transform assignedMinigame;          // In this case we will have a common minigame (here is just 
                                                //      a panel) so we are going to store it on the UIManager
                                                //      but normally every level should have its own

    /// <summary>
    /// Used for geting and checking essential components.
    /// </summary>
    private void Awake()
    {
        m_buttonComponent   = GetComponent<Button>();

        m_textComponent     = GetComponentInChildren<TextMeshProUGUI>();

        m_uiManager         = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIManager>();

        // If it hasn't been loaded by any other LevelButton we load the sprite
        if (m_lockedSprite == null)
        {
            m_lockedSprite = LoadSprite("lockedLevel");
        }

        // If it hasn't been loaded by any other LevelButton we load the sprite
        if (m_unlockedSprite == null)
        {
            m_unlockedSprite = LoadSprite("unlockedLevel");
        }

        // Checking if the ui manager exists
        if (m_uiManager == null)
        {
            Debug.LogError("UIManager: Button prefab not assigned.");
            Debug.Log("Application quiting...");
            Application.Quit();
        }
    }

    /// <summary>
    /// Used for initializing variables.
    /// </summary>
    private void Start()
    {
        levelID = idCount++;

        // Assign level name based on its level number
        m_textComponent.text = m_levelBaseName + (levelID + 1).ToString();

        // If we are the first level we are unlocked
        isUnlocked = (levelID < 1);

        // Assign the locked sprite by default unless is the first level
        m_buttonComponent.image.sprite = (isUnlocked) ?  m_unlockedSprite : m_lockedSprite;

        m_buttonComponent.interactable = isUnlocked;

        assignedMinigame = m_uiManager.minigamePanel;
    }

    /// <summary>
    /// Method called by the level buttons.
    /// Sets the current level and starts the minigame.
    /// </summary>
    public void ChooseLevel()
    {
        m_uiManager.selectedLevel = levelID;

        StartSelectedLevel();
    }

    /// <summary>
    /// Sets the locked variable.
    /// </summary>
    /// <param name="t_unlocked"> The new value for the local isUnlocked variable</param>
    public void ToggleLocked(bool t_unlocked)
    {
        // Toggle between unlocked and locked
        isUnlocked = t_unlocked;

        // If it is not unlocked, we don't want it to be interactable
        m_buttonComponent.interactable = isUnlocked;

        // Assign the correct sprite based on the button's state
        m_buttonComponent.image.sprite = (isUnlocked) ? m_unlockedSprite : m_lockedSprite;
    }

    /// <summary>
    /// Activates the panel for the minigames.
    /// And calls the start of the minigame.
    /// </summary>
    private void StartSelectedLevel()
    {
        assignedMinigame.gameObject.SetActive(true);

        m_uiManager.minigame.StartMinigame(levelID);
    }

    /// <summary>
    /// Looks for the specified sprite in the resources folder.
    /// </summary>
    /// <param name="t_spriteName"> Sprite file name</param>
    private Sprite LoadSprite(string t_spriteName)
    {
        // Adapting the path to the structure of the Resources folder
        string spritePath = "Sprites/spr_" + t_spriteName;

        // Loading the resource
        Sprite spr = Resources.Load<Sprite>(spritePath);

        if(spr == null)
        {
            Debug.LogError("LevelButton_" + levelID + ": Couldn't load the " + t_spriteName + " sprite");
            Debug.Log("Application quiting...");
            Application.Quit();
        }

        return spr;
    }
}
