using TMPro;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WordPickerHandler : MonoBehaviour
{
    public static WordPickerHandler singleton;
    private string filePath;
    private float appWidth, appHeight;
    public bool canClick;
    private bool isCorrect;
    public TextMeshProUGUI wordDisplay;
    private List<string> wordList, words;
    public List<GameObject> buttonList;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        appWidth = BackendHandler.singleton.appWidth;
        appHeight = BackendHandler.singleton.appHeight;
        filePath = Application.dataPath + "/" + "WordPickerData.txt";
        isCorrect = false;
        readFile();
    }

    private void readFile()
    {
        wordList = new List<string>();
        words = new List<string>();
        StreamReader reader = new StreamReader(filePath);
        string data = reader.ReadToEnd();
        string[] lines = data.Split('|');
        foreach (var l in lines)
        {
            wordList.Add(l);
        }
        pickWords();
    }

    private void pickWords()
    {
        System.Random ran = new System.Random();
        int index = ran.Next(1, wordList.Count);
        buttonSpawn();
        spawnWords(wordList[index]);
        wordList.RemoveAt(index);
    }

    private void spawnWords(string line)
    {
        words.Clear();
        string[] s = line.Split(',');
        foreach(string w in s)
        {
            words.Add(w);
        }

        wordDisplay.text = words[0];
        words.RemoveAt(0);

        for (int i = 0; i < buttonList.Count; i++)
        {
            System.Random ran = new System.Random();
            int index = ran.Next(0, words.Count);
            buttonList[i].GetComponent<ButtonHandler>().textDisplay.text = words[index];
            words.RemoveAt(index);
        }

        canClick = true;
    }

    private void buttonSpawn()
    {
        float rightside = appWidth * 0.5f;
        float highest = appHeight * 0.7f;

        for (int i = 0; i < buttonList.Count; i++)
        {
            float xVariance = Mathf.RoundToInt(Random.Range(0, appWidth * 0.3f));
            float yVariance = Mathf.RoundToInt(Random.Range(0, appHeight * 0.1f));
            var thisRect = buttonList[i].GetComponent<RectTransform>();
            buttonList[i].GetComponent<ButtonHandler>().inList = false;

            switch (i)
            {
                case 0:
                    thisRect.anchoredPosition = new Vector2(0 + xVariance, highest - yVariance);
                    break;

                case 1:
                    thisRect.anchoredPosition = new Vector2(0 + xVariance, highest * 0.7f - yVariance);
                    break;

                case 2:
                    thisRect.anchoredPosition = new Vector2(0 + xVariance, highest * 0.4f - yVariance);
                    break;

                case 3:
                    thisRect.anchoredPosition = new Vector2(0 + xVariance, highest * 0.1f - yVariance);
                    break;

                case 4:
                    thisRect.anchoredPosition = new Vector2(rightside + xVariance, highest - yVariance);
                    break;

                case 5:
                    thisRect.anchoredPosition = new Vector2(rightside + xVariance, highest * 0.7f - yVariance);
                    break;

                case 6:
                    thisRect.anchoredPosition = new Vector2(rightside + xVariance, highest * 0.4f - yVariance);
                    break;

                case 7:
                    thisRect.anchoredPosition = new Vector2(rightside + xVariance, highest * 0.1f - yVariance);
                    break;
            }

            //shift back to edge if crossed boundaries
            if (thisRect.anchoredPosition.y < 0)
            {
                thisRect.anchoredPosition = new Vector2(thisRect.anchoredPosition.x, 0);
            }

            if (thisRect.anchoredPosition.x > appWidth - thisRect.rect.width)
            {
                thisRect.anchoredPosition = new Vector2(appWidth - thisRect.rect.width, thisRect.anchoredPosition.y);
            }
        }
    }

    public void checkWord(string word, GameObject button)
    {
        canClick = false;
        if (word == wordDisplay.text)
        {
            button.GetComponent<Image>().color = Color.green;
            isCorrect = true;
        } else
        {
            button.GetComponent<Image>().color = Color.red;
        }
        StartCoroutine(finishDelay(1f));
    }


    private IEnumerator finishDelay (float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        foreach (var b in buttonList)
        {
            b.GetComponent<Image>().color = Color.white;
        }

        if (isCorrect)
        {
            pickWords();
            isCorrect = false;
        }
        canClick = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("WordMatcher");
        }
    }
}
