using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchingWord : MonoBehaviour
{
    public Text displayText;
    public Image crossLine;

    private string _word;
    void Start()
    {
        
    }
    void Update()
    {

    }
    private void OnEnable()
    {
        GameEvents.OnCorrectWord += CorrectWord;
    }
    private void OnDisable()
    {
        GameEvents.OnCorrectWord -= CorrectWord;
    }
    public void SetWord(string word)
    {
        _word = word;
        displayText.text = word;
    }
    private void CorrectWord(string word, List<int> squareIndexes)
    {
        if (_word == word)
        {
            crossLine.gameObject.SetActive(true);
        }
    }
}
