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
    private Text _codeStrokes;
    [SerializeField]
    private Text _rockets;



    private void Start()
    {
        //ShowMainMenuPannel();
    }
    public void ShowMainMenuPannel()
    {
        _mainMenuPannel.gameObject.SetActive(true);
        _howToPlayPannel.gameObject.SetActive(false);
        _aboutPannel.gameObject.SetActive(false);
        _updateWindowPannel.gameObject.SetActive(false);
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
    public void UpdateRocketsText(string value)
    {
        _rockets.text = "int _rocketsCount = " + value;
    }
    public void UpdateCodeStrokesText(string value)
    {
        _codeStrokes.text = "int _codeStrokesToFinishProj = " + value;
    }



}

