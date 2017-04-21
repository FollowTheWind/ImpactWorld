using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement instance;

    private int currentPriority;
    private bool isJump;
    public float jumpDistance;
    public float jumpPower;
    public float jumpDuration;
    public float moveDuration;


    void Awake()
    {
        instance = this;
        currentPriority = 0;
        isJump = true;
    }

    void Update()
    {
        if (!DOTween.IsTweening(transform))
        {
            currentPriority = 0;
        }

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
        int jumpPriority = 9;
        if (isJump && jumpPriority >= currentPriority)
        {
            isJump = false;
            currentPriority = jumpPriority;
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
        int movePriority = 8;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            print("击中物体 tag " + hit.transform.tag);

            if (hit.transform.tag == "Ground")
            {
                print("击中地面");

                if (movePriority >= currentPriority)
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

    void OnCollisionEnter(Collision col)
    {
        print("发生碰撞");
        if (!isJump)
        {
            transform.DOKill();
            isJump = true;
        }
    }

}
