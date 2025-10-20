using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using TMPro;


public class TileMapGenerator : MonoBehaviour
{
    public Tilemap myTilemap;
    public TileBase walls;
    public TileBase door;
    public TileBase field;
    public TileBase grass;
    public TileBase player;
    public TileBase grass2;
    public TileBase tree;
    public TileBase magicDoor;
    public TileBase magicDoor2;
    public TileBase openedDoor;
    public TileBase keys; 
    public string[,] multidimensionalMap = new string[25, 20];
    private string[] aString = new string[3];
    public TextMeshProUGUI stringMapText;
    public TextMeshProUGUI keyCountText; 
    string sJoined;
    int player_x = 0;
    int player_y = 0;
    string pathToMyFile;
    int nKey;
    public int keymax = 3;  //There can only be 3 keys on the map

    // Start is called before the first frame update
    void Start()
    {
        // To generate a Random map
        
        sJoined = GenerateMapString(25, 20);      
        ConvertToMap(sJoined, multidimensionalMap);
        ConvertMapToTilemap(sJoined);
        stringMapText.text = sJoined;
        
        // this ends for the random map

        //To generate a map based on a text file
<<<<<<< HEAD
        /*
        pathToMyFile = $"{Application.dataPath}/TextFileMap.txt";
        ConvertToMap(System.IO.File.ReadAllText(pathToMyFile), multidimensionalMap);
        LoadPremadeMap(pathToMyFile);
        */
=======

        //pathToMyFile = $"{Application.dataPath}/TextFileMap.txt";
        //ConvertToMap(System.IO.File.ReadAllText(pathToMyFile), multidimensionalMap);
        //LoadPremadeMap(pathToMyFile);
        
>>>>>>> 4dd99c06e847f455390bd7e8b0e4f5d667926539
        
        
        // this ends for the text file map

        nKey = 1; 
        

        for (int i = 0; i < multidimensionalMap.GetLength(0); i++)
        {
            for (int j = 0; j < multidimensionalMap.GetLength(1); j++)
            {
                if (multidimensionalMap[i, j] == "P")
                {
                    player_x = i;
                    player_y = j;

                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        myTilemap.ClearAllTiles();

        ConvertMapToTilemap(convertMapToString(25, 20, multidimensionalMap));   
        
        myTilemap.SetTile(new Vector3Int(player_x, player_y, 1), player);

        if (Input.GetKeyDown(KeyCode.S))
        {
            // The player moves down
            if (checkForCollision(player_x, player_y - 1, "#", multidimensionalMap) || checkForCollision(player_x, player_y - 1, "@", multidimensionalMap))
            {
                if(nKey > 0) 
                {
                    openDoor(player_x, player_y - 1, multidimensionalMap);                     
                }
                
            }
            else
                player_y--;

            teletransport();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {

            // The player moves up
            if (checkForCollision(player_x, player_y + 1, "#", multidimensionalMap) || checkForCollision(player_x, player_y + 1, "@", multidimensionalMap))
            {
                if (nKey > 0)
                {
                    openDoor(player_x, player_y + 1, multidimensionalMap);
                }
            }
            else
                player_y++;

            teletransport();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {

            // The player moves left
            if (checkForCollision(player_x - 1, player_y, "#", multidimensionalMap) || checkForCollision(player_x - 1, player_y, "@", multidimensionalMap))
            {
                if (nKey > 0)
                {
                    openDoor(player_x - 1, player_y, multidimensionalMap);                    
                }
            }
            else
                player_x--;


            teletransport();

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            // The player moves right
            if (checkForCollision(player_x + 1, player_y, "#", multidimensionalMap) || checkForCollision(player_x + 1, player_y, "@", multidimensionalMap))
            {
                if (nKey > 0)
                {
                    openDoor(player_x + 1, player_y, multidimensionalMap);                    
                }
            }
            else
                player_x++;

            teletransport();


        }

        if(checkForCollision(player_x, player_y, "k", multidimensionalMap)) 
        {
            nKey++;
            multidimensionalMap[player_x, player_y] = "*"; 
        }

        keyCountText.text = "Keys: " + nKey; 

    }



    public string GenerateMapString(int width, int height)
    {
        // '#' for walls, '@' for doors, '*' for field '%' for grass, '&' for a tree
        // Creating a bidimensional array for the map to later convert it into a string
        //string sMatrix = "";

        string[,] mapMatrix = new string[width, height];

        //string[,] auxMatrix = new string[width, height];

        // Creating a 2-dimensional array for the map
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                    mapMatrix[i, j] = "#";  //1st rule: The borders should be walls
                else if (i == width / 2 && j == height - 3)
                {
                    //Where the player is
                    mapMatrix[i, j] = "P";

                }
                else
                    mapMatrix[i, j] = GenerateString();
            }
        }

        
        // 2nd Rule: if there's many doors, reduce the amount convert it into normal field
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (mapMatrix[i, j] == "@")
                {
                    if (CountForString(i, j, mapMatrix, "@") > 2)
                    {
                        mapMatrix[i, j] = "*";
                    }
                }

            }
        }

        // generating the coordinates for the two magic doors
        int md1x = randomNumber(2, width / 3);
        int md1y = randomNumber(2, height / 3);
        int md2x = randomNumber(2 * width / 3, width - 3);
        int md2y = randomNumber(2 * height / 3, height - 3);


        mapMatrix[md1x, md1y] = "!";
        mapMatrix[md2x, md2y] = "^";

        //3rd rule: if there's any door or wall in the adjacent tiles from the magic doors, all those tiles would be replaced by standard field tile
        if ((CountForAdjacent(md1x, md1y, mapMatrix, "@") > 0) || (CountForAdjacent(md1x, md1y, mapMatrix, "#") > 0))
        {
            mapMatrix[md1x + 1, md1y] = "*";
            mapMatrix[md1x - 1, md1y] = "*";
            mapMatrix[md1x, md1y - 1] = "*";
            mapMatrix[md1x, md1y + 1] = "*";            
        }

        if ((CountForAdjacent(md2x, md2y, mapMatrix, "@") > 0) || (CountForAdjacent(md2x, md2y, mapMatrix, "#") > 0))
        {
            mapMatrix[md2x + 1, md2y] = "*";
            mapMatrix[md2x - 1, md2y] = "*";
            mapMatrix[md2x, md2y - 1] = "*";
            mapMatrix[md2x, md2y + 1] = "*";            
        }
                
        return convertMapToString(width, height, mapMatrix); 
    }

    string convertMapToString(int x, int y, string[,] smap) 
    {
        string result = "";

        for (int j = 0; j < y; j++)
        {
            for (int i = 0; i < x; i++)
            {
                result += smap[i, j];
            }
            result += Environment.NewLine;
        }

        return result; 
    }

    public  static int randomNumber(int a, int b)
    {
        // Generate a random number to later get a random character
        System.Random random = new System.Random();
        int rslt = random.Next(a, b);
        return rslt; 
    }

    string GenerateString() 
    {
        // Generate the char at random
        // '#' for walls, '@' for doors, '*' for field '%' for grass, '$' for grass2, '&' for a tree, 'k' for keys
        string charElement;        
        int typeOfString = randomNumber(0, 100);

        if (typeOfString < 35)
        {
            charElement = "*";
        }
        else if (typeOfString < 55)
        {
            charElement = "%";
        }
        else if (typeOfString < 75) 
        {
            charElement = "$"; 
        }
        else if (typeOfString < 85 && keymax > 0) 
        {
            charElement = "k";
            keymax--;            //There can only be 3 keys on the map
        }
        else if (typeOfString < 86)
        {
            charElement = "#";
        }
        else if (typeOfString < 92) 
        {
            charElement = "&"; 
        }
        else if (typeOfString < 100)
        {
            charElement = "@";
        }
        else
        {
            charElement = "*"; 
        }

        
        return charElement;
    }

    private void ConvertMapToTilemap(string mapData)
    {
        // Split the char (string) to set it into the 2d array
        var lines = mapData.Split("\n"[0]);
        // '#' for walls, '@' for doors, '*' for field '%' for grass, '$' for grass2, '&' for a tree
        for (int i = 0; i < lines.Length; i++) {

            for (var j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == "#"[0]) // wall
                {
                    myTilemap.SetTile(new Vector3Int(j, i, 0), walls);
                }
                else if (lines[i][j] == "@"[0])  // door
                {
                    myTilemap.SetTile(new Vector3Int(j, i, 0), door);
                }
                else if (lines[i][j] == "*"[0]) // field
                {
                    myTilemap.SetTile(new Vector3Int(j, i, 0), field);
                }
                else if (lines[i][j] == "%"[0]) // grass 
                {
                    myTilemap.SetTile(new Vector3Int(j, i, 0), grass);
                }
                else if (lines[i][j] == "$"[0]) // grass2

                {
                    myTilemap.SetTile(new Vector3Int(j, i, 0), grass2);
                }
                else if (lines[i][j] == "&"[0]) // tree
                {
                    myTilemap.SetTile(new Vector3Int(j, i, 0), tree);
                }
                else if (lines[i][j] == "P"[0]) //Player
                {
                    myTilemap.SetTile(new Vector3Int(j, i, 0), field);
                }
                else if (lines[i][j] == "!"[0]) //magic door
                    myTilemap.SetTile(new Vector3Int(j, i, 0), magicDoor);
                else if (lines[i][j] == "^"[0]) //2nd magic door
                    myTilemap.SetTile(new Vector3Int(j, i, 0), magicDoor2);
                else if (lines[i][j] == "o"[0]) //Open Door
                {
                    myTilemap.SetTile(new Vector3Int(j, i, 0), openedDoor);
                }
                else if (lines[i][j] == "k"[0]) // Keys
                    myTilemap.SetTile(new Vector3Int(j, i, 0), keys); 
                else // field by default 
                {
                    myTilemap.SetTile(new Vector3Int(j, i, 0), null);
                }
            }
        }       

    }

    private void ConvertToMap(string sMap, string[,] daMap)
    {
        // Split the char (string) from a specific 2d array
        // '#' for walls, '@' for doors, '*' for field '%' for grass, '$' for grass2, '&' for a tree, 'P' for players, '!' or '^' for magic doors
        var lines = sMap.Split("\n"[0]);

        for (int j = 0; j < daMap.GetLength(1); j++)
        {

            for (int i = 0; i < daMap.GetLength(0); i++)
            {                 
                if (lines[j][i] == "#"[0]) // wall
                {
                    daMap[i, j] = "#";
                }
                else if (lines[j][i] == "@"[0])  // door
                {
                    daMap[i, j] = "@";
                }
                else if (lines[j][i] == "*"[0]) // field
                {
                    daMap[i, j] = "*";
                }
                else if (lines[j][i] == "%"[0]) // grass 
                {
                    daMap[i, j] = "%";
                }
                else if (lines[j][i] == "$"[0]) // grass2

                {
                    daMap[i, j] = "$";
                }
                else if (lines[j][i] == "&"[0]) // tree
                {
                    daMap[i, j] = "&";
                }
                else if (lines[j][i] == "P"[0]) // Player
                {
                    daMap[i, j] = "P";
                }
                else if (lines[j][i] == "!"[0]) //magic door
                    daMap[i, j] = "!";
                else if (lines[j][i] == "o"[0]) //Opened door
                    daMap[i,j] = "o";   
                else if (lines[j][i] == "^"[0]) //2nd magic door
                        daMap[i, j] = "^"; 
                else if (lines[j][i] == "k"[0]) // keys
                    daMap[i,j] = "k";
                else // field by default 
                {
                    daMap[i, j] = "";
                }
            }
        }
    }

    
    public int CountForString(int x, int y, string[,] map, string tilestring)
    {
        // it counts how many a specific strings are around a specific character
        int count = 0;

        for (int b = y - 1; b < y + 2; b++)
        {
            for (int a = x - 1; a < x + 2; a++)
            {

                if (a < 0 || b < 0 || a >= multidimensionalMap.GetLength(0) || b >= multidimensionalMap.GetLength(1) || (a == x && b == y))
                    continue;
                
                if (map[a, b] == tilestring)
                {
                    count++;
                    
                }
            }
        }

        return count;
    }

    public int CountForAdjacent(int x, int y, string[,] map, string tilestring)
    {
        // it counts how many characters are in adjacent spaces (right, up, left and down)
        int count = 0;
        int pass = 0;

        if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1))
            pass++;  // This is not important, nothing is suppose to happen so I filled this with this variable but I'm not going to use it
        else if (map[x, y + 1] == tilestring || map[x - 1, y] == tilestring || map[x, y - 1] == tilestring || map[x = 1, y] == tilestring)
        { 
            count++; 
        }

        return count;
    }

    public bool checkForCollision(int x, int y, string colChar, string[,] map) 
    {
        //Check if in a specific position, a specific characte exists on it
        if (map[x,y] == colChar) 
        {
            return true;
        }
        else
        {
            return false;
        }

     }

    public void LoadPremadeMap(string mapFilePath) 
    {
        string myLines = System.IO.File.ReadAllText(mapFilePath);
        ConvertMapToTilemap(myLines);


    }

    public void teletransport() 
    {

        // check if you are colliding with the 1st magic door, an then get the position of the 2nd magic door to be able to swap positions
        if (checkForCollision(player_x, player_y, "!", multidimensionalMap))
        {
            for (int j = 0; j < multidimensionalMap.GetLength(1); j++)
            {
                for (int i = 0; i < multidimensionalMap.GetLength(0); i++)
                {
                    if (multidimensionalMap[i, j] == "^")
                    {

                        player_x = i;
                        player_y = j;

                    }
                }
            }
        }
        else if (checkForCollision(player_x, player_y, "^", multidimensionalMap))
        {
            // check if you are colliding with the 2nd magic door, an then get the position of the 1st magic door to be able to swap positions
            for (int j = 0; j < multidimensionalMap.GetLength(1); j++)
            {
                for (int i = 0; i < multidimensionalMap.GetLength(0); i++)
                {
                    if (multidimensionalMap[i, j] == "!" )
                    {

                        player_x = i;
                        player_y = j;

                    }
                }
            }
        }
    }

    public void openDoor(int x, int y, string[,] smap) 
    {
        if (smap[x, y] == "@")
        { 
            smap[x, y] = "o";
            nKey--;
        }    
    }

}
