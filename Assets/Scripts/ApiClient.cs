using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class WordData
{
    public string word;
    public bool Found;
}

[Serializable]
public class WordDataList
{
    public List<WordData> words;
}

public class ApiClient : MonoBehaviour
{
    public int topicId;
    public int difficultyId;
    public static ApiClient Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    private readonly string _baseUrl = "https://localhost:7111";

    public async Task<WordDataList> GetWordsAsync()
    {
        string url = $"{_baseUrl}/api/Word/{topicId}/{difficultyId}";
        using UnityWebRequest request = UnityWebRequest.Get(url);

        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
            throw new HttpRequestException(request.error);
        }

        Debug.Log("response" + request.downloadHandler.text);

        string response = request.downloadHandler.text;
        string wrappedJson = $"{{\"words\":{response}}}";
        WordDataList words = JsonUtility.FromJson<WordDataList>(wrappedJson);
        if (words.words.Count > 5)
        {
            words.words = words.words.GetRange(0, 5);
        }
        words.words.ForEach(word =>
        {
            word.word = word.word.ToUpper();
            Debug.Log("Word " + word.word);
        });

        return words;

    }
}
