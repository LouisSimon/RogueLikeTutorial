using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private const int LEVEL = 3;
    public static GameManager instance;

    private BoardManager boardScript;

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
        InitGame();
    }

    private void InitGame()
    {
        boardScript.SetupScene(LEVEL);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
