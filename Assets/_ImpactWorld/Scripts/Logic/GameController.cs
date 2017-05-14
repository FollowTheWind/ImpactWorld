using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public Player playerData;

    void Awake()
    {
        // singleton
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
