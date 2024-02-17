using UnityEngine;

public class Puck : MonoBehaviour
{
    private float _speed;

    private Vector3 _direction;

    public void Initialize(float speed, Vector2 direction)
    { 
        _speed = speed;
        _direction = direction;
    }
    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        transform.position += _direction * _speed * Time.fixedDeltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PuckFinish"))
        { 
            Destroy(gameObject);
        }
    }
}
