using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPanelController : MonoBehaviour
{

    private GameObject playerNameInput;
    private GameObject playerColorToggle;

    void Awake()
    {
        playerNameInput = transform.Find("UserNameInput").gameObject;
        playerColorToggle = transform.Find("UserColorToggleGroup").gameObject;
    }

    public void OnStartGameClicked()
    {
        InitPlayerData();
        SceneManager.LoadScene("1MainScene");
    }

    private void InitPlayerData()
    {
        GameController.instance.playerData.playerName 
            = playerNameInput.GetComponent<InputField>().text;

        Toggle[] activeToggles = playerColorToggle.GetComponentsInChildren<Toggle>();
        int toggleLen = activeToggles.Length;
        int toggleIndex = 0;
        for (int i = 0; i < toggleLen; i++)
        {
            if (activeToggles[i].isOn == true)
            {
                toggleIndex = i;
                break;
            }
        }
        Color playerColor = Color.white;
        switch(toggleIndex)
        {
            case 0:
                playerColor = Color.white;
                break;
            case 1:
                playerColor = Color.yellow;
                break;
            case 2:
                playerColor = Color.blue;
                break;
        }
        GameController.instance.playerData.playerColor = playerColor;
    }
}
