using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.Rendering;
using TMPro;
//using UnityEngine.Windows;
using UnityEditor.U2D.Aseprite;

public class TileMapGenerator : MonoBehaviour
{
    public Tilemap myTilemap;
    public TileBase walls;
    public TileBase door;
    public TileBase field;
    public TileBase grass;
    public TileBase player; 
    public string[,] multidimensionalMap = new string[25, 20];
    public int[,] TempMap = new int[25, 25];
    private string[] aString = new string[3];
    public TextMeshProUGUI charText;
    string sJoined;

    // Start is called before the first frame update
    void Start()
    {
        
        sJoined = GenerateMapString(25, 20);
        Debug.Log("The result is: " + sJoined);
        ConvertMapToTilemap(sJoined);

        //charText.text = sJoined; 
       
    }

    // Update is called once per frame
    void Update()
    {

        myTilemap.ClearAllTiles();

        if (Input.GetKeyDown(KeyCode.S)) 
        {
            
            ConvertToMap(sJoined, multidimensionalMap);
            MoveOnMap(multidimensionalMap, 0, -1);

            sJoined = "";
            for (int j = 0; j < 20; j++)
            {
                for (int i = 0; i < 25; i++)
                {
                    sJoined += multidimensionalMap[i, j];
                }
                sJoined += Environment.NewLine;
            }

        }

        if (Input.GetKeyDown(KeyCode.W))
        {

            ConvertToMap(sJoined, multidimensionalMap);
            MoveOnMap(multidimensionalMap, 0, 1);

            sJoined = "";
            for (int j = 0; j < 20; j++)
            {
                for (int i = 0; i < 25; i++)
                {
                    sJoined += multidimensionalMap[i, j];
                }
                sJoined += Environment.NewLine;
            }

        }

        if (Input.GetKeyDown(KeyCode.A))
        {

            ConvertToMap(sJoined, multidimensionalMap);
            MoveOnMap(multidimensionalMap, -1, 0);

            sJoined = "";
            for (int j = 0; j < 20; j++)
            {
                for (int i = 0; i < 25; i++)
                {
                    sJoined += multidimensionalMap[i, j];
                }
                sJoined += Environment.NewLine;
            }

        }

        if (Input.GetKeyDown(KeyCode.D))
        {

            ConvertToMap(sJoined, multidimensionalMap);
            MoveOnMap(multidimensionalMap, 1, 0);

            sJoined = "";
            for (int j = 0; j < 20; j++)
            {
                for (int i = 0; i < 25; i++)
                {
                    sJoined += multidimensionalMap[i, j];
                }
                sJoined += Environment.NewLine;
            }

        }

        ConvertMapToTilemap(sJoined);


    }

    
    
    public string GenerateMapString(int width, int height)
    {
        // '#' for walls, '@' for doors, '*' for field '%' for grass and 'P' for the player
        string sMatrix = "";

        string[,] mapMatrix = new string[width, height];
        string[,] auxMatrix = new string[width, height];

        // Creating a 2-dimensional array for the map
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                    mapMatrix[i,j] = "#";
                else if (i == width / 2 && j == height - 3)
                {
                    //It should be one player only
                    mapMatrix[i, j] = "P"; Debug.Log("P" + i + j);
                }
                else if ((i == width / 2 - 1 || i == width / 2 + 1) && (j == height - 2 || j == height - 4)) 
                {
                    mapMatrix[i, j] = "*"; 
                }
                else
                    mapMatrix[i, j] = GenerateString();
            }
        }

        // Appling rules

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (mapMatrix[i, j] == "@")
                {
                    if (CountForAdjacent(i, j, mapMatrix, "@") == 0) 
                    {
                        mapMatrix[i, j] = "*";
                    }
                }

            }
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
        // '#' for walls, '@' for doors, '*' for field '%' for grass and 'P' for the player
        string charElement;
        int typeOfString = randomNumber(0, 100);

        if (typeOfString < 40)
        {
            charElement = "*";
        }
        else if (typeOfString < 80)
        {
            charElement = "%";
        }
        else if (typeOfString < 95)
        {
            charElement = "#";
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
        var lines = mapData.Split("\n"[0]);
        // '#' for walls, '@' for doors, '*' for field '%' for grass and 'P' for the player
        for (int i = 0; i < lines.Length; i++) { 

            for (var j = 0; j < lines[i].Length - 1; j++)
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
            else if (lines[i][j] == "P"[0]) // player
                
                {
                    myTilemap.SetTile(new Vector3Int(j, i, 0), player);
                }
                else // field by default 
                {
                    myTilemap.SetTile(new Vector3Int(j, i, 0), field);
                }
            }
        }       

    }

    private void ConvertToMap(string sMap, string[,] daMap) 
    {
        var lines = sMap.Split("\n"[0]);
        
        for (int j = 0; j < daMap.GetLength(1); j++)
        {

            for (int i = 0; i < daMap.GetLength(0); i++)
            {
                Debug.Log("i is equal to: " + i + "J is equal to: " + j + " " + daMap.GetLength(0) + " " + daMap.GetLength(1)); 
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
                else if (lines[j][i] == "P"[0]) // player

                {
                    daMap[i, j] = "P";
                }
                else // field by default 
                {
                    daMap[i, j] = "*";
                }
            }
        }
    }

    public void MoveOnMap(string[,] smap, int mvx, int mvy)
    {
        // Get The position of the player
        int playerx = 0;
        int playery = 0;

        for (int i = 0; i < smap.GetLength(0); i++)
        {
            for (int j = 0; j < smap.GetLength(1); j++)
            {
                if (smap[i, j] == "P")
                {
                    playerx = i;
                    playery = j;
                }
            }
        }

        if (checkForCollision(playerx + mvx, playery + mvy, "@", smap) || checkForCollision(playerx + mvx, playery + mvy, "#", smap))
        {
            
            
        }
        else 
        {
            smap[playerx, playery] = "*";
            smap[playerx + mvx, playery + mvy] = "P";
        }

    }

    public int CountForString(int x, int y, string[,] map, string tilestring)
    {
        int count = 0;

        for (int b = y - 1; b < y + 2; b++)
        {
            for (int a = x - 1; a < x + 2; a++)
            {

                if (a < 0 || b < 0 || a >= multidimensionalMap.GetLength(0) || b >= multidimensionalMap.GetLength(1) || (a == x && b == y))
                    continue;
                Debug.Log($"Checking position x {a} and y {b}. Position is {map[a, b]}. Is position 1? {map[a, b] == tilestring}");
                if (map[a, b] == tilestring)
                {
                    count++;
                    Debug.Log($"Position x {a} and y {b}. value is 1! counting! Count is now {count}");
                }
            }
        }

        return count;
    }

    public int CountForAdjacent(int x, int y, string[,] map, string tilestring)
    {
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
        if (map[x,y] == colChar) 
        {
            return true;
        }
        else
        {
            return false;
        }

     }



}
