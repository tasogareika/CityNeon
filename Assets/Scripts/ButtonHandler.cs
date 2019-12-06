using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string type;
    [HideInInspector] public bool inList;
    private RectTransform thisRect;
    private float xOffset, yOffset, appWidthLimit, appHeightLimit;

    private void Start()
    {
        inList = false;
        thisRect = GetComponent<RectTransform>();
        xOffset = thisRect.rect.width * 0.5f;
        yOffset = thisRect.rect.height * 0.5f;
        appWidthLimit = BackendHandler.singleton.appWidth - thisRect.rect.width;
        appHeightLimit = BackendHandler.singleton.appHeight - thisRect.rect.height;
    }

    #region WORD PICKER FUNCTIONS
    public void OnClick()
    {
        if (WordPickerHandler.singleton.canClick)
        {
            WordPickerHandler.singleton.checkWord(textDisplay.text, gameObject);
        }
    }
    #endregion

    #region WORD SORTER FUNCTIONS
    public void StartDrag()
    {
        transform.SetParent(WordSorterHandler.singleton.mainCanvas.transform);
        transform.SetSiblingIndex(1);
        WordSorterHandler.currButton = gameObject;
        thisRect.anchorMin = new Vector2(0, 0);
        thisRect.anchorMax = new Vector2(0, 0);
    }

    public void OnDrag()
    {
        thisRect.anchoredPosition = new Vector2(Input.mousePosition.x - xOffset, Input.mousePosition.y - yOffset);
    }

    public void DragEnd()
    {
        Vector2 currPos = thisRect.anchoredPosition;

        //check if crossed boundaries and reset if necessary
        if (currPos.x < 0)
        {
            currPos.x = 0;
        }

        if (currPos.x > appWidthLimit)
        {
            currPos.x = appWidthLimit;
        }

        if (currPos.y < 0)
        {
            currPos.y = 0;
        }

        if (currPos.y > appHeightLimit)
        {
            currPos.y = appHeightLimit;
        }

        //auto move to sort if places crossed
        if (WordSorterHandler.currList == null)
        {
            thisRect.anchoredPosition = currPos;
            inList = false;
            WordSorterHandler.currButton = null;

        } else
        {
            WordSorterHandler.singleton.putIntoList();
        }
    }
    #endregion
}
