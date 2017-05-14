using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), "Name: " + GameController.instance.playerData.playerName);
    }
}
