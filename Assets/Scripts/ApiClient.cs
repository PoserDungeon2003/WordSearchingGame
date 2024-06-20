using i5.Toolkit.Core.RocketChatClient;
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

[Serializable]
public class UserProgressRequest
{
    public int userId;
    public string difficultyLevel;
    public string topicName;
    public int levelNumber;
    public bool completed;
    public string completionTime;
}

[Serializable]
public class UserProgressResponse
{
    public int userId;
    public int levelId;
    public bool completed;
    public string completionTime;
}

public class ApiClient : MonoBehaviour
{
    [HideInInspector]
    public int topicId;
    [HideInInspector]
    public int difficultyId;
    public static ApiClient Instance;
    private UserLoginRS _userLoginRS;
    private string accessToken;
    [HideInInspector]
    public int currentIndex;
    [HideInInspector]
    public WordDataList words;
    [HideInInspector]
    public UserProgressResponse UserProgressResponse;

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

    private void Update()
    {
        //if (ApiClient.Instance.GetAccessToken() != null)
        //{
        //    GoogleLoginButton.SetActive(false);
        //    LogoutButton.SetActive(true);
        //}
        //else
        //{
        //    GoogleLoginButton.SetActive(true);
        //    LogoutButton.SetActive(false);
        //}
    }

    private readonly string _baseUrl = "https://localhost:7111";

    public async Task<WordDataList> GetWordsAsync(int difficultyId)
    {
        this.difficultyId = difficultyId;
        string url = $"{_baseUrl}/api/Word/{topicId}/{this.difficultyId}";
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

        Debug.Log("response " + request.downloadHandler.text);

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
        this.words = words;
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
            var response = request.downloadHandler.text;
            UserLoginRS userLoginRS = JsonUtility.FromJson<UserLoginRS>(response);

            _userLoginRS = userLoginRS;
            Debug.Log("Sign up completed: " + _userLoginRS.username);
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
        Debug.Log("Login success: " + _userLoginRS.username);
        yield return userLoginRS;
    }

    public async void SendUserProgress(string topicName, int currentIndex, bool completed)
    {
        string difficulty;

        switch(difficultyId)
        {
            case 1:
                difficulty = "Easy";
                break;
            case 2:
                difficulty = "Medium";
                break;
            case 3:
                difficulty = "Hard";
                break;
            default:
                difficulty = "Easy";
                break;
        }
        // Create a new UserProgressRequest object
        UserProgressRequest requestObject = new UserProgressRequest()
        {
            userId = _userLoginRS.userId,
            difficultyLevel = difficulty,
            topicName = topicName,
            levelNumber = currentIndex + 1,
            completed = completed,
            completionTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };
        // Convert the requestObject to JSON format
        string jsonRequestBody = JsonUtility.ToJson(requestObject);
        Debug.Log("Send user progress: " + jsonRequestBody);

        // Define the URL to which the POST request will be sent
        string url = $"{_baseUrl}/api/UserProgress/v2"; // Replace with your actual API endpoint

        // Create a UnityWebRequest POST object
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        // Set the request headers
        request.SetRequestHeader("Content-Type", "application/json");

        // Attach the JSON data to the request
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Send the request
        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        Debug.Log("Response code: " + request.responseCode);
        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error sending POST request: " + request.error.ToString());
        }
        else
        {
            Debug.Log("POST request successful!");
            var response = request.downloadHandler.text;
            UserProgressResponse userProgressResponse = JsonUtility.FromJson<UserProgressResponse>(response);
            Debug.Log("Response: " + userProgressResponse.completionTime);
        }
    }

    public string GetAccessToken()
    {
        return accessToken;
    }

    public void SetAccessToken(string token)
    {
        accessToken = token;
    }
}
