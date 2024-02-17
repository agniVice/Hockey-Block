using UnityEngine;

public class PuckSpawner : MonoBehaviour
{
    public static PuckSpawner Instance;

    [SerializeField] private Transform[] _spawnPostions;
    [SerializeField] private Vector2[] _directions;

    [SerializeField] private GameObject _puckPrefab;
    [SerializeField] private float _spawnRate;

    private float _currentTime;

    private int _lastPos;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else Instance = this;
    }
    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;
        if (_currentTime < _spawnRate)
        {
            _currentTime += Time.fixedDeltaTime;
        }
        else
        {
            _currentTime = 0f;

            _spawnRate = Random.Range(0.8f, 1.8f);

            SpawnPuck();
        }
    }
    public void SpawnPuck()
    {
        int pos = GetRandomPos();
        if (pos == _lastPos)
        {
            SpawnPuck();
            return;
        }

        _lastPos = pos;

        Vector2 spawnPosition = _spawnPostions[pos].position;
        Vector2 direction = _directions[pos];

        var puck = Instantiate(_puckPrefab, spawnPosition, Quaternion.identity);
        puck.GetComponent<Puck>().Initialize(2f, _directions[pos]);
    }
    private int GetRandomPos()
    {
        return Random.Range(0, _spawnPostions.Length);
    }
}
