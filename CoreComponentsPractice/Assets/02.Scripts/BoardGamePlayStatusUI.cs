using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class BoardGamePlayStatusUI : MonoBehaviour
{ 
    [SerializeField] GameObject[] _numberImages;
    [SerializeField] float _rollingAnimationDuration;
    private int _currentImageIndex;


    public void PlayRollingAnimation(int value, Action<int> onFinished)
    {
        StartCoroutine(C_RollingAnimation(value, onFinished));
    }

    IEnumerator C_RollingAnimation(int value, Action<int> onFinished)
    {
        int count = 0;
        float elapsedTime = 0.0f;

        while (elapsedTime < _rollingAnimationDuration)
        {
            _numberImages[_currentImageIndex].SetActive(false);
            _currentImageIndex = Random.Range(0, _numberImages.Length);
            _numberImages[_currentImageIndex].SetActive(true);
            count++;
            elapsedTime += Time.deltaTime * count;
            yield return new WaitForSeconds(Time.deltaTime * count);
        }

        _numberImages[_currentImageIndex].SetActive(false);
        _currentImageIndex = value - 1;
        _numberImages[_currentImageIndex].SetActive(true);

        onFinished?.Invoke(value);
    }
}