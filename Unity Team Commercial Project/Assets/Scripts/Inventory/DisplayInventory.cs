using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventoryHuman;
    public InventoryObject inventoryGhost;
    public int xStart;
    public int xSpaceBetweenItems;
    public int yStart;
    public int ySpaceBetweenItems;
    public int numberOfColumns;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    private void Start()
    {
        CreateDisplay();
    }

    public void Update()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if(GameObject.Find("HumanPlayerCharacter"))
        {
            for (int i = 0; i < inventoryHuman.Container.Count; i++)
            {
                if (itemsDisplayed.ContainsKey(inventoryHuman.Container[i]))
                {
                    itemsDisplayed[inventoryHuman.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventoryHuman.Container[i].amount.ToString("n0");
                }
                else
                {
                    var obj = Instantiate(inventoryHuman.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                    obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = inventoryHuman.Container[i].amount.ToString("n0");
                    itemsDisplayed.Add(inventoryHuman.Container[i], obj);
                }
            }
        }

        if (GameObject.Find("GhostCharacter"))
        {
            for (int i = 0; i < inventoryGhost.Container.Count; i++)
            {
                if (itemsDisplayed.ContainsKey(inventoryGhost.Container[i]))
                {
                    itemsDisplayed[inventoryGhost.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventoryGhost.Container[i].amount.ToString("n0");
                }
                else
                {
                    var obj = Instantiate(inventoryGhost.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                    obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = inventoryGhost.Container[i].amount.ToString("n0");
                    itemsDisplayed.Add(inventoryGhost.Container[i], obj);
                }
            }
        }

    }

    public void CreateDisplay()
    {
        if (GameObject.Find("HumanPlayerCharacter"))

            for (int i = 0; i < inventoryHuman.Container.Count; i++)
            {
                var obj = Instantiate(inventoryHuman.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventoryHuman.Container[i].amount.ToString("n0");
                itemsDisplayed.Add(inventoryHuman.Container[i], obj);
            }

        if (GameObject.Find("GhostCharacter"))

            for (int i = 0; i < inventoryGhost.Container.Count; i++)
            {
                var obj = Instantiate(inventoryGhost.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventoryGhost.Container[i].amount.ToString("n0");
                itemsDisplayed.Add(inventoryGhost.Container[i], obj);
            }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (xSpaceBetweenItems * (i % numberOfColumns)), yStart + (-ySpaceBetweenItems * (i / numberOfColumns)), 0f);
    }
}
