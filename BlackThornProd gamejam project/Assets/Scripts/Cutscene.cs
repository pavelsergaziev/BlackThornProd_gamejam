using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Cutscene : MonoBehaviour {

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float[] _animationTimeStamps;

    [SerializeField]
    private Text _textOnScreen;

    [SerializeField]
    private string[] _textPieces;

    [SerializeField]
    private float[] _textTimeStamps;

    private float _timer;
    private int _nextAnimationStateIndex;
    private int _nextTextPieceIndex;

    public void StartCutscene()
    {

        StartCoroutine(TimeLoop());
    }

    //вариант с корутиной
    private IEnumerator TimeLoop()
    {

        SwitchVisibility();

        while (_nextAnimationStateIndex < _animationTimeStamps.Length - 1)
        {      

            //анимация
            if (_timer > _animationTimeStamps[_nextAnimationStateIndex])
                _animator.SetInteger("switchStateTo", ++_nextAnimationStateIndex);

            //текст
            if (_timer > _textTimeStamps[_nextTextPieceIndex])
                _textOnScreen.text = _textPieces[_nextTextPieceIndex++];            

            yield return new WaitForEndOfFrame();
            _timer += Time.deltaTime;

        }

        SwitchVisibility();

    }

    private void SwitchVisibility()
    {
        _textOnScreen.enabled = !_textOnScreen.enabled;

        foreach (Transform child in transform)
            child.gameObject.SetActive(!child.gameObject.activeSelf);
    }


}
