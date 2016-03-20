using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public const float TURN_DELAY = 0.1f;
    public const float LEVEL_START_DELAY = 2f;

    public static GameManager instance;

    [HideInInspector]
    public bool playerTurn = false;
    public int playerFoodPoints = 100;

    private int level = 1;
    private BoardManager boardScript;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private Text levelText;
    private GameObject levelImage;
    private bool doingSetup;

    private GameManager()
    {

    }

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        boardScript = GetComponent<BoardManager>();
        enemies = new List<Enemy>();
        InitGame();
    }

    private void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerTurn && !enemiesMoving && !doingSetup)
        {
            StartCoroutine(MoveEnemies());
        }
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    private void InitGame()
    {
        doingSetup = true;  
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", LEVEL_START_DELAY);    

        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(TURN_DELAY);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(TURN_DELAY);
        }

        foreach (Enemy enemy in enemies)
        {
            enemy.MoveEnemy();
            yield return new WaitForSeconds(enemy.moveTime);
        }

        playerTurn = true;
        enemiesMoving = false;
    }
}
