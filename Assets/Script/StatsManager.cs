using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public int playerKey;

    public string mapString; 


    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
       

    public void ObtainOneKey() 
    {
        playerKey++; 
    }

    public void UseAKey() 
    {
        playerKey--;
    }

    public void SetInitialKeys() 
    {
        playerKey = 1; 
    }

    public void getMapString(string sMap) 
    {
        mapString = sMap;
    }
}
