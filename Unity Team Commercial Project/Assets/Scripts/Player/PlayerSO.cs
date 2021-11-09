using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player_Data", menuName = "player_Data/Player_Data_SO", order = 1)]
public class PlayerSO : ScriptableObject
{

    public int PlayerCharacterChoise;

    public string PlayerName;

    public bool AutoConnect;

}
