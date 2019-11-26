using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ListHander : MonoBehaviour
{
    public TextMeshProUGUI columnHeader;

    public void PointerEnter()
    {
        WordSorterHandler.currList = this.gameObject;
    }

    public void PointerExit()
    {
        WordSorterHandler.currList = null;
    }

    public bool allCorrect()
    {
        ButtonHandler[] buttons = transform.GetComponentsInChildren<ButtonHandler>();
        foreach (var b in buttons)
        {
            if (b.type != columnHeader.text)
            {
                return false;
            }
        }
        return true;
    }
}
