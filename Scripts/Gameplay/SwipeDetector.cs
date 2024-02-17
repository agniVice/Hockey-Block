using UnityEngine;
using System;

public class SwipeDetector : MonoBehaviour, ISubscriber
{
    public static SwipeDetector Instance;

    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    [SerializeField]
    private bool detectSwipeOnlyAfterRelease = true;
    [SerializeField]
    private float minDistanceForSwipe = 20f;

    public event Action SwipedUp;
    public event Action SwipedDown;
    public event Action SwipedRight;
    public event Action SwipedLeft;

    private bool _isActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public void SubscribeAll()
    {
        PlayerInput.Instance.PlayerMouseDown += OnPlayerMouseDown;
        PlayerInput.Instance.PlayerMouseUp += OnPlayerMouseUp;
    }
    public void UnsubscribeAll()
    {
        PlayerInput.Instance.PlayerMouseDown -= OnPlayerMouseDown;
        PlayerInput.Instance.PlayerMouseUp -= OnPlayerMouseUp;
    }
    private void OnDisable()
    {
        UnsubscribeAll();
    }
    private void Update()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        if (!_isActive)
            return;

        if (!detectSwipeOnlyAfterRelease && Input.GetMouseButton(0))
        {
            fingerUpPosition = Input.mousePosition;
            DetectSwipe();
        }
    }
    private void OnPlayerMouseDown()
    {
        _isActive = true;

        fingerDownPosition = Input.mousePosition;
        fingerUpPosition = Input.mousePosition;
    }
    private void OnPlayerMouseUp()
    {
        _isActive = false;
        fingerUpPosition = Input.mousePosition;
        DetectSwipe();
    }
    private void DetectSwipe()
    {
        if (Vector2.Distance(fingerDownPosition, fingerUpPosition) >= minDistanceForSwipe)
        {
            Vector2 swipeDirection = fingerUpPosition - fingerDownPosition;

            if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
            {
                if (swipeDirection.x > 0)
                    SwipedRight?.Invoke();
                else
                    SwipedLeft?.Invoke();
            }
            else
            {
                if (swipeDirection.y > 0)
                    SwipedUp?.Invoke();
                else
                    SwipedDown?.Invoke();
            }
        }
    }
}