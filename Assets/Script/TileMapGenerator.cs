using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.Windows;

public class TileMapGenerator : MonoBehaviour
{
    public Tilemap myTilemap;
    public TileBase walls;
    public TileBase door;
    public TileBase field;
    public TileBase grass;
    public TileBase player; 
    public int[,] multidimensionalMap = new int[25, 25];
    public int[,] TempMap = new int[25, 25];
    private string[] aString = new string[3];
    public TextMeshProUGUI charText;


    // Start is called before the first frame update
    void Start()
    {
        
        string sJoined = GenerateMapString(25, 20);
        Debug.Log("The result is: " + sJoined);
        //ConvertMapToTilemap(sJoined);

        charText.text = sJoined; 
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public string GenerateMapString(int width, int height)
    {
        // '#' for walls, '@' for doors, '*' for field '%' for grass and 'P' for the player
        string sMatrix = "";

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                // the border has to be walls
                if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                    sMatrix += "#";
                else if (i == width / 2 && j == height - 3) 
                { 
                    //It should be one player only
                    sMatrix += "P"; Debug.Log("P" + i + j); 
                }                    
                else
                    sMatrix += GenerateString(0, 4); 
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

    string GenerateString(int min, int max) 
    {
        // '#' for walls, '@' for doors, '*' for field '%' for grass and 'P' for the player
        string charElement;
        int typeOfString = randomNumber(min, max);

        switch (typeOfString)
        {
            case 0:
                charElement = "#";
                break;
            case 1:
                charElement = "@";
                break;
            case 2:
                charElement = "*";
                break;
            case 3:
                charElement = "%";
                break;
            case 4:
                charElement = "P";
                break;
            default:
                charElement = "*";
                break;
        }


        return charElement;
    }

    private void ConvertMapToTilemap(string mapData)
    {
        var lines = mapData.Split("\n"[0]);
        // '#' for walls, '@' for doors, '*' for field '%' for grass and 'P' for the player
        for (int i = 0; i < lines.Length; i++) { 

            for (var j = 0; j < lines[i].Length; j++)
            {
            if (lines[i][j] == "#"[0]) // wall
                {

                } 
            else if (lines[i][j] == "@"[0])  // door
                {

                }
            else if (lines[i][j] == "*"[0]) // field
                {

                }
            else if (lines[i][j] == "%"[0]) // grass 
                {

                }
            else if (lines[i][j] == "P"[0]) // player
                
                {

                }
                else // field by default 
                {

                }
            }
        }       

    }


}
