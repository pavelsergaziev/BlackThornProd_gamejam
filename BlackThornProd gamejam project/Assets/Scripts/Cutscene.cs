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

    [SerializeField]
    private float _transitionStartTimeStamp;

    [SerializeField]
    private SpriteRenderer _transitionSprite;

    [SerializeField]
    private float _transitionMidTimeStamp;

    [SerializeField]
    private float _transitionEndTimeStamp;

    [SerializeField]
    private float _transitionColorChangeSpeed;

    private float _timer;
    private int _nextAnimationStateIndex;
    private int _nextTextPieceIndex;
    private GameManager _gameManager;

    private bool _isTransitioning;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

    }

    public void StartCutscene()
    {

        StartCoroutine(TimeLoop());
    }


    //private IEnumerator TimeLoop()
    //{

    //    SwitchVisibility();
    //    _transitionSprite.color = new Color(1, 1, 1, 0);
    //    Color transitionSpriteInitialColor = _transitionSprite.color;
    //    float lerpTimer = 0;

    //    Debug.Log(transitionSpriteInitialColor);

    //    while (_timer < _transitionMidTimeStamp)
    //    {

    //        if (!_isTransitioning)
    //        {
    //            //анимация

    //            if (_timer > _animationTimeStamps[_nextAnimationStateIndex])
    //                _animator.SetInteger("switchStateTo", ++_nextAnimationStateIndex);

    //            //текст
    //            if (_timer > _textTimeStamps[_nextTextPieceIndex])
    //                _textOnScreen.text = _textPieces[_nextTextPieceIndex++];

    //            //переход к геймплею
    //            if (_timer > _transitionStartTimeStamp)
    //                _isTransitioning = true;
    //        }
    //        else
    //        {                
    //            lerpTimer += Time.deltaTime * _transitionColorChangeSpeed;
    //            _transitionSprite.color = Color.Lerp(transitionSpriteInitialColor, Color.white, lerpTimer);
    //            Debug.Log("lerptimer = " + lerpTimer);
    //            Debug.Log("цвет меняется - " + _transitionSprite.color);
    //        }

            
    //        yield return new WaitForEndOfFrame();
    //        _timer += Time.deltaTime;

    //    }

    //    SwitchVisibility();
    //    lerpTimer = 0;
    //    _gameManager.StartGameplay();

    //    while (_timer < _transitionEndTimeStamp)
    //    {
    //        Debug.Log("цвет меняется обратно - " + _transitionSprite.color);
    //        Debug.Log("lerptimer = " + lerpTimer);

    //        lerpTimer += Time.deltaTime * _transitionColorChangeSpeed;

    //        _transitionSprite.color = Color.Lerp(Color.white, transitionSpriteInitialColor, lerpTimer);

    //        yield return new WaitForEndOfFrame();
    //        _timer += Time.deltaTime;
    //    }

    //}

    private IEnumerator TimeLoop()
    {

        SwitchVisibility();        

        while (_timer < _transitionMidTimeStamp)
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
        _gameManager.StartGameplay();

    }

    private void SwitchVisibility()
    {
        _textOnScreen.transform.parent.gameObject.SetActive(!_textOnScreen.transform.parent.gameObject.activeSelf);

        foreach (Transform child in transform)
            child.gameObject.SetActive(!child.gameObject.activeSelf);
    }

}
