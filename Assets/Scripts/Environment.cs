using System;
using System.Collections;
using System.Collections.Generic;
using Character.Model;
using UnityEngine;

public class Environment : MonoBehaviour
{

    public static Environment instance;

    // arena dimensions
    public static double width;
    public static double depth;

    
    
    private Dictionary<CharacterModel, Vector2> positions;
    private Dictionary<Vector2, CharacterModel> characters;
    
    

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
