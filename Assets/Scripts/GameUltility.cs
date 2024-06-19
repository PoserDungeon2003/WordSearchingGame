using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SelectPuzzleButton;

public enum topicId
{
    Animals = 1,
    Food = 2,
    Sport = 3,
    Hobbies = 4,
    Work = 5,
    Countries = 6
}

public class GameUltility : MonoBehaviour
{
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);

    public void ExitApplication() => Application.Quit();

    public void HideBannerAds() => AdManager.Instance.HideBanner();

    public void MuteToggleBackgroundMusic() => SoundManager.instance.ToggleBackgroundMusic();

    public void MuteToggleSoundFX() => SoundManager.instance.ToggleSoundFX();

    public void ShowPlayerPref() => Debug.Log(PlayerPrefs.GetInt("PlayerScore"));

    public static int GetTopicIdNumber(string topicName)
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
