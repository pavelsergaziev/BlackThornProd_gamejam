using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonListener : MonoBehaviour {

    private GameManager _gameManager;
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

    }
    public void StartNewGame()
    {
        _gameManager.StartNewGame();
    }
    public void StartGameplay()
    {
        _gameManager.StartGameplay();
    }
    public void ReturnToMainMenu()
    {
        _gameManager.ReturnToMainMenu();
    }
    public void RestartGame()
    {
        _gameManager.RestartGameplay();
    }
    public void ResumeGame()
    {
        _gameManager.ResumeGame();
    }
    public void PauseGame()
    {
        _gameManager.PauseGame();
    }
}
