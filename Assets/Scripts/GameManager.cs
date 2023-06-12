using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager : MonoBehaviour
{
    public static GameManager scr;
    public bool isGame;
    private void Awake()
    {
        scr = this;
    }
    private void Start()
    {
        isGame = true;
    }
    public void WinControl(bool win)
    {
        if (!isGame)
            return;
        BaseView.scr.endPanel.EndPanelActive(win);
        isGame = false;
    }

    
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
