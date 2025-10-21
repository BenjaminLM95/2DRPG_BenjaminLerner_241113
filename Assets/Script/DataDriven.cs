using UnityEngine;
using System.IO; 


public class DataDriven : MonoBehaviour
{
    public static GameData gData = new GameData()
    {
        _numKeys = 5,
        _numLevels = 3,

        _stringMap = "#########################\r\n#*@k$$**@*$k$$%&*@%*&$$$#\r\n#k***@***%%$*%%%*$%&#&%@#\r\n#&!>$*&#****#**#&%%**&$*#\r\n" +
        "#***@**$$%*#&&%*&*%**$*%#\r\n##$%***%*#@%$*$$%%**%%**#\r\n#$*@*#%%**#$***$**$$#$*%#\r\n#***$%$*%$%*$&&%%**%@$**#\r\n#*$$#$%%***%$*%%#*$*$%%%#\r\n" +
        "#%*$**%@&%$$%$%%*$**#***#\r\n#*$%*&*%**$*#%#*&%@*$#*$#\r\n#%@%****%@*#**%*$$&@@$%%#\r\n#$*%&*$&%@#$$#*$*##&&%%%#\r\n#$**@*%%&#&*%$%*$%$%%^*%#\r\n" +
        "#@*$$&&%*$@$%%#%%$*$%$**#\r\n##%%&*&&$**$@**%*$*%**$$#\r\n#%$&@%**$%%$$@*$**%@**%%#\r\n##*&$$*@$$#%P#@@$*%$*#*%#\r\n#**@#$*%***$##$*****%$%*#\r\n" +
        "#########################"

    };

    string json = JsonUtility.ToJson(gData);

    public string path = Application.streamingAssetsPath + "/DataDriven.json";
    [Header("References")]
    [SerializeField] private StatsManager statsManager;
    [SerializeField] private LevelManager levelManager; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statsManager = FindFirstObjectByType<StatsManager>();
        levelManager = FindFirstObjectByType<LevelManager>();
        path = Application.streamingAssetsPath + "/DataDriven.json";
        Debug.Log(json);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveJSON(GameData _gData, string filePath) 
    {
        json = JsonUtility.ToJson(_gData);
        File.WriteAllText(filePath, json);
    }

    public void LoadJSON() 
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "DataDriven.json");

        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            GameData data = JsonUtility.FromJson<GameData>(jsonString);

            Debug.Log($"Keys: {data._numKeys}, Level: {data._numLevels}, Map: {data._stringMap}");

           statsManager.playerKey = data._numKeys;
           levelManager.numRoom = data._numLevels - 1;
           statsManager.mapString = data._stringMap;
            levelManager.isLoadedMap = true;
        }
        else
        {
            Debug.LogError("File not found!");
        }


    }


}
