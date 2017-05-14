using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{

    [SerializeField]
    private Transform delayTimeObj;
    public float delayTime;

	void Awake()
    {
        delayTimeObj.DOMoveX(0, delayTime).OnComplete( () => {
            SceneManager.LoadScene(1);
        });
    }
}
