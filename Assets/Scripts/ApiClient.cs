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

[Serializable]
public class UserData
{
    public int userId;
    public string username;
    public string passwordHash;
    public string email;
    public DateTime dateJoined;
}

[Serializable]
public class GoogleUserLoginRQ
{
    public string username;
}

[Serializable]
public class UserLoginRS
{
    public int userId;
    public string username;
    public string email;
}

public class ApiClient : MonoBehaviour
{
    public int topicId;
    public int difficultyId;
    public static ApiClient Instance;
    public UserLoginRS _userLoginRS;
    public string accessToken;
    public int currentIndex;

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

    public async Task<WordDataList> GetWordsAsync(int difficultyId)
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
        });

        return words;

    }

    public IEnumerator GoogleSignupUser(UserData userData)
    {
        Debug.Log("Signing up user...");
        string url = $"{_baseUrl}/api/User";
        string jsonData = JsonUtility.ToJson(userData);
        Debug.Log("Json Data: " + jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            if (request.responseCode == 500)
            {
                StartCoroutine(GoogleLoginUser(new GoogleUserLoginRQ { username = userData.username }));
            }
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }

    public IEnumerator GoogleLoginUser(GoogleUserLoginRQ userLoginRQ)
    {
        string url = $"{_baseUrl}/api/User/login";
        string jsonData = JsonUtility.ToJson(userLoginRQ);
        Debug.Log("GoogleLoginUser: " + jsonData);

        using UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            yield return Task.Yield();
        }

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
            throw new HttpRequestException(request.error);
        }

        Debug.Log("response" + request.downloadHandler.text);

        string response = request.downloadHandler.text;
        UserLoginRS userLoginRS = JsonUtility.FromJson<UserLoginRS>(response);

        _userLoginRS = userLoginRS;
        Debug.Log("User Login Response: " + _userLoginRS.username);
        yield return userLoginRS;
    }
}
