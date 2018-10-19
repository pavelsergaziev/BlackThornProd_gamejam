using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    [SerializeField]
    private Transform _mainMenuPannel;
    [SerializeField]
    private Transform _howToPlayPannel;
    [SerializeField]
    private Transform _aboutPannel;
    [SerializeField]
    private Transform _updateWindowPannel;
    [SerializeField]
    private Transform _gamePannel;
    [SerializeField]
    private Transform _pauseMenuPannel;
    [SerializeField]
    private Transform _deadMenuPannel;
    [SerializeField]
    private Transform _textPannel;
    [SerializeField]
    private Text _textPannelText;

    [SerializeField]
    private Text _codeStrokes;
    [SerializeField]
    private Text _rockets;

    public bool InGame;

    private SoundManager _soundManager;
    private CursorController _cursorController;
    private PlayerController _playerController;
    
    
    private void Start()
    {
        _soundManager = FindObjectOfType<SoundManager>();
        _cursorController = FindObjectOfType<CursorController>();
        _playerController = FindObjectOfType<PlayerController>();

    }

    public void ShowMainMenuPannel()
    {
        _pauseMenuPannel.gameObject.SetActive(false);
        _mainMenuPannel.gameObject.SetActive(true);
        _howToPlayPannel.gameObject.SetActive(false);
        _aboutPannel.gameObject.SetActive(false);
        _updateWindowPannel.gameObject.SetActive(false);
        _gamePannel.gameObject.SetActive(false);
    }
    public void ShowHowToPlayPannel()
    {
        _mainMenuPannel.gameObject.SetActive(false);
        _howToPlayPannel.gameObject.SetActive(true);
        _aboutPannel.gameObject.SetActive(false);
        _updateWindowPannel.gameObject.SetActive(false);
    }
    public void ShowAboutPannel()
    {
        _mainMenuPannel.gameObject.SetActive(false);
        _howToPlayPannel.gameObject.SetActive(false);
        _aboutPannel.gameObject.SetActive(true);
        _updateWindowPannel.gameObject.SetActive(false);
    }
    public void ShowUpdateWIndowPannel()
    {
        _mainMenuPannel.gameObject.SetActive(false);
        _howToPlayPannel.gameObject.SetActive(false);
        _aboutPannel.gameObject.SetActive(false);
        _updateWindowPannel.gameObject.SetActive(true);
    }
    public void StartGame()
    {
        _mainMenuPannel.gameObject.SetActive(false);
        _howToPlayPannel.gameObject.SetActive(false);
        _aboutPannel.gameObject.SetActive(false);
        _updateWindowPannel.gameObject.SetActive(false);        
    }

    public void StartGameplay()
    {
        _mainMenuPannel.gameObject.SetActive(false);
        _howToPlayPannel.gameObject.SetActive(false);
        _aboutPannel.gameObject.SetActive(false);
        _updateWindowPannel.gameObject.SetActive(false);

        _codeStrokes.text = "int _codeSnippetsToFinishProj = 10";
        _gamePannel.gameObject.SetActive(true);
    }

    public void UpdateRocketsText(string value)
    {
        _rockets.text = "int _rocketsCount = " + value;
    }
    public void UpdateCodeStrokesText(string value)
    {
        _codeStrokes.text = "int _codeSnippetsToFinishProj = " + value;
    }
    public void ShowHidePouseMenu()
    {
        _pauseMenuPannel.gameObject.SetActive(!_pauseMenuPannel.gameObject.activeSelf);
    }
    //public void ResumeGame()
    //{
    //    _pauseMenuPannel.gameObject.SetActive(false);
    //}
    //public void PauseGame()
    //{
    //    _pauseMenuPannel.gameObject.SetActive(true);
    //}

    public void ShowDeadMenu()
    {
        _deadMenuPannel.gameObject.SetActive(true);
    }
    
    
}

