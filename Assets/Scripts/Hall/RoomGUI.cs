using Mirror;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomGUI : MonoBehaviour
{
    private Guid roomId;

    public TextMeshProUGUI text;
    public Image image;
    public Toggle toggleButton;
    private HallManager hallManager;

    private void Awake()
    {
        hallManager = FindAnyObjectByType<HallManager>();
        toggleButton.onValueChanged.AddListener(OnToggleClicked);
    }
    [ClientCallback]
    public void OnToggleClicked(bool isOn)
    {
        hallManager.SelectRoom(isOn ? roomId : Guid.Empty);
        image.color = isOn ? new Color(0f, 1f, 0f, 0.5f) : new Color(1f, 1f, 1f, 0.2f);
    }
    public void Init(string str,Guid roomId)
    {
        text.text = str;
        this.roomId = roomId;
    }
}
