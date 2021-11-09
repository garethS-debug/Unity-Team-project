using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level_Data", menuName = "Level_Data/Level_Data_SO", order = 1)]
public class LevelInfoSO : ScriptableObject
{
	

	[Header("Level Settings")]
	[SerializeField] public List<SALevelScore> LevelScoreValue;

	[Header("info updated during level")]
	public int pointsGainedInLevel;



	public struct LevelData
	{
		public GameObject AsteriodPrefab;
	}



	[System.Serializable]
	public struct SALevelScore
	{
		public EScale scale;
		public int value;


	}

	public enum EScale
	{
		Min,
		Medium,
		Max
	}


}
