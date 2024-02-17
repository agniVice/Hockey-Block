using DG.Tweening;
using UnityEngine;

public class Stick : MonoBehaviour, ISubscriber
{
    private int row = 1;
    private int col = 1;

    private void Start()
    {
        GoToNewPosition();
    }
    private void OnDisable()
    {
        UnsubscribeAll();
    }
    public void SubscribeAll()
    {
        SwipeDetector.Instance.SwipedUp += TryToGoUp;
        SwipeDetector.Instance.SwipedDown += TryToGoDown;
        SwipeDetector.Instance.SwipedLeft += TryToGoLeft;
        SwipeDetector.Instance.SwipedRight += TryToGoRight;
    }
    public void UnsubscribeAll()
    {
        SwipeDetector.Instance.SwipedUp -= TryToGoUp;
        SwipeDetector.Instance.SwipedDown -= TryToGoDown;
        SwipeDetector.Instance.SwipedLeft -= TryToGoLeft;
        SwipeDetector.Instance.SwipedRight -= TryToGoRight;
    }
    private void TryToGoUp()
    {
        if (!Field.Instance.CanIGoUp(row, col))
            return;

        Field.Instance.SetStickNewPosition(row, col, row - 1, col);
        row -= 1;
        GoToNewPosition();
    }
    private void TryToGoDown()
    {
        if (!Field.Instance.CanIGoDown(row, col))
            return;

        Field.Instance.SetStickNewPosition(row, col, row + 1, col);
        row += 1;
        GoToNewPosition();
    }
    private void TryToGoLeft()
    {
        if (!Field.Instance.CanIGoLeft(row, col))
            return;

        Field.Instance.SetStickNewPosition(row, col, row, col - 1);
        col -= 1;
        GoToNewPosition();
    }
    private void TryToGoRight()
    {
        if (!Field.Instance.CanIGoRight(row, col))
            return;

        Field.Instance.SetStickNewPosition(row, col, row, col + 1);
        col += 1;
        GoToNewPosition();
    }
    private void GoToNewPosition()
    {
        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Swap, 1f);
        transform.DOMove(Field.Instance.Positions[row, col].position, 0.15f).SetLink(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        if (collision.gameObject.CompareTag("Goal"))
        {
            AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.ScoreAdd, 1f);
            PlayerScore.Instance.AddScore();

            collision.gameObject.GetComponent<Goal>().DestroyMe();
        }
        if (collision.gameObject.CompareTag("Puck"))
        {
            AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Win, 1f);
            GameState.Instance.FinishGame();
        }
    }
    public int GetRow()
    {
        return row;
    }
    public int GetCol()
    {
        return col;
    }
}
