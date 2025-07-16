using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIItem : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public Image icon;
    public Image hero;
    public List<Image> skills;
    public GameObject ready;
    public GameObject unReady;
}
