using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement instance;

    private bool isJump;
    public float jumpDistance;
    public float jumpPower;
    public float jumpDuration;
    public float moveDuration;


    void Awake()
    {
        instance = this;
        isJump = true;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            HorizontalMove();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerJump();
        }

    }

    /// <summary>
    /// Control the player jump
    /// </summary>
    private void PlayerJump()
    {
        if (isJump)
        {
            isJump = false;
            transform.DOKill();
            Vector3 pos = transform.position;
            pos += transform.forward * jumpDistance;
            transform.DOJump(new Vector3(pos.x, transform.position.y, pos.z), jumpPower, 1, jumpDuration)
                .OnComplete( () => {
                    isJump = true;
                });
        }
        
    }

    /// <summary>
    /// Move along the XZ plane
    /// </summary>
    private void HorizontalMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            print("击中物体 tag " + hit.transform.tag);

            if (hit.transform.tag == "Ground")
            {
                print("击中地面");

                if (isJump)
                {
                    transform.DOLookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z), 0.3f);
                    transform.DOMove(new Vector3(hit.point.x, transform.position.y, hit.point.z), moveDuration);
                }                
            }
            else
            {
                print("不是地面");
            }
        }
    }

}
