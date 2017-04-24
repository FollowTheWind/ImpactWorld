using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Check the player's position whether in the air
/// </summary>
public class PlayerFloatingDetection : MonoBehaviour {

    private bool isJump; 

    void Update()
    {
        isJump = PlayerController.instance.isJump;
        Vector3 pos = transform.position;
        Ray ray = new Ray(pos, Vector3.down);
        RaycastHit hit;
        Debug.DrawRay(pos, 1.1f * Vector3.down, Color.red, 10f, false);

        if (isJump && !Physics.Raycast(ray, out hit, 1.1f))
        {
            print("Floating.......");
            transform.DOKill();
        }
        
    }

}
