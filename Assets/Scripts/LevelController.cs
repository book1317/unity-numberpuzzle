using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public enum GameState
    {
        MainMenu, Playing, Wining
    }
    public GameState currentGameState = GameState.MainMenu;
    const int GRID_WIDTH = 4;
    const int GRID_HEIGHT = 4;
    float girdLenght = 2.0f;
    Grid[,] allGrid;
    List<int> allNumber;
    List<BlockController> allBlock;
    int count = 0;
    public GameObject BlockPrefabs;
    public int moveCount;
    public Text moveCountText;

    public float TimeCount;
    public Text timeCountText;

    public Text minMoveCountText;
    public Text minTimeCountText;
    private float minTime;
    private int minMove;

    void Start()
    {
        minTime = int.MaxValue;
        minMove = int.MaxValue;
        InitBlock();
    }

    void Update()
    {
        if (currentGameState == GameState.Playing)
        {
            TimeCount += Time.fixedDeltaTime;
            UpdateTimeCountText(timeCountText);
        }
    }

    void UpdateTimeCountText(Text theText)
    {
        int minute = (int)TimeCount / 60;
        int second = (int)TimeCount % 60;
        int fraction = (int)(TimeCount * 100);
        fraction = fraction % 100;
        theText.text = "Time : " + minute.ToString("00") + ":" + second.ToString("00") + ":" + fraction.ToString("00");
    }

    void ShowAllGrid()
    {
        for (int y = 0; y < GRID_WIDTH; y++)
            for (int x = 0; x < GRID_HEIGHT; x++)
                Debug.Log(allGrid[x, y].number);
    }

    public void CheckWin()
    {
        int count = 1;
        int correctPosition = 0;
        for (int y = 0; y < GRID_WIDTH; y++)
            for (int x = 0; x < GRID_HEIGHT; x++)
            {
                if (allGrid[x, y].number == count)
                    correctPosition++;
                count++;
            }
        if (correctPosition == GRID_WIDTH * GRID_HEIGHT - 1)
            Win();
    }

    public void Win()
    {
        if (TimeCount < minTime)
        {
            minTime = TimeCount;
            UpdateTimeCountText(minTimeCountText);
        }

        if (moveCount < minMove)
        {
            minMove = moveCount;
            UpdateMoveCountText(minMoveCountText);
        }

        currentGameState = GameState.Wining;
    }

    public Grid GetGrid(int x, int y)
    {
        if (x >= 0 && x < GRID_WIDTH && y >= 0 && y < GRID_HEIGHT)
            return allGrid[x, y];
        return null;
    }

    void InitBlock()
    {
        allGrid = new Grid[GRID_WIDTH, GRID_HEIGHT];
        allBlock = new List<BlockController>();
        allNumber = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        for (int y = 0; y < GRID_WIDTH; y++)
            for (int x = 0; x < GRID_HEIGHT; x++)
            {
                if (x == (GRID_WIDTH - 1) && y == (GRID_HEIGHT - 1))
                {
                    allGrid[x, y] = new Grid(x, y, 0);
                    allGrid[x, y].number = 0;
                    continue;
                }

                int index = Random.Range(0, allNumber.Count);
                int randomNumber = allNumber[index];
                allGrid[x, y] = new Grid(x, y, randomNumber);
                allNumber.RemoveAt(index);

                BlockController theBlock = Instantiate(BlockPrefabs, new Vector3(-3 + x * girdLenght, 3 - y * girdLenght, transform.position.z), Quaternion.identity).GetComponent<BlockController>();
                theBlock.UpdateText(randomNumber);
                theBlock.x = x;
                theBlock.y = y;
                theBlock.number = randomNumber;
                allBlock.Add(theBlock);
                allGrid[x, y].number = randomNumber;
            }
    }

    void RemoveBlock()
    {
        for (int i = 0; i < allBlock.Count; i++)
            Destroy(allBlock[i].gameObject);
    }

    public void Reset()
    {
        moveCount = 0;
        UpdateMoveCountText(moveCountText);
        TimeCount = 0;
        UpdateTimeCountText(timeCountText);
        RemoveBlock();
        InitBlock();
        currentGameState = GameState.MainMenu;
    }

    public void IncreaseMoveCount()
    {
        moveCount++;
        UpdateMoveCountText(moveCountText);
    }

    void UpdateMoveCountText(Text theText)
    {
        theText.text = "Move : " + moveCount;
    }
}