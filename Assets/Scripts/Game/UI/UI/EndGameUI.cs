using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Button button;
    private void Start()
    {
        string text = name.Split("UI")[0];
        title.text = text;
        button.onClick.AddListener(Back);
    }
    public void Back()
    {
        //GameEntry.ProcedureComponent.Change<EndGameProcedure>();
    }
}
