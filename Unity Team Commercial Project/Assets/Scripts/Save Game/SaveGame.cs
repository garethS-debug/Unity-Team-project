using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;            //Input / output
using System;
using System.Runtime.Serialization.Formatters.Binary;   //For creating binary files

public class SaveGame : MonoBehaviour
{
	public static SaveGame Instance { get; private set; }

	public static SaveGame saveGame;


	//Example data containers
	public int PLAYER_LEVEL;

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


	void Awake()
	{
		if (saveGame == null)
		{
			saveGame = this;                                        //Ensure only 1 existsts
		}

		else if (saveGame != this)
		{
			Debug.LogWarning("SaveGame: more than one Existst!!");
		}

	}


	public void Save()                                                                                              //-------------Saving--------------
	{
		BinaryFormatter bf = new BinaryFormatter();                                                                     //New Binary formatter

		if (!File.Exists(Application.persistentDataPath + "/saveInfo.dat"))                                              //If save file does not exist
		{

			FileStream file = File.Create(Application.persistentDataPath + "/saveInfo.dat");                            //creates a file

			//---Note- Monobehaviours cant be saved--//

			DataToSave data = new DataToSave();                                                                         //Creates a container


			//-- Save information here. Data.X = X//
		//	data.achievements = AchievementsManager.GetAchievements();                                                  //Adding informaiton to the file
		//	data.stepRecords = AchievementsManager.GetStepRecords();

			Debug.Log("Saving..." + Application.persistentDataPath);


			//	data.playerLevel = PLAYER_LEVEL;

			bf.Serialize(file, data);                                                                                   //Serialize data to file or Writes the container to the file
			file.Close();                                                                                               //Closes the file

		}

		if (File.Exists(Application.persistentDataPath + "/saveInfo.dat"))                                              //Making sure file exists
		{

			FileStream file = File.Open(Application.persistentDataPath + "/saveInfo.dat", FileMode.Open);               //Opens ths file 

			//---Note- Monobehaviours cant be saved--//

			DataToSave data = new DataToSave();                                                                         //Creates a container


			//-- Save information here. Data.X = X//
		//	data.achievements = AchievementsManager.GetAchievements();                                                  //Adding informaiton to the file
		//	data.stepRecords = AchievementsManager.GetStepRecords();

			Debug.Log("Saving..." + Application.persistentDataPath);


			//	data.playerLevel = PLAYER_LEVEL;

			bf.Serialize(file, data);                                                                                   //Serialize data to file or Writes the container to the file
			file.Close();                                                                                               //Closes the file

		}




	}





	public void Load()                                                                                              //-------------Loading---------------
	{
		if (File.Exists(Application.persistentDataPath + "/saveInfo.dat"))                                          //Making sure file exists before loading
		{

			BinaryFormatter bf = new BinaryFormatter();                                                             //New Binary formatter
			FileStream file = File.Open(Application.persistentDataPath + "/saveInfo.dat", FileMode.Open);           //Open the file

			DataToSave data = (DataToSave)bf.Deserialize(file);                                                     //Deserialize. Converting from generic object to 'datatoSave'
			file.Close();                                                                                           //Close File
																													//


			//----- Insert reference code to pass information to other scripts
			Debug.Log("Loading Data.... ");                                                                         //---Example saving
																													//	
			PLAYER_LEVEL = data.playerLevel;                                                                        //Generic save - saving to this file
		//	AchievementsManager.LoadDataFromSaveFile(data);                                                         //Method save - pushing the save data to another script


		}
		else
		{
			Debug.Log("file does not exist.... ");
		}


	}






	[Serializable]                                                                                                  //Make it to save to a file 
	public class DataToSave
	{
		public static DataToSave dataToSave;


		//CHANGE THESE VARIABLES PER GAME 
		public string playerName;                                                                               //Generic Save Information
		public int playerLevel;                                                                                 //Generic Save Information


		//public AchievementsManager.Achievement[] achievements;                                                  //Data to save from Acheivements system
		//public AchievementsManager.StepRecord[] stepRecords;                                                    //Data to save from Acheivements system
	}

}
