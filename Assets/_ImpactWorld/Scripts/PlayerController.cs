using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;

    private int currentPriority;
    [HideInInspector]
    public bool isJump;
    public float jumpDistance;
    public float jumpPower;
    public float jumpDuration;
    public float moveDuration;
    public float rotationSpeed;


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

        MoveByArrow();

        // Only change the player's direction
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
    /// Directional movement and rotation
    /// </summary>
    private void MoveByArrow()
    {

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

            // Rotate to the cursor direction
            transform.DOLookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z), 0f);
            if (hit.transform.tag == "Ground")
            {
                print("击中地面");

                if (movePriority >= currentPriority)
                {
                    // Dont move by cursor
                    //transform.DOMove(new Vector3(hit.point.x, transform.position.y, hit.point.z), moveDuration);
                }                
            }
            else
            {
                print("不是地面");
            }
        }
    }

    /// <summary>
    /// Detect the player collision in the environment
    /// </summary>
    /// <param name="col"></param>
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
