using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UIWinEl
{
    public GameObject winLogin;
    public GameObject winSignUp;
    public GameObject winFind;
    public GameObject winOption;
    public GameObject winCreate;
    public GameObject winConnect;
    public GameObject winLog;
}

[Serializable]
public class UIBtnEl
{
    public Button btnLogin;
    public Button btnSingUp;
    public Button btnFind;
    public Button btnCreate;
    public Button btnConnect;
    public Button btnStore;
}

[Serializable]
public class UIFldEl
{
    public TMP_InputField fldLoginId;
    public TMP_InputField fldLoginPassword;
    public TMP_InputField fldSignUpId;
    public TMP_InputField fldSignUpPassword;
    public TMP_InputField fldSignUpPasswordCheck;
    public TMP_InputField fldSignUpEmail;
    public TMP_InputField fldFindEmail;
    public TMP_InputField fldCreateServerName;
    public TMP_InputField fldCreateAddress;
    public TMP_InputField fldConnectAddress;
}

[Serializable]
public class UIOtherEl
{
    public GameObject boardBlack;
    public GameObject findId;
    public GameObject findPassword;
    public TextMeshProUGUI log;
}

[Serializable]
public class UIOptionEl
{
    public GameObject bgm;
    public GameObject sfx;
    public GameObject keyForward;
    public GameObject keyBackward;
    public GameObject keyRight;
    public GameObject keyLeft;
}

public class UIManager : MonoBehaviour
{
    private delegate void ResetDelegate();

    private string findType = "ID";

    private ILobbyCommand _commandWindow;
    private ILobbyCommand _commandLogin;
    private ILobbyCommand _commandCheck;
    private ILobbyCommand _commandSignUp;
    private ILobbyCommand _commandFindButton;
    private ILobbyCommand _commandFind;
    private ILobbyCommand _commandOption;
    private ILobbyCommand _commandCreate;
    private ILobbyCommand _commandConnect;
    private ILobbyCommand _commandSoundOption;
    private ILobbyCommand _commandKeyOption;

    private Coroutine loginCoroutine;
    private Coroutine checkCoroutine;
    private Coroutine signUpCoroutine;
    private Coroutine findCoroutine;
    private Coroutine createCoroutine;
    private Coroutine connectCoroutine;
    private Coroutine setKeyCoroutine;

    private KeyCode saveForward;
    private KeyCode saveBackward;
    private KeyCode saveRight;
    private KeyCode saveLeft;

    [SerializeField] private UIWinEl winEl;
    [SerializeField] private UIBtnEl btnEl;
    [SerializeField] private UIFldEl fldEl;
    [SerializeField] private UIOtherEl otherEl;
    [SerializeField] private UIOptionEl optionEl;

    private readonly float ENABLE_VALUE = 1;
    private readonly float DISABLE_VALUE = 0.8f;

    private void Start()
    {
        InitializationInstances();
        ExecuteResetDelegate();
    }

    private void InitializationInstances()
    {
        _commandWindow = new WindowCommand();
        _commandLogin = new LoginCommand(fldEl.fldLoginId, fldEl.fldLoginPassword, StartLoginCoroutine);
        _commandCheck = new CheckCommand(fldEl.fldSignUpId, StartCheckCoroutine);
        _commandSignUp = new SignUpCommand(fldEl.fldSignUpId, fldEl.fldSignUpPassword, fldEl.fldSignUpPasswordCheck, fldEl.fldSignUpEmail, StartSignUpCoroutine);
        _commandFindButton = new FindButtonCommand(otherEl.findId, otherEl.findPassword, SetFindType);
        _commandFind = new FindCommand(fldEl.fldFindEmail, findType, StartFindCoroutine);
        _commandOption = new OptionCommand(optionEl.bgm, optionEl.sfx, UpdateKey);
        _commandCreate = new CreateCommand(fldEl.fldCreateServerName, fldEl.fldCreateAddress, StartCreateCoroutine);
        _commandConnect = new ConnectCommand(fldEl.fldConnectAddress, StartConnectCoroutine);
        _commandSoundOption = new OptionSoundCommand(optionEl.bgm, optionEl.sfx, SetSoundValue);
        _commandKeyOption = new OptionKeyCommand(StartSetKeyCoroutine);
    }

    private void ExecuteResetDelegate()
    {
        ResetDelegate resetDel = ResetField;
        resetDel += ResetButton;
        resetDel += ResetText;
        resetDel += ResetSlider;

        resetDel.Invoke();
    }

    private void ResetField()
    {
        fldEl.fldLoginId.text = string.Empty;
        fldEl.fldLoginPassword.text = string.Empty;
        fldEl.fldSignUpId.text = string.Empty;
        fldEl.fldSignUpPassword.text = string.Empty;
        fldEl.fldSignUpPasswordCheck.text = string.Empty;
        fldEl.fldSignUpEmail.text = string.Empty;
        fldEl.fldFindEmail.text = string.Empty;
        fldEl.fldCreateServerName.text = string.Empty;
        fldEl.fldCreateAddress.text = string.Empty;
        fldEl.fldConnectAddress.text = string.Empty;
    }

    private void ResetButton()
    {
        CanvasGroup findId = otherEl.findId.GetComponent<CanvasGroup>();
        CanvasGroup findPassword = otherEl.findPassword.GetComponent<CanvasGroup>();

        findId.alpha = ENABLE_VALUE;
        findPassword.alpha = DISABLE_VALUE;

        findType = "ID";
    }

    private void ResetText()
    {
        TextMeshProUGUI forward = optionEl.keyForward.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI backward = optionEl.keyBackward.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI right = optionEl.keyRight.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI left = optionEl.keyLeft.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        forward.text = GameManager.Instance.keyBindings["Forward"].ToString();
        backward.text = GameManager.Instance.keyBindings["Backward"].ToString();
        right.text = GameManager.Instance.keyBindings["Right"].ToString();
        left.text = GameManager.Instance.keyBindings["Left"].ToString();
    }

    private void ResetSlider()
    {
        Slider bgm = optionEl.bgm.GetComponent<Slider>();
        Slider sfx = optionEl.sfx.GetComponent<Slider>();

        bgm.value = GameManager.Instance.bgm;
        sfx.value = GameManager.Instance.sfx;
    }

    #region Button
    public void ButtonLogin() => WindowExecute(winEl.winLogin);
    public void ButtonSingUp() => WindowExecute(winEl.winSignUp);
    public void ButtonFind() => WindowExecute(winEl.winFind);
    public void ButtonOption() => WindowExecute(winEl.winOption);
    public void ButtonCreate() => WindowExecute(winEl.winCreate);
    public void ButtonConnect() => WindowExecute(winEl.winConnect);
    public void ButtonStore() => Application.OpenURL("https://www.youtube.com");
    public void ButtonExit() => Application.Quit();
    public void ButtonLog() => _commandWindow.Execute(winEl.winLog);
    public void ButtonLoginConfirm() => _commandLogin.Execute(winEl.winLog);
    public void ButtonCheckConfirm() => _commandCheck.Execute(winEl.winLog);
    public void ButtonFindId() => _commandFindButton.Execute(otherEl.findId);
    public void ButtonFindPassword() => _commandFindButton.Execute(otherEl.findPassword);
    public void ButtonFindConfirm() => _commandFind.Execute(winEl.winLog);
    public void ButtonSignUpConfirm() => _commandSignUp.Execute(winEl.winLog);
    public void ButtonOptionConfirm() => _commandOption.Execute(winEl.winLog);
    public void ButtonCreateConfirm() => _commandCreate.Execute(winEl.winLog);
    public void ButtonConnectConfirm() => _commandConnect.Execute(winEl.winLog);
    public void ButtonSetKeyForward() => _commandKeyOption.Execute(optionEl.keyForward);
    public void ButtonSetKeyBackward() => _commandKeyOption.Execute(optionEl.keyBackward);
    public void ButtonSetKeyRight() => _commandKeyOption.Execute(optionEl.keyRight);
    public void ButtonSetKeyLeft() => _commandKeyOption.Execute(optionEl.keyLeft);
    #endregion

    #region Slider
    public void SliderBGM() => _commandSoundOption.Execute(optionEl.bgm);
    public void SliderSFX() => _commandSoundOption.Execute(optionEl.sfx);
    #endregion

    private void WindowExecute(GameObject window)
    {
        ExecuteResetDelegate();
        otherEl.boardBlack.SetActive(!otherEl.boardBlack.activeSelf);
        _commandWindow.Execute(window);
    }

    private void SetFindType(string value)
    {
        findType = value;
    }

    private void SetSoundValue(string type, float value)
    {
        switch (type)
        {
            case "BGM": optionEl.bgm.GetComponent<Slider>().value = value; break;
            case "SFX": optionEl.sfx.GetComponent<Slider>().value = value; break;
        }
    }

    private void UpdateKey()
    {
        GameManager.Instance.SetKey(saveForward, saveBackward, saveRight, saveLeft);

        Debug.Log($"Setup Ok");

        otherEl.log.text = $"Setup Ok";
        _commandWindow.Execute(winEl.winLog);
    }
    #region Coroutine

    #region Key
    private void StartSetKeyCoroutine(GameObject gameObject)
    {
        if (setKeyCoroutine != null) { setKeyCoroutine = null; }

        setKeyCoroutine = StartCoroutine(SetKeyCoroutine(gameObject));
    }

    private IEnumerator SetKeyCoroutine(GameObject gameObject)
    {
        do
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(KeyCode.Mouse0)) { setKeyCoroutine = null; }
                else if (Input.GetKeyDown(keyCode))
                {
                    if (optionEl.keyForward == gameObject) { saveForward = keyCode; }
                    else if (optionEl.keyBackward == gameObject) { saveBackward = keyCode; }
                    else if (optionEl.keyRight == gameObject) { saveRight = keyCode; }
                    else if (optionEl.keyLeft == gameObject) { saveLeft = keyCode; }

                    TextMeshProUGUI key = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    key.text = keyCode.ToString();

                    setKeyCoroutine = null;
                }
            }

            yield return null;
        } while (setKeyCoroutine != null);
    }
    #endregion

    #region Login
    private void StartLoginCoroutine(string id, string password)
    {
        if (loginCoroutine != null) { return; }

        loginCoroutine = StartCoroutine(LoginCoroutine(id, password));
    }

    private IEnumerator LoginCoroutine(string id, string password)
    {
        yield return null;

        Debug.Log($"ID : {id}, Password : {password}");

        otherEl.log.text = $"ID : {id}, Password : {password}";
        _commandWindow.Execute(winEl.winLog);

        loginCoroutine = null;
    }
    #endregion

    #region Check
    private void StartCheckCoroutine(string id)
    {
        if (checkCoroutine != null) { return; }

        checkCoroutine = StartCoroutine(CheckCoroutine(id));
    }

    private IEnumerator CheckCoroutine(string id)
    {
        yield return null;

        Debug.Log($"ID : {id}");

        otherEl.log.text = $"ID : {id}";
        _commandWindow.Execute(winEl.winLog);

        checkCoroutine = null;
    }
    #endregion

    #region SignUp
    private void StartSignUpCoroutine(string id, string password, string passwordCheck, string email)
    {
        if (signUpCoroutine != null) { return; }

        signUpCoroutine = StartCoroutine(SignUpCoroutine(id, password, passwordCheck, email));
    }

    private IEnumerator SignUpCoroutine(string id, string password, string passwordCheck, string email)
    {
        yield return null;

        Debug.Log($"ID : {id}, Password : {password}, Check : {passwordCheck}, Email : {email}");

        otherEl.log.text = $"ID : {id}, Password : {password}, Check : {passwordCheck}, Email : {email}";
        _commandWindow.Execute(winEl.winLog);

        signUpCoroutine = null;
    }
    #endregion

    #region Find
    private void StartFindCoroutine(string email)
    {
        if (findCoroutine != null) { return; }

        if (findType == "ID") { findCoroutine = StartCoroutine(FindIdCoroutine(email)); }
        else { findCoroutine = StartCoroutine(FindPasswordCoroutine(email)); }
    }

    private IEnumerator FindIdCoroutine(string email)
    {
        yield return null;

        Debug.Log($"Find ID Email : {email}");

        otherEl.log.text = $"Find ID Email : {email}";
        _commandWindow.Execute(winEl.winLog);

        findCoroutine = null;
    }

    private IEnumerator FindPasswordCoroutine(string email)
    {
        yield return null;

        Debug.Log($"Find Password Email : {email}");

        otherEl.log.text = $"Find Password Email : {email}";
        _commandWindow.Execute(winEl.winLog);

        findCoroutine = null;
    }
    #endregion

    #region Create
    private void StartCreateCoroutine(string serverName, string address)
    {
        if (createCoroutine != null) { return; }

        createCoroutine = StartCoroutine(CreateCoroutine(serverName, address));
    }

    private IEnumerator CreateCoroutine(string serverName, string address)
    {
        yield return null;

        Debug.Log($"ServerName : {serverName}, Address : {address}");

        otherEl.log.text = $"ServerName : {serverName}, Address : {address}";
        _commandWindow.Execute(winEl.winLog);

        createCoroutine = null;
    }
    #endregion

    #region Connect
    private void StartConnectCoroutine(string address)
    {
        if (connectCoroutine != null) { return; }

        connectCoroutine = StartCoroutine(ConnectCoroutine(address));
    }

    private IEnumerator ConnectCoroutine(string address)
    {
        yield return null;

        Debug.Log($"Address : {address}");

        otherEl.log.text = $"Address : {address}";
        _commandWindow.Execute(winEl.winLog);

        connectCoroutine = null;
    }
    #endregion

    #endregion
}

public interface ILobbyCommand
{
    public void Execute(GameObject windows);
}

public class WindowCommand : ILobbyCommand
{
    public void Execute(GameObject window)
    {
        window.SetActive(!window.activeSelf);
    }
}

public class LoginCommand : ILobbyCommand
{
    private TMP_InputField _loginId;
    private TMP_InputField _loginPassword;
    private Action<string, string> _loginAction;

    public LoginCommand(
        TMP_InputField loginId,
        TMP_InputField loginPassword,
        Action<string, string> loginAction)
    {
        _loginId = loginId;
        _loginPassword = loginPassword;
        _loginAction = loginAction;
    }

    public void Execute(GameObject window)
    {
        string id = _loginId.text;
        string password = _loginPassword.text;

        _loginAction(id, password);
    }
}

public class CheckCommand : ILobbyCommand
{
    private TMP_InputField _signUpId;
    private Action<string> _signUpAction;

    public CheckCommand(
        TMP_InputField signUpId,
        Action<string> signUpAction
        )
    {
        _signUpId = signUpId;
        _signUpAction = signUpAction;
    }

    public void Execute(GameObject window)
    {
        string id = _signUpId.text;

        _signUpAction(id);
    }
}

public class SignUpCommand : ILobbyCommand
{
    private TMP_InputField _signUpId;
    private TMP_InputField _signUpPassword;
    private TMP_InputField _signUpIdPasswordCheck;
    private TMP_InputField _signUpEmail;
    private Action<string, string, string, string> _signUpAction;

    public SignUpCommand(
        TMP_InputField signUpId,
        TMP_InputField signUpPassword,
        TMP_InputField signUpIdPasswordCheck,
        TMP_InputField signUpEmail,
        Action<string, string, string, string> signUpAction
        )
    {
        _signUpId = signUpId;
        _signUpPassword = signUpPassword;
        _signUpIdPasswordCheck = signUpIdPasswordCheck;
        _signUpEmail = signUpEmail;
        _signUpAction = signUpAction;
    }

    public void Execute(GameObject window)
    {
        string id = _signUpId.text;
        string password = _signUpPassword.text;
        string passwordCheck = _signUpIdPasswordCheck.text;
        string email = _signUpEmail.text;

        _signUpAction(id, password, passwordCheck, email);
    }
}

public class FindButtonCommand : ILobbyCommand
{
    private GameObject _findId;
    private GameObject _findPassword;
    Action<string> _findAction;

    private readonly float ENABLE_VALUE = 1;
    private readonly float DISABLE_VALUE = 0.8f;

    public FindButtonCommand(
        GameObject findId,
        GameObject findPassword,
        Action<string> findAction)
    {
        _findId = findId;
        _findPassword = findPassword;
        _findAction = findAction;
    }

    public void Execute(GameObject gameObject)
    {
        if (gameObject == _findId)
        {
            _findId.GetComponent<CanvasGroup>().alpha = ENABLE_VALUE;
            _findPassword.GetComponent<CanvasGroup>().alpha = DISABLE_VALUE;

            _findAction("ID");
        }
        else if (gameObject == _findPassword)
        {
            _findId.GetComponent<CanvasGroup>().alpha = DISABLE_VALUE;
            _findPassword.GetComponent<CanvasGroup>().alpha = ENABLE_VALUE;

            _findAction("Password");
        }
    }
}

public class FindCommand : ILobbyCommand
{
    private TMP_InputField _findEmail;
    private string _findType;
    private Action<string> _findAction;

    public FindCommand(
        TMP_InputField findEmail,
        string findType,
        Action<string> findAction
        )
    {
        _findEmail = findEmail;
        _findType = findType;
        _findAction = findAction;

    }

    public void Execute(GameObject window)
    {
        string type = _findType;
        string email = _findEmail.text;

        _findAction(email);
    }
}

public class OptionCommand : ILobbyCommand
{
    private GameObject _bgm;
    private GameObject _sfx;
    private Action _keyAction;

    public OptionCommand(
        GameObject bgm,
        GameObject sfx,
        Action keyAction
        )
    {
        _bgm = bgm;
        _sfx = sfx;
        _keyAction = keyAction;
    }

    public void Execute(GameObject window)
    {
        float bgm = _bgm.GetComponent<Slider>().value;
        float sfx = _sfx.GetComponent<Slider>().value;

        GameManager.Instance.SetSound(bgm, sfx);

        _keyAction();
    }
}

public class CreateCommand : ILobbyCommand
{
    private TMP_InputField _createServerName;
    private TMP_InputField _createAddress;
    private Action<string, string> _createAction;

    public CreateCommand(
        TMP_InputField createServerName,
        TMP_InputField createAddress,
        Action<string, string> createAction
        )
    {
        _createServerName = createServerName;
        _createAddress = createAddress;
        _createAction = createAction;
    }

    public void Execute(GameObject window)
    {
        string serverName = _createServerName.text;
        string address = _createAddress.text;

        _createAction(serverName, address);
    }
}

public class ConnectCommand : ILobbyCommand
{
    private TMP_InputField _connectAddress;
    private Action<string> _connectAction;

    public ConnectCommand(
        TMP_InputField connectAddress,
        Action<string> connectAction
        )
    {
        _connectAddress = connectAddress;
        _connectAction = connectAction;
    }

    public void Execute(GameObject window)
    {
        string address = _connectAddress.text;

        _connectAction(address);
    }
}

public class OptionSoundCommand : ILobbyCommand
{
    private GameObject _bgm;
    private GameObject _sfx;
    private Action<string, float> _optionAction;

    public OptionSoundCommand(
        GameObject bgm,
        GameObject sfx,
        Action<string, float> optionAction
        )
    {
        _bgm = bgm;
        _sfx = sfx;
        _optionAction = optionAction;
    }

    public void Execute(GameObject gameObject)
    {
        float value = gameObject.GetComponent<Slider>().value;

        if (gameObject == _bgm)
        {
            _optionAction("BGM", value);
        }
        else if (gameObject == _sfx)
        {
            _optionAction("SFX", value);
        }
    }
}

public class OptionKeyCommand : ILobbyCommand
{
    private Action<GameObject> _optionAction;

    public OptionKeyCommand(
        Action<GameObject> optionAction
        )
    {
        _optionAction = optionAction;
    }

    public void Execute(GameObject gameObject)
    {
        _optionAction(gameObject);
    }
}