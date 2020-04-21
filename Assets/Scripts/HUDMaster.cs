using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDMaster : MonoBehaviour
{
    private GameMaster _gameMaster;
    
    private void Awake()
    {
        _gameMaster = FindObjectOfType<GameMaster>();
    }


    public void OnResetMap()
    {
        _gameMaster.ResetMap(); 
    }


    public void OnCalculatePath()
    {
        _gameMaster.CalculatePath();
    }

    public void OnRandomFillMap()
    {
        _gameMaster.FillMapByRandom();
    }
}
