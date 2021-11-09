using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;	//For creating binary files
using UnityEngine;
using System.IO;            //Input / output
using System;


public class SaveGameManager : MonoBehaviour
{
	//---------------- Static ---------------//

	static private SaveFile saveFile;
	static private string filePath;


	/// <summary>
	/// Summary of reuqirements 
	/// </summary>
	//Contains a static class SaveGameManager

	//Contains a nested class 'SaveFile'

	//Contains a nested class 'saveFile'

	//Contains 'Load' and 'Save' methods to perform serlization and deserilazation

	//Application. PersistentDatapath will get you the recommended save location for all build targets 



	static public bool LOCK
	{
		get;            //Globally get 
		private set;    //cannot change the property from another script

		//lock if true prevents the game from saving. This avoids issues that can happen when loading files.
		//what happens is, in this case, right now the the session requirements have to just save the data automatically. When the game is over, or when unlocking each achievement. 
		//When we are saving function, we override the existing data. If we try to load the existing data, we should put a lock or secuiry measure to prevent
		//scripts from overriding the specific data. We are trying to 'serialize' the data or 'convert' it to readable data. 
		//if we dont employ a lock (to prevent a data override) this can corrupt the save data. Becuase it will save some parts in existing data while trying to 
		//get data at the same time. 
	}



	static SaveGameManager()
	{
		LOCK = false;


		filePath = Application.persistentDataPath + "/Journey.save";

		Debug.Log(filePath);

#if DEBUG_VerboseConsoleLogging
	Debug.Log("SaveGameManager:Awake() - Path: " + filePath);
#endif

		saveFile = new SaveFile();                                                  //craeting an instance of the saveFile path
	}







	static public void Save()                                                       //Method to save data to a file (does not save with web)
	{
		if (LOCK) return;                                                           //If the lock is active return. No save done

		//saveFile.StepRecords = AchievementsManager.GetStepRecords();
		//saveFile.achievements = AchievementsManager.GetAchievements();

		saveFile.slectedCharacter = GameSetup.PlayerCharacter;
		saveFile.playerName = GameSetup.playerName;

		Debug.Log("Saving.... Selected Character: " + saveFile.slectedCharacter);



		//-----Saving in json ----//
		string jsonSaveFile = JsonUtility.ToJson(saveFile, true);                   //Saving to a JASON File



		File.WriteAllText(filePath, jsonSaveFile);
	}






	static public void Load()
	{
		if (File.Exists(filePath))
		{
			string dataAsJson = File.ReadAllText(filePath);                         //Convert JSON to readable 

			try
			{
				saveFile = JsonUtility.FromJson<SaveFile>(dataAsJson);              //Pass this 			
			}
			catch
			{
				Debug.LogWarning("SaveGameManager:Load() - SaveFile was malformed. \n " + dataAsJson);
				return;
			}

			LOCK = true;                                                            //Locking the file
																					//Load the Achemenents
			GameSetup.LoadDataFromSaveFile(saveFile);


			LOCK = false;

		}

		else
		{
#if DEBUG_VerboseConsoleLogging
	Debug.Log("SaveGameManager:Load() - unable to find save file);
#endif
		}
	}


	static public void DeleteSave()
	{

	}

	static public bool CheckforSaveGame()
    {
		if (File.Exists(filePath))
		{
			return true;
		}

		else
		{
			return false;
		}
    }






	[SerializeField]
	public class SaveFile
	{
		//public StepRecord[]	stepRecords;
		//public Achievement[] achievements;

	//	public int highScore = 5000;
		public int slectedCharacter= 0;                         // 1 - Paretn 2- Child
		public string playerName = "";
	//	public int selectedTurret = 0;
	}


}
