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

    public static CharacterModel player;
    public static CharacterModel boss;
    
    private Dictionary<Glaidiator.Model.Character, Vector2> positions;
    private Dictionary<Vector2, Glaidiator.Model.Character> characters;
    
    

    private void Awake()
    {
        instance = this;
        positions = new Dictionary<CharacterModel, Vector2>();
        characters = new Dictionary<Vector2, CharacterModel>();

        player.onMove += OnMove;
        boss.onMove += OnMove;
        player.onMove += OnAttack;
        boss.onMove += OnAttack;
    }

    private void OnMove()
    {
        
    }

    private void OnAttack()
    {
        
    }
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    
    //public static Vector2 ConvertToModel(Vector3 )

    public static bool IsIntersectOther(CharacterModel agent1)
    {
        CharacterModel agent2;
        if (agent1.Equals(player))
        {
            agent2 = boss;
        }
        else
        {
            agent2 = player;
        }

        Vector2 origin = new Vector2(player.Transform.Position.x,
            player.Transform.Position.z);
        
        
        
        return false;
    }
}
