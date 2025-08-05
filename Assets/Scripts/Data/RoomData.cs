using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoomData
{
    public string owner;
    public string chapter;
    public string title;
    public string description;
    public string gameMode;
    public string maxNumber;
    public RoomData(string owner, string chapter, string title, string description, string gameMode, string maxNumber)
    {
        this.owner = owner;
        this.chapter = chapter;
        this.title = title;
        this.description = description;
        this.gameMode = gameMode;
        this.maxNumber = maxNumber;
    }
    public override string ToString()
    {
        return $"owner={owner},chapter={chapter},title={title},gameMode={gameMode},maxPlayer={maxNumber}";
    }
}
