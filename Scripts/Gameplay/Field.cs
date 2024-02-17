using DG.Tweening;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Field : MonoBehaviour, ISubscriber
{
    public static Field Instance;

    public GameObject[,] Grid = new GameObject[3, 3];
    public Transform[,] Positions = new Transform[3, 3];

    private void OnDisable()
    {
        UnsubscribeAll();   
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        for (int row = 0; row < Positions.GetLength(0); row++)
        {
            for (int col = 0; col < Positions.GetLength(1); col++)
            {
                int childIndex = row * Positions.GetLength(1) + col;
                if (childIndex < transform.childCount)
                    Positions[row, col] = transform.GetChild(childIndex);
                else
                    Positions[row, col] = null;
            }
        }
    }
    public void SubscribeAll()
    {
        GameState.Instance.GameStarted += SetStickNewPosition;
        GameState.Instance.GameStarted += SetGoalNewPosition;
    }
    public void UnsubscribeAll()
    {
        GameState.Instance.GameStarted -= SetStickNewPosition;
        GameState.Instance.GameStarted -= SetGoalNewPosition;
    }
    public void SetStickNewPosition(int row, int col, int rowNew, int colNew)
    {
        GameObject old = Grid[row, col];

        Grid[rowNew, colNew] = old;
        Grid[row, col] = null;
    }
    private void SetGoalNewPosition()
    {
        int row = 1;
        while (row == 1)
            row = Random.Range(0, 3);
        int col = 1;
        while (col == 1)
            col = Random.Range(0, 3);

        var goal = FindObjectOfType<Goal>();

        goal.Row = row; 
        goal.Col = col;

        Grid[row, col] = goal.gameObject;

        goal.SpawnMe();
        goal.transform.DOMove(Positions[row, col].position, 0.15f).SetLink(goal.gameObject);
    }
    public void SetGoalPosition()
    {
        var stick = FindObjectOfType<Stick>();
        int row = 1;

        while (row == stick.GetRow())
            row = Random.Range(0, 3);
        int col = 1;
        while (col == stick.GetCol())
            col = Random.Range(0, 3);

        var goal = FindObjectOfType<Goal>();

        goal.Row = row;
        goal.Col = col;

        Grid[row, col] = goal.gameObject;
        goal.transform.DOMove(Positions[row, col].position, 0.01f).SetLink(goal.gameObject).SetDelay(0.2f);
    }
    public void RemoveElement(int row, int col)
    {
        Grid[row, col] = null;
    }
    public void SetStickNewPosition()
    {
        Grid[1, 1] = FindObjectOfType<Stick>().gameObject;
    }
    public bool CanIGoUp(int row, int col)
    {
        if (row == 0)
            return false;

        if (Grid[row - 1, col] == null)
            return true;
        else
            return true;
    }
    public bool CanIGoDown(int row, int col)
    {
        if (row == Grid.GetLength(0) - 1)
            return false;

        if (Grid[row + 1, col] == null)
            return true;
        else
            return true;
    }
    public bool CanIGoRight(int row, int col)
    {
        if (col == Grid.GetLength(1) - 1)
            return false;

        if (Grid[row, col + 1] == null)
            return true;
        else
            return true;
    }
    public bool CanIGoLeft(int row, int col)
    {
        if (col == 0)
            return false;

        if (Grid[row, col - 1] == null)
            return true;
        else
            return true;
    }
}
