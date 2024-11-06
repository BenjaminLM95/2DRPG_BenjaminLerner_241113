using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.Rendering;
using TMPro;

public class TileMapGenerator : MonoBehaviour
{
    public Tilemap myTilemap;
    public TileBase Walls;
    public TileBase Door;
    public int[,] multidimensionalMap = new int[25, 25];
    public int[,] TempMap = new int[25, 25];
    private string[] aString = new string[3];
    public TextMeshProUGUI charText;


    // Start is called before the first frame update
    void Start()
    {
        /*string linebreakedstring = $"This is a string with a {Environment.NewLine}linebreak!";
        Debug.Log(linebreakedstring);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/testfile.txt", linebreakedstring); */

        string sJoined = GenerateMapString(20, 25);
        Debug.Log("The result is: " + sJoined);

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

        for(int i = 0; i < width; i++) 
        {
            for (int j = 0; j < height; j++) 
            {
                sMatrix += GenerateString(0, 5); 
            }
            sMatrix += Environment.NewLine; 

        }

        return sMatrix; 
    }

    public  static int randomNumber(int a, int b)
    {
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
    

}
