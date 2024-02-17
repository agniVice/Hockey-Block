using DG.Tweening;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int Row;
    public int Col;

    public void SpawnMe()
    {
        transform.DOScale(1, 0.2f).SetLink(gameObject).SetDelay(0.2f).OnKill(() => { GetComponent<Collider2D>().enabled = true; });
    }
    public void DestroyMe()
    {
        Field.Instance.RemoveElement(Row, Col);
        Field.Instance.SetGoalPosition();

        transform.DOScale(0, 0.15f).SetLink(gameObject);

        GetComponent<Collider2D>().enabled = false;

        SpawnMe();
    }
}
