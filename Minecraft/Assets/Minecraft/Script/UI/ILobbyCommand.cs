using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface UICommand
{
    public void Execute();
}

public class ConfirmLoginCommand : UICommand
{
    private Dictionary<string, TMP_InputField> inputs;
    private Action<string, string> action;

    public ConfirmLoginCommand(Dictionary<string, TMP_InputField> inputs, Action<string, string> action)
    {
        this.inputs = inputs;
        this.action = action;
    }

    public void Execute()
    {
        string id = inputs["ID"].text;
        string password = inputs["PASSWORD"].text;

        action(id, password);
    }
}

public class ConfirmSignUpCommand : UICommand
{
    private Dictionary<string, TMP_InputField> inputs;
    private Action<string, string, string, string> action;

    public ConfirmSignUpCommand(Dictionary<string, TMP_InputField> inputs, Action<string, string, string, string> action)
    {
        this.inputs = inputs;
        this.action = action;
    }

    public void Execute()
    {
        string id = inputs["ID"].text;
        string password = inputs["PASSWORD"].text;
        string passwordCheck = inputs["PASSWORDCHECK"].text;
        string email = inputs["EMAIL"].text;

        action(id, password, passwordCheck, email);
    }
}

public class ConfirmFindCommand : UICommand
{
    private TMP_InputField input;
    private string type;
    private Action<string> actionId;
    private Action<string> actionPassword;

    public ConfirmFindCommand(TMP_InputField input, string type, Action<string> actionId, Action<string> actionPassword)
    {
        this.input = input;
        this.type = type;
        this.actionId = actionId;
        this.actionPassword = actionPassword;
    }

    public void Execute()
    {
        string value = input.text;

        switch (type)
        {
            case "ID": actionId(value); break;
            case "PASSWORD": actionPassword(value); break;
        }
    }
}

public class ConfirmOptionCommand : UICommand
{
    private Dictionary<string, Slider> sliders;
    private Dictionary<string, KeyCode> keys;

    public ConfirmOptionCommand(Dictionary<string, Slider> sliders, Dictionary<string, KeyCode> keys)
    {
        this.sliders = sliders;
        this.keys = keys;
    }

    public void Execute()
    {
        GameManager.Instance.SetSound(sliders["BGM"].value, sliders["SFX"].value);
        GameManager.Instance.SetKey(keys["FORWARD"], keys["BACKWARD"], keys["RIGHT"], keys["LEFT"]);
    }
}

public class ConfirmCreateCommand : UICommand
{
    private Dictionary<string, TMP_InputField> inputs;
    private Action<string, string> action;

    public ConfirmCreateCommand(Dictionary<string, TMP_InputField> inputs, Action<string, string> action)
    {
        this.inputs = inputs;
        this.action = action;
    }

    public void Execute()
    {
        string serverName = inputs["SERVERNAME"].text;
        string address = inputs["ADDRESS"].text;

        action(serverName, address);
    }
}

public class ConfirmConnectCommand : UICommand
{
    private Dictionary<string, TMP_InputField> inputs;
    private Action<string> action;

    public ConfirmConnectCommand(Dictionary<string, TMP_InputField> inputs, Action<string> action)
    {
        this.inputs = inputs;
        this.action = action;
    }

    public void Execute()
    {
        string address = inputs["ADDRESS"].text;

        action(address);
    }
}

public class CheckIdCommand : UICommand
{
    private Dictionary<string, TMP_InputField> inputs;
    private Action<string> action;

    public CheckIdCommand(Dictionary<string, TMP_InputField> inputs, Action<string> action)
    {
        this.inputs = inputs;
        this.action = action;
    }

    public void Execute()
    {
        string id = inputs["ID"].text;

        action(id);
    }
}

public class CheckEmailCommand : UICommand
{
    private Dictionary<string, TMP_InputField> inputs;
    private Action<string> action;

    public CheckEmailCommand(Dictionary<string, TMP_InputField> inputs, Action<string> action)
    {
        this.inputs = inputs;
        this.action = action;
    }

    public void Execute()
    {
        string email = inputs["EMAIL"].text;

        action(email);
    }
}

public class WindowEnableCommand : UICommand
{
    private GameObject[] windows;

    public WindowEnableCommand(GameObject[] windows)
    {
        this.windows = windows;
    }

    public void Execute()
    {
        foreach (GameObject window in windows) { window.SetActive(true); }
    }
}

public class WindowDisableCommand : UICommand
{
    private GameObject window;

    public WindowDisableCommand(GameObject window)
    {
        this.window = window;
    }

    public void Execute()
    {
        window.SetActive(false);
    }
}

public class CancleBaseCommand : UICommand
{
    private TMP_InputField[] inputs;
    private GameObject[] windows;

    public CancleBaseCommand(TMP_InputField[] inputs, GameObject[] windows)
    {
        this.inputs = inputs;
        this.windows = windows;
    }

    public void Execute()
    {
        foreach (TMP_InputField input in inputs) { input.text = string.Empty; }
        foreach (GameObject window in windows) { window.SetActive(false); }
    }
}

public class CancleFindCommand : UICommand
{
    private TMP_InputField[] inputs;
    private GameObject[] windows;
    private Action action;

    public CancleFindCommand(TMP_InputField[] inputs, GameObject[] windows, Action action)
    {
        this.inputs = inputs;
        this.windows = windows;
        this.action = action;
    }

    public void Execute()
    {
        foreach (TMP_InputField input in inputs) { input.text = string.Empty; }
        foreach (GameObject window in windows) { window.SetActive(false); }

        action();
    }
}

public class CancleOptionCommand : UICommand
{
    private GameObject[] windows;
    private Action action;

    public CancleOptionCommand(GameObject[] windows, Action action)
    {
        this.windows = windows;
        this.action = action;
    }

    public void Execute()
    {
        foreach (GameObject window in windows) { window.SetActive(false); }

        action();
    }
}