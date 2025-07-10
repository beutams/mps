/*using Mirror;
using Mirror.Discovery;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfflineGUI : MonoBehaviour
{
    [Header("MenuUI")]
    public GameObject menuUI;
    public Button createButton;
    public Button findButton;
    [Header("ListUI")]
    public GameObject listUI;
    public GameObject content;
    public Button exitButton;

    private HallManager hallManager;
    private void Awake()
    {
        hallManager = FindAnyObjectByType<HallManager>();
        createButton.onClick.AddListener(OnCreateClick);
        findButton.onClick.AddListener(OnFindClick);
        exitButton.onClick.AddListener(OnExitClick);
        menuUI.SetActive(true);
        listUI.SetActive(false);
    }
    private void OnFindClick()
    {
        menuUI.SetActive(false);
        listUI.SetActive(true);
        RefreshRoomList();
    }
    private void OnExitClick()
    {
        menuUI.SetActive(true);
        listUI.SetActive(false);
    }
    private void OnCreateClick()
    {
        hallManager.RequestCreateRoom();
    }
    public void RefreshRoomList()
    {
        
    }
}*/