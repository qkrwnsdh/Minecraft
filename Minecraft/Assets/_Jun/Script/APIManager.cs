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

        log.text = GetResponseLog(api, (int)request.responseCode);
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

    private string GetResponseLog(API api, int responseCode)
    {
        Dictionary<int, string> responseMessages = GetApiResponseMessages(api);
        if (responseMessages.TryGetValue(responseCode, out string message))
        {
            return message;
        }

        return "알 수 없는 응답 코드입니다.";
    }

    private Dictionary<int, string> GetApiResponseMessages(API api)
    {
        switch (api)
        {
            case API.ACCOUNT_JOIN:
                return new Dictionary<int, string>
            {
                { 200, "회원가입 하셨습니다." },
                { 400, "정보를 확인해 주십시오." }
            };

            case API.LOGIN:
                return new Dictionary<int, string>
            {
                { 200, "로그인 하셨습니다." },
                { 401, "정보를 확인해 주십시오." }
            };

            case API.ACCOUNT_FIND_PASSWORD:
                return new Dictionary<int, string>
            {
                { 200, "이메일로 임시 비밀번호를 보냈습니다." },
                { 400, "정보를 확인해 주십시오." }
            };

            case API.ACCOUNT_FIND_USERNAME:
                return new Dictionary<int, string>
            {
                { 200, "아이디 찾기를 하셨습니다." },
                { 400, "정보를 확인해 주십시오." }
            };

            case API.ACCOUNT_SET_PASSWORD:
                return new Dictionary<int, string>
            {
                { 200, "비밀번호가 재설정 되셨습니다." },
                { 400, "정보를 확인해 주십시오." },
                { 403, "권한이 없습니다." }
            };

            case API.ACCOUNT_USERNAME_CHECK:
                return new Dictionary<int, string>
            {
                { 200, "사용가능한 아이디 입니다." },
                { 400, "이미 존재하는 아이디 입니다." }
            };

            case API.ACCOUNT_EMAIL_CHECK:
                return new Dictionary<int, string>
            {
                { 200, "사용가능한 이메일 입니다." },
                { 400, "이미 존재하는 이메일 입니다." }
            };

            default:
                return new Dictionary<int, string>
            {
                { 0, "알 수 없는 오류가 발생했습니다." }
            };
        }
    }
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