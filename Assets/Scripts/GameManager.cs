using System.Collections;
using System.Collections.Generic;
using Glaidiator;
using Glaidiator.BehaviorTree;
using Glaidiator.Model;
using Glaidiator.Model.Collision;
using Glaidiator.Presenter;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    #region Serialized Fields
    public TextMeshProUGUI textRounds;
    public TextMeshProUGUI textScoreP1;
    public TextMeshProUGUI textScoreP2;
    public WorldObject worldObj;
    public CharacterPresenter cha1;
    public CharacterPresenter cha2;
    #endregion
    
    private Character _p1;
    private Character _p2;
    private World _world;
    private int _score1 = 0;
    private int _score2 = 0;
    private int _round = 1;

    private void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {   
        _p1 = cha1.GetCharacter();
        _p2 = cha2.GetCharacter();
        _world = worldObj.World;
        textRounds.text = $"Round {_round}";
        textScoreP1.text = _score1.ToString();
        textScoreP2.text = _score2.ToString();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_p1.IsDead && !_p2.IsDead) return;
        if (_p2.IsDead) _score1++;
        if (_p1.IsDead) _score2++;
        textScoreP1.text = _score1.ToString();
        textScoreP2.text = _score2.ToString();
        NewRound();
    }

    private void NewRound()
    {
        _round++;
        textRounds.text = $"Round {_round}";
        _p1.Revive(Arena.P1StartPos, Arena.P1StartRot);
        _p2.Revive(Arena.P2StartPos, Arena.P2StartRot);
        TheMatrix.instance.ResetActiveTrees();
    }
}
