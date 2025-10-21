using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [Header("References")]
    private UIManager uiManager; 
    StatsManager statsManager;
    FileData fileData; 

    [Header("Level info")]
    public int numRoom;
    public bool isLoadedMap; 


    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        statsManager = FindFirstObjectByType<StatsManager>();
        fileData = FindFirstObjectByType<FileData>();

        uiManager.MainMenuEnable();
        numRoom = 1;

        isLoadedMap = false; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToStartGame() 
    {
        SceneManager.LoadScene("Gameplay");
        statsManager.SetInitialKeys();
        uiManager.GamePlayEnable();
    }

    public void GoToMainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
        numRoom = 1; 
        uiManager.MainMenuEnable();
    }

    public void GoToNextLevel() 
    {
        SceneManager.LoadScene("Gameplay");        
        uiManager.GamePlayEnable();
        numRoom++; 
    }

    public void SaveFile() 
    {
        fileData.Save(); 
        GoToMainMenu();
    }

    public void LoadFile() 
    {
        fileData.Load();
        GoToNextLevel(); 
    }
}
