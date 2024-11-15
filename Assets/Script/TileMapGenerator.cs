using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.Rendering;
using TMPro;
//using UnityEngine.Windows;
using UnityEditor.U2D.Aseprite;
using static UnityEditor.Experimental.GraphView.GraphView;


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
    public string[,] multidimensionalMap = new string[25, 20];
    private string[] aString = new string[3];
    public TextMeshProUGUI charText;
    string sJoined;
    int player_x = 0;
    int player_y = 0;
    string pathToMyFile;

    // Start is called before the first frame update
    void Start()
    {
        // To generate a Random map
        /*
        sJoined = GenerateMapString(25, 20);      
        ConvertToMap(sJoined, multidimensionalMap);
        ConvertMapToTilemap(sJoined);
        charText.text = sJoined;  
        */
        // this ends for the random map

        //To generate a map based on a text file
        
        pathToMyFile = $"{Application.dataPath}/TextFileMap.txt";
        LoadPremadeMap(pathToMyFile);

        ConvertToMap(System.IO.File.ReadAllText(pathToMyFile), multidimensionalMap);
        
        // this ends for the text file map

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

        //ConvertMapToTilemap(sJoined);     //Comment the command from the left only if you are running with a text file

        LoadPremadeMap(pathToMyFile);       //Comment the command from the left only if you are running with a random map generator

        myTilemap.SetTile(new Vector3Int(player_x, player_y, 1), player);

        if (Input.GetKeyDown(KeyCode.S))
        {
            // The player moves down
            if (checkForCollision(player_x, player_y - 1, "#", multidimensionalMap) || checkForCollision(player_x, player_y - 1, "@", multidimensionalMap))
            { //Debug.Log("You can't pass" + player_x + " " + player_y);
            }
            else
                player_y--;

            SwapPositions();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {

            // The player moves up
            if (checkForCollision(player_x, player_y + 1, "#", multidimensionalMap) || checkForCollision(player_x, player_y + 1, "@", multidimensionalMap))
            { //Debug.Log("You can't pass" + player_x + " " + player_y);
            }
            else
                player_y++;



            SwapPositions();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {

            // The player moves left
            if (checkForCollision(player_x - 1, player_y, "#", multidimensionalMap) || checkForCollision(player_x - 1, player_y, "@", multidimensionalMap))
            { //Debug.Log("You can't pass" + player_x + " " + player_y);
            }
            else
                player_x--;


            SwapPositions();

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            // The player moves right
            if (checkForCollision(player_x + 1, player_y, "#", multidimensionalMap) || checkForCollision(player_x + 1, player_y, "@", multidimensionalMap))
            { //Debug.Log("You can't pass" + player_x + " " + player_y);
            }
            else
                player_x++;

            SwapPositions();


        }



    }



    public string GenerateMapString(int width, int height)
    {
        // '#' for walls, '@' for doors, '*' for field '%' for grass, '&' for a tree
        // Creating a bidimensional array for the map to later convert it into a string
        string sMatrix = "";

        string[,] mapMatrix = new string[width, height];

        //string[,] auxMatrix = new string[width, height];

        // Creating a 2-dimensional array for the map
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                    mapMatrix[i, j] = "#";  //The borders should be walls
                else if (i == width / 2 && j == height - 3)
                {
                    //Where the player is
                    mapMatrix[i, j] = "P";

                }
                else
                    mapMatrix[i, j] = GenerateString();
            }
        }

        // Appling rules
        // It can't be multiple doors together
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


        int md1x = randomNumber(2, width / 3);
        int md1y = randomNumber(2, height / 3);
        int md2x = randomNumber(2 * width / 3, width - 3);
        int md2y = randomNumber(2 * height / 3, height - 3);


        mapMatrix[md1x, md1y] = "!";
        mapMatrix[md2x, md2y] = "^";

        if ((CountForAdjacent(md1x, md1y, mapMatrix, "@") > 0) || (CountForAdjacent(md1x, md1y, mapMatrix, "#") > 0))
        {
            mapMatrix[md1x + 1, md1y] = "*";
            mapMatrix[md1x - 1, md1y] = "*";
            mapMatrix[md1x, md1y - 1] = "*";
            mapMatrix[md1x, md1y + 1] = "*";
            Debug.Log("changed in MD1"); 
        }

        if ((CountForAdjacent(md2x, md2y, mapMatrix, "@") > 0) || (CountForAdjacent(md2x, md2y, mapMatrix, "#") > 0))
        {
            mapMatrix[md2x + 1, md2y] = "*";
            mapMatrix[md2x - 1, md2y] = "*";
            mapMatrix[md2x, md2y - 1] = "*";
            mapMatrix[md2x, md2y + 1] = "*";
            Debug.Log("changed in MD2");
        }


        // Create a string based on the 2D array map

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                sMatrix += mapMatrix[i, j]; 
            }
            sMatrix += Environment.NewLine; 
        }

        return sMatrix; 
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
        // '#' for walls, '@' for doors, '*' for field '%' for grass, '$' for grass2, '&' for a tree
        string charElement;
        int typeOfString = randomNumber(0, 100);

        if (typeOfString < 35)
        {
            charElement = "*";
        }
        else if (typeOfString < 50)
        {
            charElement = "%";
        }
        else if (typeOfString < 65) 
        {
            charElement = "$"; 
        }
        else if (typeOfString < 84)
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
        // '#' for walls, '@' for doors, '*' for field '%' for grass, '$' for grass2, '&' for a tree
        var lines = sMap.Split("\n"[0]);

        for (int j = 0; j < daMap.GetLength(1); j++)
        {

            for (int i = 0; i < daMap.GetLength(0); i++)
            {
                //Debug.Log("i is equal to: " + i + "J is equal to: " + j + " " + daMap.GetLength(0) + " " + daMap.GetLength(1)); 
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
                else if (lines[j][i] == "^"[0]) //2nd magic door
                        daMap[i, j] = "^"; 
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

    public void SwapPositions() 
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

}
