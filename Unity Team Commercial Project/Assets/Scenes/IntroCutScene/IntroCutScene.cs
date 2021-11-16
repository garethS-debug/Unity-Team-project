using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroCutScene : MonoBehaviour
{
    [Header("Camera")]
    public Camera cutsceneCamera;


    [Header("Delays")]

    public float cutSceneDelayAtStart = 2;
    public float cutSceneDelay = 5;

    [Header("References")]
    public Animator trainAnimator;
    public LobbyManager lobbyManager;

    public GameObject trainFollowCube;

    [Header("Intro")]
    public GameObject intro;
    public TMP_Text txt;
    [TextArea]
    public string story;

    /*
     * 
     *   
  \n    New line
  \t    Tab
  \v    Vertical Tab
  \b    Backspace
  \r    Carriage return
  \f    Formfeed
  \\    Backslash
  \'    Single quotation mark
  \"    Double quotation mark
  \d    Octal
  \xd    Hexadecimal
  \ud    Unicode character
     */

    // Start is called before the first frame update
    void Start()
    {
     //   trainAnimator.SetBool("StartEntry", false);
        //Start the coroutine we define below named ExampleCoroutine.
        StartCoroutine(CutSceneCoRoutine());

    }

    // Update is called once per frame
    void Update()
    {
        cutsceneCamera.transform.LookAt(trainFollowCube.transform);
    }

    IEnumerator CutSceneCoRoutine()
    {
        intro.gameObject.SetActive(true);
        //Print the time of when the function is first called.
        Debug.Log("Started IntroSplash Coroutine at timestamp : ".Bold().Color("yellow") + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(cutSceneDelayAtStart);
        intro.gameObject.SetActive(false);
        //Print the time of when the function is first called.
        Debug.Log("Started Cutscene Coroutine at timestamp : ".Bold().Color("yellow") + Time.time);

        trainAnimator.SetBool("StartEntry", true);


        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Cutscene Coroutine at timestamp : ".Bold().Color("yellow") + Time.time);

        yield return new WaitForSeconds(cutSceneDelay);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Spawn Player after Cutscene Coroutine at timestamp : ".Bold().Color("yellow") + Time.time);


        lobbyManager.OnTriggerSpawnPlayers();


    }



    void Awake()
    {
        //txt = txt.GetComponent<TMP_Text>();
      //  story = txt.text;
        txt.text = "";

        // TODO: add optional delay when to start
        StartCoroutine("PlayText");
    }

    IEnumerator PlayText()
    {
        foreach (char c in story)
        {
            txt.text += c;
            yield return new WaitForSeconds(0.075f);

            if (c >= story.Length)
            {
                print("End Of Story");
            }
        }
    }

}
