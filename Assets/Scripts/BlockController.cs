using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockController : MonoBehaviour
{
    public int x, y;
    private Vector3 firtTouchPosition;
    private Vector3 finalTouchPoition;
    private float swipeAngle;
    private bool canMove;
    private bool canLerp;

    private Vector3 startPosition;
    private Vector3 endPosition;

    LerpController theLerp;
    public float moveLenght = 2.0f;

    public int number = 0;
    public Text numberText;

    LevelController theLevel;
    void Start()
    {
        theLevel = FindObjectOfType<LevelController>();
        theLerp = GetComponent<LerpController>();
    }

    void Update()
    {
        if (canLerp)
            canLerp = theLerp.LerpObject(startPosition, endPosition, 0.4f);
    }

    void OnMouseDown()
    {
        firtTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        if (theLevel.currentGameState == LevelController.GameState.Playing || theLevel.currentGameState == LevelController.GameState.MainMenu)
        {
            finalTouchPoition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
            float swipeRangeX = Mathf.Abs(firtTouchPosition.x - finalTouchPoition.x);
            float swipeRangeY = Mathf.Abs(firtTouchPosition.y - finalTouchPoition.y);
            if (swipeRangeX > 0.2f || swipeRangeY > 0.2f)
                MoveBlock();
        }
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPoition.y - firtTouchPosition.y, finalTouchPoition.x - firtTouchPosition.x) * 180 / Mathf.PI;
    }

    void MoveBlock()
    {
        int lastX = x;
        int lastY = y;
        if (swipeAngle > -45 && swipeAngle <= 45 && theLevel.GetGrid(x + 1, y) != null && theLevel.GetGrid(x + 1, y).number == 0)  //Right
        {
            x++;
            startPosition = new Vector2(transform.position.x, transform.position.y); ;
            endPosition = new Vector2(transform.position.x + moveLenght, transform.position.y); ;
            canLerp = true;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && theLevel.GetGrid(x, y - 1) != null && theLevel.GetGrid(x, y - 1).number == 0) //Up
        {
            y--;
            startPosition = new Vector2(transform.position.x, transform.position.y); ;
            endPosition = new Vector2(transform.position.x, transform.position.y + moveLenght); ;
            canLerp = true;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && theLevel.GetGrid(x - 1, y) != null && theLevel.GetGrid(x - 1, y).number == 0) //Left
        {
            x--;
            startPosition = new Vector2(transform.position.x, transform.position.y); ;
            endPosition = new Vector2(transform.position.x - moveLenght, transform.position.y); ;
            canLerp = true;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && theLevel.GetGrid(x, y + 1) != null && theLevel.GetGrid(x, y + 1).number == 0) //Down
        {
            y++;
            startPosition = new Vector2(transform.position.x, transform.position.y); ;
            endPosition = new Vector2(transform.position.x, transform.position.y - moveLenght); ;
            canLerp = true;
        }

        if (canLerp)
        {
            if (theLevel.currentGameState == LevelController.GameState.MainMenu)
                theLevel.currentGameState = LevelController.GameState.Playing;
            theLevel.IncreaseMoveCount();
            SetGrid(lastX, lastY, 0);
            SetGrid(x, y, number);
            theLevel.CheckWin();
        }
    }

    public void UpdateText(int number)
    {
        numberText.text = number + "";
    }

    void SetGrid(int x, int y, int number)
    {
        theLevel.GetGrid(x, y).number = number;
    }
}
