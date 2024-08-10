using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public enum API
{
    ACCOUNT_JOIN,
    LOGIN,
    ACCOUNT_FIND_PASSWORD,
    ACCOUNT_FIND_USERNAME,
    ACCOUNT_SET_PASSWORD,
    ACCOUNT_USERNAME_CHECK,
    ACCOUNT_EMAIL_CHECK
}

public class APIManager : MonoBehaviour
{
    #region Singleton and Awake()
    public static APIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private readonly Dictionary<API, string> endpoints = new Dictionary<API, string>
    {
        { API.ACCOUNT_JOIN, "http://localhost:8080/api/account/v1/join" },
        { API.LOGIN, "http://localhost:8080/login" },
        { API.ACCOUNT_FIND_PASSWORD, "http://localhost:8080/api/account/v1/find_password" },
        { API.ACCOUNT_FIND_USERNAME, "http://localhost:8080/api/account/v1/find_username" },
        { API.ACCOUNT_SET_PASSWORD, "http://localhost:8080/api/account/v1/set_password" },
        { API.ACCOUNT_USERNAME_CHECK, "http://localhost:8080/api/account/v1/username_check" },
        { API.ACCOUNT_EMAIL_CHECK, "http://localhost:8080/api/account/v1/email_check" }
    };

    #region POST
    private IEnumerator SendRequest(API api, TextMeshProUGUI log, object requestData)
    {
        string jsonData = JsonUtility.ToJson(requestData);
        string url = endpoints[api];

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            log.text = $"{api} successful!";
        }
        else
        {
            log.text = $"{api} failed: " + request.error;
            Debug.Log($"{api} failed: " + request.error);
        }
    }

    public IEnumerator AccountJoin(TextMeshProUGUI log, string username, string password, string email)
    {
        AccountJoin accountJoin = new AccountJoin(username, password, email);
        return SendRequest(API.ACCOUNT_JOIN, log, accountJoin);
    }

    public IEnumerator Login(TextMeshProUGUI log, string username, string password)
    {
        Login login = new Login(username, password);
        return SendRequest(API.LOGIN, log, login);
    }

    public IEnumerator AccountFindPassword(TextMeshProUGUI log, string username)
    {
        AccountFindPassword accountFindPassword = new AccountFindPassword(username);
        return SendRequest(API.ACCOUNT_FIND_PASSWORD, log, accountFindPassword);
    }

    public IEnumerator AccountFindUsername(TextMeshProUGUI log, string email)
    {
        AccountFindUsername accountFindUsername = new AccountFindUsername(email);
        return SendRequest(API.ACCOUNT_FIND_USERNAME, log, accountFindUsername);
    }

    public IEnumerator AccountSetPassword(TextMeshProUGUI log, string password, string new_password)
    {
        AccountSetPassword accountSetPassword = new AccountSetPassword(password, new_password);
        return SendRequest(API.ACCOUNT_SET_PASSWORD, log, accountSetPassword);
    }

    public IEnumerator AccountUsernameCheck(TextMeshProUGUI log, string username)
    {
        AccountUsernameCheck accountUsernameCheck = new AccountUsernameCheck(username);
        return SendRequest(API.ACCOUNT_USERNAME_CHECK, log, accountUsernameCheck);
    }

    public IEnumerator AccountEmailCheck(TextMeshProUGUI log, string email)
    {
        AccountEmailCheck accountEmailCheck = new AccountEmailCheck(email);
        return SendRequest(API.ACCOUNT_EMAIL_CHECK, log, accountEmailCheck);
    }
    #endregion
}

[Serializable]
public class AccountJoin
{
    public string username;
    public string password;
    public string email;

    public AccountJoin(string username, string password, string email)
    {
        this.username = username;
        this.password = password;
        this.email = email;
    }
}

[Serializable]
public class Login
{
    public string username;
    public string password;

    public Login(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

[Serializable]
public class AccountFindPassword
{
    public string username;

    public AccountFindPassword(string username)
    {
        this.username = username;
    }
}

[Serializable]
public class AccountFindUsername
{
    public string email;

    public AccountFindUsername(string email)
    {
        this.email = email;
    }
}

[Serializable]
public class AccountSetPassword
{
    public string password;
    public string new_password;

    public AccountSetPassword(string password, string new_password)
    {
        this.password = password;
        this.new_password = new_password;
    }
}

[Serializable]
public class AccountUsernameCheck
{
    public string username;

    public AccountUsernameCheck(string username)
    {
        this.username = username;
    }
}

[Serializable]
public class AccountEmailCheck
{
    public string email;

    public AccountEmailCheck(string email)
    {
        this.email = email;
    }
}