using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    private UIManager uiManager; 

    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindFirstObjectByType<UIManager>();

        uiManager.MainMenuEnable(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToStartGame() 
    {
        SceneManager.LoadScene("Gameplay");
        uiManager.GamePlayEnable();
    }

    public void GoToMainMenu() 
    {
        SceneManager.LoadScene("MainMenu"); 
        uiManager.MainMenuEnable();
    }
}
