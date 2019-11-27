using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WordSorterHandler : MonoBehaviour
{
    public static WordSorterHandler singleton;
    public static GameObject currButton, currList;
    public static int roundNum;
    public Camera mainCam;
    public Canvas mainCanvas;
    public GameObject leftColumn, rightColumn;
    public List<string> wordList;
    public List<GameObject> buttonList;
    [HideInInspector] public float appWidth, appHeight;
    private string filePath;

    private void Awake()
    {
        singleton = this;
        appWidth = 1080;
        appHeight = 1920;
    }

    private void Start()
    {
        currButton = null;
        currList = null;
        roundNum = 1;

        filePath = Application.dataPath + "/" + "wordData.txt";
        readFile();
    }

    private void readFile()
    {
        wordList = new List<string>();
        StreamReader reader = new StreamReader(filePath);
        string data = reader.ReadToEnd();
        string[] lines = data.Split('|');
        foreach (var l in lines)
        {
            wordList.Add(l);
        }

        chooseCategory();
    }

    private void chooseCategory()
    {
        System.Random ran = new System.Random();
        int index = ran.Next(1, wordList.Count);
        buttonSpawn();
        spawnWords(wordList[index]);
        wordList.RemoveAt(index);
    }

    private void buttonSpawn() //randomize starting placement
    {
        foreach (var b in buttonList)
        {
            b.transform.SetParent(mainCanvas.transform);
            b.transform.SetSiblingIndex(1);
            b.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            b.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        }
        
        float rightside = appWidth * 0.5f;
        float midpoint = appHeight * 0.45f;

        for (int i = 0; i < buttonList.Count; i++)
        {
            float xVariance = Mathf.RoundToInt(Random.Range(0, appWidth * 0.3f));
            float yVariance = Mathf.RoundToInt(Random.Range(0, appHeight * 0.1f));

            switch (i)
            {
                case 0:
                    buttonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0 + xVariance, midpoint - yVariance);
                    break;

                case 1:
                    buttonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0 + xVariance, midpoint * 0.7f - yVariance);
                    break;

                case 2:
                    buttonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0 + xVariance, midpoint * 0.4f - yVariance);
                    break;

                case 3:
                    buttonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0 + xVariance, midpoint * 0.1f - yVariance);
                    break;

                case 4:
                    buttonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(rightside + xVariance, midpoint - yVariance);
                    break;

                case 5:
                    buttonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(rightside + xVariance, midpoint * 0.7f - yVariance);
                    break;

                case 6:
                    buttonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(rightside + xVariance, midpoint * 0.4f - yVariance);
                    break;

                case 7:
                    buttonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(rightside + xVariance, midpoint * 0.1f - yVariance);
                    break;
            }

            //shift back to edge if crossed boundaries
            if (buttonList[i].GetComponent<RectTransform>().anchoredPosition.y < 0)
            {
                buttonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(buttonList[i].GetComponent<RectTransform>().anchoredPosition.x, 0);
            }

            if (buttonList[i].GetComponent<RectTransform>().anchoredPosition.x > appWidth - buttonList[i].GetComponent<RectTransform>().rect.width)
            {
                buttonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(appWidth - buttonList[i].GetComponent<RectTransform>().rect.width, buttonList[i].GetComponent<RectTransform>().anchoredPosition.y);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void spawnWords(string line)
    {
        string[] words = line.Split('\n');
        List<string> tempList = new List<string>();
        foreach (var w in words)
        {
            tempList.Add(w);
        }

        //put category labels
        string[] headers = tempList[0].Split(',');
        leftColumn.transform.parent.GetComponent<ListHander>().columnHeader.text = headers[0].Trim();
        rightColumn.transform.parent.GetComponent<ListHander>().columnHeader.text = headers[1].Trim();
        tempList.RemoveAt(0);

        //put words onto buttons
        for (int i = 0; i < buttonList.Count; i++)
        {
            System.Random ran = new System.Random();
            int index = ran.Next(0, tempList.Count - 1);
            string[] vars = tempList[index].Split(',');
            buttonList[i].GetComponent<ButtonHandler>().textDisplay.text = vars[0];
            buttonList[i].GetComponent<ButtonHandler>().type = vars[1].Trim();
            tempList.RemoveAt(index);
        }
    }

    public void putIntoList()
    {
        currButton.transform.SetParent(currList.transform.GetChild(0).transform);
        currButton.GetComponent<ButtonHandler>().inList = true;
        currButton = null;
        currList = null;
        if (ifAllInList())
        {
            checkAnswers();
        }
    }

    private void checkAnswers()
    {
        if (leftColumn.transform.parent.GetComponent<ListHander>().allCorrect() && rightColumn.transform.parent.GetComponent<ListHander>().allCorrect())
        {
            Debug.Log("all correct!");
            roundNum++;
            if (roundNum < 4)
            {
                chooseCategory();
            }

        } else
        {
            Debug.Log("try again");
        }
    }

    private bool ifAllInList()
    {
        foreach (var b in buttonList)
        {
            if (!b.GetComponent<ButtonHandler>().inList)
            {
                return false;
            }
        }
        return true;
    }
}