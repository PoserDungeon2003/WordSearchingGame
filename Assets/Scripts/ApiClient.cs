using Codice.CM.Client.Differences;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public interface IWordSearchingGame
{
    Task<WordDataList> GetWordsAsync(int topicId, int difficultyId);
}

[Serializable]
public class WordData
{
    public string word;
}

[Serializable]
public class WordDataList
{
    public List<WordData> words;
}

public class ApiClient : IWordSearchingGame
{
    private readonly string _baseUrl = "https://localhost:7111";
    public ApiClient() { }
    public ApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public async Task<WordDataList> GetWordsAsync(int topicId, int difficultyId)
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
        words.words.ForEach(word =>
        {
            word.word = word.word.ToUpper();
            Debug.Log("Word " + word.word);
        });

        return words;

    }
}
