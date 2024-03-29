﻿using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class FinishWindow : MonoBehaviour, IInitializable, ISubscriber
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private List<Transform> _transforms = new List<Transform>();

    private bool _isInitialized;

    private void OnEnable()
    {
        if (!_isInitialized)
            return;

        SubscribeAll();
    }
    private void OnDisable()
    {
        UnsubscribeAll();
    }
    public void Initialize()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;

        Hide();

        _isInitialized = true;
    }
    public void SubscribeAll()
    {
        GameState.Instance.GameFinished += Show;
    }
    public void UnsubscribeAll()
    {
        GameState.Instance.GameFinished -= Show;
    }
    private void Show()
    {
        _panel.SetActive(true);

        foreach (Transform transform in _transforms)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, Random.Range(0.2f, 0.7f)).SetEase(Ease.OutBack).SetLink(transform.gameObject).SetUpdate(true);
        }

        _panel.GetComponent<CanvasGroup>().alpha = 0f;
        _panel.GetComponent<CanvasGroup>().DOFade(1f, 0.6f).SetLink(_panel.gameObject).SetUpdate(true);
    }
    private void Hide()
    {
        _panel.SetActive(false);
    }
    public void OnRestartButtonClicked()
    {
        Time.timeScale = 1.0f;
        SceneLoader.Instance.LoadScene("Gameplay");
    }
    public void OnMenuButtonClicked()
    {
        Time.timeScale = 1.0f;
        SceneLoader.Instance.LoadScene("Menu");
    }
}