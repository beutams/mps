using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Image img;
    public TextMeshProUGUI titleObject;
    public TextMeshProUGUI descriptionObject;
    public TextMeshProUGUI gameModeObject;
    public TextMeshProUGUI playerObject;

    protected RoomData roomData;
    public string number;
    public void SetData(RoomData data)
    {
        roomData = data;
    }
    protected virtual void Refresh()
    {
        titleObject.text = roomData.title; 
        descriptionObject.text = roomData.description;
        gameModeObject.text = roomData.gameMode;
        playerObject.text = number + "/" + roomData.maxNumber;
    }
    protected virtual void OnJoin()
    {

    }
}
