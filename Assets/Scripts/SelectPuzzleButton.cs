using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SelectPuzzleButton : MonoBehaviour
{
    public GameData gameData;
    public GameLevelData levelData;
    public Text categoryText;
    public Image progressBarFilling;

    private string gameSceneName = "GameScene";
    private bool _levelLocked;
    enum topicId
    {
        Animals=1,
        Food=2,
        Sport=3,
        Hobbies=4,
        Work=5,
        Countries=6
    }

    private void Start()
    {
        _levelLocked = false;
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        UpdateButtonInformation();
        if (_levelLocked)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    private void OnEnable()
    {
        AdManager.OnInterstitialAdsClosed += InterstitialAdsClosed;
    }

    private void OnDisable()
    {
        AdManager.OnInterstitialAdsClosed -= InterstitialAdsClosed;
    }

    private void InterstitialAdsClosed()
    {
        
    }

    private void UpdateButtonInformation()
    {
        var currentIndex = -1;
        var totalBoards = 0;
        foreach (var data in levelData.data) 
        {
            if (data.categoryName == gameObject.name)
            {
                currentIndex = DataSaver.ReadCategoryCurrentIndexValues(gameObject.name);
                totalBoards = data.boardData.Count;
                if (levelData.data[0].categoryName == gameObject.name && currentIndex < 0)
                {
                    DataSaver.SaveCategoryData(levelData.data[0].categoryName, 0);
                    currentIndex = DataSaver.ReadCategoryCurrentIndexValues(gameObject.name);
                    totalBoards = data.boardData.Count;
                }
            }
        }

        if (currentIndex == -1) 
            _levelLocked = true;

        categoryText.text = _levelLocked ? string.Empty : (currentIndex.ToString() + "/" + totalBoards.ToString());
        progressBarFilling.fillAmount = 
            (currentIndex > 0 && totalBoards > 0) ? ((float)currentIndex / (float)totalBoards) : 0f;
    }


    private void OnButtonClick()
    {
        gameData.selectedCategoryName = gameObject.name;
        int topicId = GetTopicIdNumber(gameObject.name.ToString());
        ApiClient.Instance.topicId = topicId;
        //AdManager.Instance.ShowInterstitialAd();
        SceneManager.LoadScene(gameSceneName);
    }

    static int GetTopicIdNumber(string topicName)
    {
        if (Enum.TryParse(typeof(topicId), topicName, true, out var result))
        {
            return (int)result;
        }
        else
        {
            throw new ArgumentException("Invalid topic name");
        }
    }
}
