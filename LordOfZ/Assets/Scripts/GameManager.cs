using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<LevelGestion> levelGestions;
    public List<ZombieScript> playerZombies = new List<ZombieScript>();
    public bool canPlay;
    public int playerMoney = 0;
    public Camera cam;
    public LevelGestion currentLevelGestion;
    public int currentLevelNumber = 0;

    [SerializeField] private GameObject camParent;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject retryScreen;
    [SerializeField] private GameObject endScreen;

    private LevelGestion savedLevelGestion;
    private Dictionary<HumanScript, GameObject> humansInSaved;
    private float timerBeforeScreen = 0;
    private float maxTimerBeforeScreen = 3.5f;

    private Queue<string> sentences = new Queue<string>();
    private Dialogue currentDialogue;
    [SerializeField] private UI_Manager ui_Manager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("LevelNumber") == levelGestions.Count)
        {
            PlayerPrefs.DeleteKey("LevelNumber");
        }

        if (PlayerPrefs.GetInt("LevelNumber") != 0)
        {
            currentLevelNumber = PlayerPrefs.GetInt("LevelNumber");
        }
        StartLevel();
    }

    private void Update()
    {
        //Victory
        if (currentLevelGestion.humansToKill.Count <= 0)
        {
            timerBeforeScreen += Time.deltaTime;
            if (currentLevelNumber < levelGestions.Count - 1)
            {
                if (timerBeforeScreen > maxTimerBeforeScreen)
                {
                    victoryScreen.SetActive(true);
                }
            }
            else
            {
                if (timerBeforeScreen > maxTimerBeforeScreen)
                {
                    endScreen.SetActive(true);
                }
            }
        }

        //Defeat
        if (playerZombies.Count <= 0 && playerMoney <= 0)
        {
            timerBeforeScreen += Time.deltaTime;
            if (timerBeforeScreen > maxTimerBeforeScreen)
            {
                retryScreen.SetActive(true);
            }
        }
    }

    #region Buttons
    public void NextLevelButton()
    {
        victoryScreen.SetActive(false);
        currentLevelNumber++;
        timerBeforeScreen = 0;
        ZombieSpawner.Instance.selectedZombie = null;

        foreach (var item in playerZombies)
        {
            Destroy(item.gameObject);
        }
        playerZombies.Clear();

        if (currentLevelNumber < levelGestions.Count)
        {
            StartLevel();
        }
    }

    public void Quit()
    {
        PlayerPrefs.SetInt("LevelNumber", currentLevelNumber);
        Application.Quit();
    }

    //retry button
    public void Retry()
    {
        retryScreen.SetActive(false);
        timerBeforeScreen = 0;
        //Set base money amount to the player
        playerMoney = savedLevelGestion.money;
        moneyText.text = playerMoney.ToString();
        //Clear player zombies
        foreach (var zombie in playerZombies)
        {
            Destroy(zombie.gameObject);
        }
        playerZombies.Clear();

        //Reset humans to kill
        foreach (var human in currentLevelGestion.humansToKill)
        {
            human.isDying = true;
            human.gameObject.SetActive(false);
        }
        currentLevelGestion.humansToKill.Clear();

        foreach (var item in humansInSaved)
        {
            GameObject humanGO = Instantiate(item.Key.gameObject, item.Value.transform.position, item.Value.transform.rotation, currentLevelGestion.camPoint);
            humanGO.gameObject.SetActive(true);
            HumanScript humanScript = humanGO.GetComponent<HumanScript>();
            humanScript.Reset();
            currentLevelGestion.humansToKill.Add(humanGO.GetComponent<HumanScript>());
        }
    }

    #endregion

    public void StartLevel()
    {
        ui_Manager.panelShow = false;
        StartCoroutine(ui_Manager.PanelSlide(ui_Manager.shownPos.position));
        currentLevelGestion = levelGestions[currentLevelNumber];
        //Starts dialogue if there is one for this level
        if (currentLevelGestion.dialogueTrigger != null)
        {
            currentLevelGestion.dialogueTrigger.gameObject.SetActive(true);
            StartDialogue(currentLevelGestion.dialogueTrigger.dialogue);
        }
        cam.orthographicSize = levelGestions[currentLevelNumber].orthographicSize;
        playerMoney = currentLevelGestion.money;
        camParent.transform.position = currentLevelGestion.camPoint.position;
        moneyText.text = playerMoney.ToString();
        foreach (var human in currentLevelGestion.humansToKill)
        {
            human.startMoving = true;
        }

        //Store humans for retry
        savedLevelGestion = currentLevelGestion;
        humansInSaved = new Dictionary<HumanScript, GameObject>();
        foreach (var human in savedLevelGestion.humansToKill)
        {
            humansInSaved.Add(human, human.gameObject);
        }
    }

    #region Dialogues
    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        currentDialogue = dialogue;

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        currentLevelGestion.dialogueTrigger.textArea.text = sentence;
        //Debug.Log(sentence);
    }

    public void DisplayNextArrow()
    {
        currentLevelGestion.dialogueTrigger.currentArrowNumber++;
        if (currentLevelGestion.dialogueTrigger.currentArrowNumber < currentLevelGestion.dialogueTrigger.arrows.Length)
        {
            currentLevelGestion.dialogueTrigger.arrows[currentLevelGestion.dialogueTrigger.currentArrowNumber].SetActive(true);
        }
    }

    void EndDialogue()
    {
        currentLevelGestion.dialogueTrigger.gameObject.SetActive(false);
    }
    #endregion

    public void BuyUnit(int cost)
    {
        playerMoney -= cost;

        if (playerMoney <= 0)
        {
            playerMoney = 0;
        }

        moneyText.text = playerMoney.ToString();
    }
}

[Serializable]
public class LevelGestion
{
    public int levelNumber;
    public Transform camPoint;
    public int money;
    public bool startMoving = false;
    public int orthographicSize;
    public DialogueTrigger dialogueTrigger;
    public List<HumanScript> humansToKill;
}
