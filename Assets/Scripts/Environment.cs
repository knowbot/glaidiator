using System;
using System.Collections;
using System.Collections.Generic;
using Glaidiator.Model;
using UnityEngine;

public class Environment : MonoBehaviour
{

    public static Environment instance;

    // arena dimensions
    public static double width;
    public static double depth;

    
    
    private Dictionary<Glaidiator.Model.Character, Vector2> positions;
    private Dictionary<Vector2, Glaidiator.Model.Character> characters;
    
    

    private void Awake()
    {
        instance = this;
    }

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    // take character model component, update data in environment
    public static void MoveAgent()
    {
        
    }
    
    //public static Vector2 ConvertToModel(Vector3 )
}
