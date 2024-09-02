using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#region UI Element
[Serializable]
public class UIWindowElement
{
    public GameObject windowBlack;
    public GameObject windowLogin;
    public GameObject windowSignUp;
    public GameObject windowFind;
    public GameObject windowOption;
    public GameObject windowCreate;
    public GameObject windowConnect;
    public GameObject windowLog;
}

[Serializable]
public class UIButtonElement
{
    public Button buttonLoginConfirm;
    public Button buttonSingUpConfirm;
    public Button buttonSignUpIdCheck;
    public Button buttonSignUpEmailCheck;
    public Button buttonFindConfirm;
    public Button buttonFindId;
    public Button buttonFindPassword;
    public Button buttonOptionConfirm;
    public Button buttonCreateConfirm;
    public Button buttonConnectConfirm;
    public Button buttonStore;
}

[Serializable]
public class UIInputElement
{
    public TMP_InputField inputLoginId;
    public TMP_InputField inputLoginPassword;
    public TMP_InputField inputSignUpId;
    public TMP_InputField inputSignUpPassword;
    public TMP_InputField inputSignUpPasswordCheck;
    public TMP_InputField inputSignUpEmail;
    public TMP_InputField inputFindIdEmail;
    public TMP_InputField inputFindPasswordId;
    public TMP_InputField inputCreateServerName;
    public TMP_InputField inputCreateAddress;
    public TMP_InputField inputConnectAddress;
}

[Serializable]
public class UITextElement
{
    public TextMeshProUGUI textFind;
    public TextMeshProUGUI textLog;
    public TextMeshProUGUI textForward;
    public TextMeshProUGUI textBackward;
    public TextMeshProUGUI textRight;
    public TextMeshProUGUI textLeft;
}

[Serializable]
public class UISliderElement
{
    public Slider sliderBgm;
    public Slider sliderSfx;
}
#endregion

public class UIManager : MonoBehaviour
{
    private string findType = "ID";

    private UICommand _ConfirmLoginCommand;
    private UICommand _ConfirmSignUpCommand;
    private UICommand _ConfirmFindCommand;
    private UICommand _ConfirmOptionCommand;
    private UICommand _ConfirmCreateCommand;
    private UICommand _ConfirmConnectCommand;
    private UICommand _WindowEnableCommand;
    private UICommand _WindowDisableCommand;
    private UICommand _CancleBaseCommand;
    private UICommand _CancleFindCommand;
    private UICommand _CancleOptionCommand;
    private UICommand _CheckIdCommand;
    private UICommand _CheckEmailCommand;

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

    [SerializeField] private UIWindowElement windowElement;
    [SerializeField] private UIButtonElement buttonElement;
    [SerializeField] private UIInputElement inputElement;
    [SerializeField] private UITextElement textElement;
    [SerializeField] private UISliderElement sliderElement;

    private readonly float ENABLE_VALUE = 1;
    private readonly float DISABLE_VALUE = 0.8f;

    private void ResetFind()
    {
        CanvasGroup findId = buttonElement.buttonFindId.GetComponent<CanvasGroup>();
        CanvasGroup findPassword = buttonElement.buttonFindPassword.GetComponent<CanvasGroup>();

        findId.alpha = ENABLE_VALUE;
        findPassword.alpha = DISABLE_VALUE;

        textElement.textFind.text = "이메일";

        findType = "ID";
    }

    private void ResetOption()
    {
        TextMeshProUGUI forward = textElement.textForward;
        TextMeshProUGUI backward = textElement.textBackward;
        TextMeshProUGUI right = textElement.textRight;
        TextMeshProUGUI left = textElement.textLeft;
        Slider bgm = sliderElement.sliderBgm;
        Slider sfx = sliderElement.sliderSfx;

        forward.text = GameManager.Instance.keyBindings["Forward"].ToString();
        backward.text = GameManager.Instance.keyBindings["Backward"].ToString();
        right.text = GameManager.Instance.keyBindings["Right"].ToString();
        left.text = GameManager.Instance.keyBindings["Left"].ToString();
        bgm.value = GameManager.Instance.bgm;
        sfx.value = GameManager.Instance.sfx;
    }

    #region Button
    public void ButtonLogin()
    {
        GameObject[] windows = { windowElement.windowBlack, windowElement.windowLogin };

        _WindowEnableCommand = new WindowEnableCommand(windows);
        _WindowEnableCommand.Execute();
    }
    public void ButtonSingUp()
    {
        GameObject[] windows = { windowElement.windowBlack, windowElement.windowSignUp };

        _WindowEnableCommand = new WindowEnableCommand(windows);
        _WindowEnableCommand.Execute();
    }
    public void ButtonFind()
    {
        GameObject[] windows = { windowElement.windowBlack, windowElement.windowFind };

        _WindowEnableCommand = new WindowEnableCommand(windows);
        _WindowEnableCommand.Execute();
    }
    public void ButtonOption()
    {
        GameObject[] windows = { windowElement.windowBlack, windowElement.windowOption };

        _WindowEnableCommand = new WindowEnableCommand(windows);
        _WindowEnableCommand.Execute();
    }
    public void ButtonCreate()
    {
        GameObject[] windows = { windowElement.windowBlack, windowElement.windowCreate };

        _WindowEnableCommand = new WindowEnableCommand(windows);
        _WindowEnableCommand.Execute();
    }
    public void ButtonConnect()
    {
        GameObject[] windows = { windowElement.windowBlack, windowElement.windowConnect };

        _WindowEnableCommand = new WindowEnableCommand(windows);
        _WindowEnableCommand.Execute();
    }
    public void ButtonStore() => Application.OpenURL("https://www.youtube.com");
    public void ButtonExit() => Application.Quit();
    public void ButtonConfirmLogin()
    {
        Dictionary<string, TMP_InputField> inputs = new Dictionary<string, TMP_InputField>
        {
            { "ID", inputElement.inputLoginId},
            { "PASSWORD", inputElement.inputLoginPassword}
        };

        _ConfirmLoginCommand = new ConfirmLoginCommand(inputs, StartLoginCoroutine);
        _ConfirmLoginCommand.Execute();
    }
    public void ButtonConfirmSignUp()
    {
        Dictionary<string, TMP_InputField> inputs = new Dictionary<string, TMP_InputField>
        {
            { "ID", inputElement.inputSignUpId },
            { "PASSWORD", inputElement.inputSignUpPassword},
            { "PASSWORDCHECK", inputElement.inputSignUpPasswordCheck},
            { "EMAIL",inputElement.inputSignUpEmail}
        };

        _ConfirmSignUpCommand = new ConfirmSignUpCommand(inputs, StartSignUpCoroutine);
        _ConfirmSignUpCommand.Execute();
    }
    public void ButtonConfirmFind()
    {
        _ConfirmFindCommand = new ConfirmFindCommand(inputElement.inputFindIdEmail, findType, StartFindIdCoroutine, StartFindPasswordCoroutine);
        _ConfirmFindCommand.Execute();
    }
    public void ButtonConfirmOption()
    {
        Dictionary<string, Slider> sliders = new Dictionary<string, Slider>
        {
            { "BGM", sliderElement.sliderBgm},
            { "SFX", sliderElement.sliderSfx}
        };
        Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>
        {
            { "FORWARD", saveForward},
            { "BACKWARD", saveBackward},
            { "RIGHT",saveRight},
            { "LEFT",saveLeft}
        };

        textElement.textLog.text = "Have been Updated";
        GameObject[] windows = { windowElement.windowLog };

        _ConfirmOptionCommand = new ConfirmOptionCommand(sliders, keys);
        _ConfirmOptionCommand.Execute();

        _WindowEnableCommand = new WindowEnableCommand(windows);
        _WindowEnableCommand.Execute();
    }
    public void ButtonConfirmCreate()
    {
        Dictionary<string, TMP_InputField> inputs = new Dictionary<string, TMP_InputField>
        {
            { "SERVERNAME", inputElement.inputCreateServerName},
            { "ADDRESS", inputElement.inputCreateAddress}
        };

        _ConfirmCreateCommand = new ConfirmCreateCommand(inputs, StartCreateCoroutine);
        _ConfirmCreateCommand.Execute();
    }
    public void ButtonConfirmConnect()
    {
        Dictionary<string, TMP_InputField> inputs = new Dictionary<string, TMP_InputField>
        {
            { "ADDRESS", inputElement.inputConnectAddress}
        };

        _ConfirmConnectCommand = new ConfirmConnectCommand(inputs, StartConnectCoroutine);
        _ConfirmConnectCommand.Execute();
    }
    public void ButtonSelectId()
    {
        CanvasGroup findId = buttonElement.buttonFindId.GetComponent<CanvasGroup>();
        CanvasGroup findPassword = buttonElement.buttonFindPassword.GetComponent<CanvasGroup>();

        findId.alpha = ENABLE_VALUE;
        findPassword.alpha = DISABLE_VALUE;

        textElement.textFind.text = "이메일";

        findType = "ID";
    }
    public void ButtonSelectPassword()
    {
        CanvasGroup findId = buttonElement.buttonFindId.GetComponent<CanvasGroup>();
        CanvasGroup findPassword = buttonElement.buttonFindPassword.GetComponent<CanvasGroup>();

        findId.alpha = DISABLE_VALUE;
        findPassword.alpha = ENABLE_VALUE;

        textElement.textFind.text = "아이디";

        findType = "PASSWORD";
    }
    public void ButtonIdCheck()
    {
        Dictionary<string, TMP_InputField> inputs = new Dictionary<string, TMP_InputField>
        {
            { "ID",inputElement.inputSignUpId}
        };

        _CheckIdCommand = new CheckIdCommand(inputs, StartCheckIdCoroutine);
        _CheckIdCommand.Execute();
    }
    public void ButtonEmailCheck()
    {
        Dictionary<string, TMP_InputField> inputs = new Dictionary<string, TMP_InputField>
        {
            { "EMAIL",inputElement.inputSignUpEmail}
        };

        _CheckEmailCommand = new CheckEmailCommand(inputs, StartCheckEmailCoroutine);
        _CheckEmailCommand.Execute();
    }
    public void ButtonCancleLogin()
    {
        TMP_InputField[] inputs =
        {
            inputElement.inputLoginId,
            inputElement.inputLoginPassword
        };
        GameObject[] windows =
        {
            windowElement.windowLogin,
            windowElement.windowBlack
        };

        _CancleBaseCommand = new CancleBaseCommand(inputs, windows);
        _CancleBaseCommand.Execute();
    }
    public void ButtonCancleSignUp()
    {
        TMP_InputField[] inputs =
        {
            inputElement.inputSignUpId,
            inputElement.inputSignUpPassword,
            inputElement.inputSignUpPasswordCheck,
            inputElement.inputSignUpEmail
        };
        GameObject[] windows =
        {
            windowElement.windowSignUp,
            windowElement.windowBlack
        };

        _CancleBaseCommand = new CancleBaseCommand(inputs, windows);
        _CancleBaseCommand.Execute();
    }
    public void ButtonCancleFind()
    {
        TMP_InputField[] inputs =
        {
            inputElement.inputFindIdEmail,
            inputElement.inputFindPasswordId
        };
        GameObject[] windows =
        {
            windowElement.windowFind,
            windowElement.windowBlack
        };

        _CancleFindCommand = new CancleFindCommand(inputs, windows, ResetFind);
        _CancleFindCommand.Execute();
    }
    public void ButtonCancleOption()
    {
        GameObject[] windows =
        {
            windowElement.windowOption,
            windowElement.windowBlack
        };

        _CancleOptionCommand = new CancleOptionCommand(windows, ResetOption);
        _CancleOptionCommand.Execute();
    }
    public void ButtonCancleCreate()
    {
        TMP_InputField[] inputs =
        {
            inputElement.inputCreateServerName,
            inputElement.inputCreateAddress
        };
        GameObject[] windows =
        {
            windowElement.windowCreate,
            windowElement.windowBlack
        };

        _CancleBaseCommand = new CancleBaseCommand(inputs, windows);
        _CancleBaseCommand.Execute();
    }
    public void ButtonCancleConnect()
    {
        TMP_InputField[] inputs =
        {
            inputElement.inputConnectAddress
        };
        GameObject[] windows =
        {
            windowElement.windowConnect,
            windowElement.windowBlack
        };

        _CancleBaseCommand = new CancleBaseCommand(inputs, windows);
        _CancleBaseCommand.Execute();
    }
    public void ButtonCancleLog()
    {
        _WindowDisableCommand = new WindowDisableCommand(windowElement.windowLog);
        _WindowDisableCommand.Execute();
    }
    public void SetForwardKey() => StartSetKeyCoroutine(textElement.textForward);
    public void SetBackwardKey() => StartSetKeyCoroutine(textElement.textBackward);
    public void SetRightKey() => StartSetKeyCoroutine(textElement.textRight);
    public void SetLeftKey() => StartSetKeyCoroutine(textElement.textLeft);
    #endregion

    #region Key
    private void StartSetKeyCoroutine(TextMeshProUGUI text)
    {
        if (setKeyCoroutine != null) { setKeyCoroutine = null; }

        setKeyCoroutine = StartCoroutine(SetKeyCoroutine(text));
    }

    private IEnumerator SetKeyCoroutine(TextMeshProUGUI text)
    {
        do
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Escape))
                {
                    setKeyCoroutine = null;
                }

                if (Input.GetKeyDown(keyCode))
                {
                    if (text == textElement.textForward) saveForward = keyCode;
                    else if (text == textElement.textBackward) saveBackward = keyCode;
                    else if (text == textElement.textRight) saveRight = keyCode;
                    else if (text == textElement.textLeft) saveLeft = keyCode;

                    text.text = keyCode.ToString();

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
        yield return APIManager.Instance.Login(textElement.textLog, id, password);

        GameObject[] windows = { windowElement.windowLog };
        _WindowEnableCommand = new WindowEnableCommand(windows);
        _WindowEnableCommand.Execute();

        loginCoroutine = null;
    }
    #endregion

    #region SignUp
    private void StartCheckIdCoroutine(string id)
    {
        if (checkCoroutine != null) { return; }

        checkCoroutine = StartCoroutine(CheckIdCoroutine(id));
    }

    private void StartCheckEmailCoroutine(string email)
    {
        if (checkCoroutine != null) { return; }

        checkCoroutine = StartCoroutine(CheckEmailCoroutine(email));
    }

    private void StartSignUpCoroutine(string id, string password, string passwordCheck, string email)
    {
        if (signUpCoroutine != null) { return; }

        if (password != passwordCheck)
        {
            textElement.textLog.text = "Please confirm password";

            GameObject[] windows = { windowElement.windowLog };
            _WindowEnableCommand = new WindowEnableCommand(windows);
            _WindowEnableCommand.Execute();
        }
        else
        {
            signUpCoroutine = StartCoroutine(SignUpCoroutine(id, password, email));
        }
    }

    private IEnumerator CheckIdCoroutine(string id)
    {
        yield return APIManager.Instance.AccountUsernameCheck(textElement.textLog, id);

        GameObject[] windows = { windowElement.windowLog };
        _WindowEnableCommand = new WindowEnableCommand(windows);
        _WindowEnableCommand.Execute();

        checkCoroutine = null;
    }

    private IEnumerator CheckEmailCoroutine(string email)
    {
        yield return APIManager.Instance.AccountEmailCheck(textElement.textLog, email);

        GameObject[] windows = { windowElement.windowLog };
        _WindowEnableCommand = new WindowEnableCommand(windows);
        _WindowEnableCommand.Execute();

        checkCoroutine = null;
    }

    private IEnumerator SignUpCoroutine(string id, string password, string email)
    {
        yield return APIManager.Instance.AccountJoin(textElement.textLog, id, password, email);

        GameObject[] windows = { windowElement.windowLog };
        _WindowEnableCommand = new WindowEnableCommand(windows);
        _WindowEnableCommand.Execute();

        signUpCoroutine = null;
    }
    #endregion

    #region Find
    private void StartFindIdCoroutine(string email)
    {
        if (findCoroutine != null) { return; }

        findCoroutine = StartCoroutine(FindIdCoroutine(email));
    }

    private void StartFindPasswordCoroutine(string id)
    {
        if (findCoroutine != null) { return; }

        findCoroutine = StartCoroutine(FindPasswordCoroutine(id));
    }

    private IEnumerator FindIdCoroutine(string email)
    {
        yield return APIManager.Instance.AccountFindUsername(textElement.textLog, email);

        GameObject[] windows = { windowElement.windowLog };
        _WindowEnableCommand = new WindowEnableCommand(windows);
        _WindowEnableCommand.Execute();

        findCoroutine = null;
    }

    private IEnumerator FindPasswordCoroutine(string id)
    {
        yield return APIManager.Instance.AccountFindPassword(textElement.textLog, id);

        GameObject[] windows = { windowElement.windowLog };
        _WindowEnableCommand = new WindowEnableCommand(windows);
        _WindowEnableCommand.Execute();

        findCoroutine = null;
    }
    #endregion

    #region Create
    private void StartCreateCoroutine(string serverName, string address)
    {
        if (createCoroutine != null) { return; }
    }
    #endregion

    #region Connect
    private void StartConnectCoroutine(string address)
    {
        if (connectCoroutine != null) { return; }
    }
    #endregion
}