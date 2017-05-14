using UnityEngine;

[CreateAssetMenu(fileName ="Data", menuName ="Impact/Player", order = 1)]
public class Player : ScriptableObject
{

    public string playerName = "";
    public Color playerColor = Color.white;
}

public enum PlayerColor
{
    White,
    Yellow,
    Blue,
}

