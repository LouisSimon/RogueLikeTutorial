using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Assets.Scripts;

public class BoardManager : MonoBehaviour
{

    private const int ROWS = 8;
    private const int COLUMNS = 8;

    public GameObject Exit;
    public GameObject[] FloorTiles;
    public GameObject[] WallTiles;
    public GameObject[] FoodTiles;
    public GameObject[] EnemyTiles;
    public GameObject[] OuterWallTiles;

    private Count wallCount = new Count(5, 9);
    private Count foodCount = new Count(1, 5);
    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    private void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < COLUMNS - 1; x++)
        {
            for (int y = 1; y < ROWS - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    private void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < COLUMNS + 1; x++)
        {
            for (int y = -1; y < ROWS + 1; y++)
            {
                GameObject instance = (x == -1 || x == COLUMNS || y == -1 || y == ROWS) ? getRandomCell<GameObject>(OuterWallTiles) : getRandomCell<GameObject>(FloorTiles);
                instance = Instantiate(instance, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    private Vector3 RandomPosition()
    {
        Vector3 randomPosition = getRandomCell<Vector3>(gridPositions);
        gridPositions.Remove(randomPosition);
        return randomPosition;
    }

    private void LayoutObjectAtRandom(GameObject[] tileArray, Count limits)
    {
        int objectCount = Random.Range(limits.Minimum, limits.Maximum + 1);
        for (int i = 0; i < objectCount; i++)
        {
            Instantiate(getRandomCell<GameObject>(tileArray), RandomPosition(), Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        int enemyAmount = (int)Mathf.Log(level, 2f);
        Count enemyCount = new Count(enemyAmount, enemyAmount);
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(WallTiles, wallCount);
        LayoutObjectAtRandom(FoodTiles, foodCount);
        LayoutObjectAtRandom(EnemyTiles, enemyCount);
        Instantiate(Exit, new Vector3(COLUMNS - 1, ROWS - 1, 0f), Quaternion.identity);
    }

    private T getRandomCell<T>(IList<T> array)
    {
        return array[Random.Range(0, array.Count)];
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
