using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordSorterHandler : MonoBehaviour
{
    public static WordSorterHandler singleton;
    public static GameObject currButton, currList;
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
        spawnWords(wordList[index]);
        wordList.RemoveAt(index);
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