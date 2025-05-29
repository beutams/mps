using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Image img;
    public TextMeshProUGUI titleObject;
    public TextMeshProUGUI descriptionObject;
    public TextMeshProUGUI gameModeObject;
    public TextMeshProUGUI playerObject;

    protected string owner;
    protected string chapter;
    protected string title;
    protected string description;
    protected string gameMode;
    protected string number;
    protected string maxNumber;
    protected virtual void Refresh()
    {
        titleObject.text = title; 
        descriptionObject.text = description;
        gameModeObject.text = gameMode;
        playerObject.text = number + "/" + maxNumber;
    }
    protected virtual void OnJoin()
    {

    }
}
