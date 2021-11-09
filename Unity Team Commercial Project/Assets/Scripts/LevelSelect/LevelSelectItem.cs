using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System;


public class LevelSelectItem : MonoBehaviour
{
    [SerializeField] private bool unlocked; //cannot be accessed from outside the class

    public Image unlockedImage;
    public GameObject[] stars;

    public Scene loadScene;


    public LevelInfoSO levelInfo;

    public int currentScore;


 //   [SerializeField] private List<LevelInfoSO.SALevelScore> starScoreValue;

    /*
private void OnGUI()
{
    DisplayLevel(levelToLoad);
}


public void DisplayLevel(SceneReference scene)
{
    GUILayout.Label(new GUIContent("Scene name Path: " + scene));
    if (GUILayout.Button("Load " + scene))
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
*/
    public SceneReference levelToLoad;


    public void Start()
    {
        if (levelInfo!= null)
        {
            currentScore = levelInfo.pointsGainedInLevel;
        }
     

        for (int i = 0; i < stars.Length ; i++)
        {
            stars[i].gameObject.SetActive(false);
        }

        UpdateLevelImage();


    }


    private void UpdateLevelImage()
    {
        if (!unlocked) //False bool = locked
        {
            unlockedImage.gameObject.SetActive(true);

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(false);
            }
        }
        else //if true
        {
            unlockedImage.gameObject.SetActive(false);

            int starsToLoopThrough = 0;

            foreach (LevelInfoSO.SALevelScore scoreStruct in levelInfo.LevelScoreValue)
            {


                if (scoreStruct.value <= currentScore)
                {
                    starsToLoopThrough += 1;
                }


                print("Stars to Loop through " + starsToLoopThrough);

            }

            //Length of array = 3 , we want to check 1st 2 (array - 1) 

            for (int i = 0; i < stars.Length - (stars.Length - starsToLoopThrough); i++)
            {

                print("Stars checked = " + (stars.Length - (stars.Length - starsToLoopThrough)));
                stars[i].gameObject.SetActive(true);
            }
            

        }


    }

    public void OnClickedLevelSelect()
    {
        if (unlocked) //true bool = locked
        {

          PhotonNetwork.LoadLevel(levelToLoad); //Load the 'Game' scene when master cliet clicks
            print(levelToLoad.ScenePath);
        }
        else //if true
        {
        
        }
    }
    /*
    /// <summary>
    /// Look at all the Score configuration and return the score according to a specific Asteroid Size
    /// </summary>
    /// <param name="_scale">The size that you want to score</param>
    /// <returns>The score of the given size</returns>
    public int GetScoreFromSize(LevelInfoSO.EScale _scale)
    {
        return starScoreValue.Find(x => x.scale == _scale).value;
    }



    /*
    public int returnScoreValue(LevelInfoSO.EScale scale)
    {
        switch (scale)
        {
            case LevelInfoSO.EScale.Min:
                //  spawnedAsteroid.transform.localScale = Vector3.one * 0.5f;
                return 100;
                break;
            case LevelInfoSO.EScale.Medium:
                //   spawnedAsteroid.transform.localScale = Vector3.one;
                return 100;
                break;
            case LevelInfoSO.EScale.Max:
                //  spawnedAsteroid.transform.localScale = Vector3.one * 3;
                return 100;
                break;
        }

    }
    */
}
