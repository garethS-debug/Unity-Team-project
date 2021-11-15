using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GriefBarDisplay : MonoBehaviour
{
    public Slider slider;
    public GriefBarObject griefBarHuman;
    public GriefBarObject griefBarGhost;
    public int maxGrief = 100;
    public int currentGrief;

    public void Start()
    {
        CreateDisplay();
        currentGrief = maxGrief;
        SetMaxGrief(maxGrief);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReduceGriefBar(20);
        }
    }

    public void CreateDisplay()
    {
        if(GameObject.Find("HumanPlayerCharacter"))
        {
            var obj = Instantiate(griefBarHuman.griefBarPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
            slider.fillRect = obj.GetComponent<RectTransform>();
        }

        if (GameObject.Find("GhostCharacter"))
        {
            var obj = Instantiate(griefBarGhost.griefBarPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
            slider.fillRect = obj.GetComponent<RectTransform>();
        }
    }

    private void SetMaxGrief(int grief)
    {
        slider.maxValue = grief;
        slider.value = grief;
    }

    private void SetGrief(int grief)
    {
        slider.value = grief;
    }

    private void ReduceGriefBar(int reduction)
    {
        currentGrief -= reduction;
        SetGrief(currentGrief);
    }
}
