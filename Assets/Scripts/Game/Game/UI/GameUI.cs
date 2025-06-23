using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("MainSkill")]
    [SerializeField] protected Transform mainSkill;
    [Header("Info")]
    [SerializeField] protected TextMeshProUGUI population;
    [SerializeField] protected TextMeshProUGUI property;
    [Header("Sount")]
    [SerializeField] protected TextMeshProUGUI sount;
    //[Header("MiniMap")]
    [Header("HeroPanel")]
    [SerializeField] protected Image health;
    [SerializeField] protected Image icon;
    //[Header("WeapenPanel")]

}
