using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class FileData : MonoBehaviour
{
    [SerializeField] private StatsManager statsManager;
    [SerializeField] private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        statsManager = FindFirstObjectByType<StatsManager>();
        levelManager = FindFirstObjectByType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save() 
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PlayerData.dat");

        GameData gdata = new GameData();
        gdata._numKeys = statsManager.playerKey;
        gdata._numLevels = levelManager.numRoom;
        gdata._stringMap = statsManager.mapString;

        Debug.Log("Data saved");

        binaryFormatter.Serialize(file, gdata);
        file.Close();
    }

    public void Load() 
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.dat"))
        {

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
            GameData gameData = (GameData)binaryFormatter.Deserialize(file);

            statsManager.playerKey = gameData._numKeys;
            statsManager.mapString = gameData._stringMap;
            levelManager.numRoom = gameData._numLevels;
            levelManager.isLoadedMap = true; 

            Debug.Log("Data Loaded");
        }
        else
            Debug.Log("No File exists");
    }
}
