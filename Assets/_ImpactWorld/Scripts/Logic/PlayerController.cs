﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{

    public static PlayerController instance;

    private int currentPriority;
    public int damageHealth;
    [HideInInspector]
    public bool isJump;
    public float jumpDistance;
    public float jumpPower;
    public float jumpDuration;
    public float moveDuration;
    public float moveSpeed;
    public float playerBounciness;
    public float rotationSpeed;
    public float wallBounciness;

    public override void OnStartLocalPlayer()
    {
        transform.Find("Body").
            GetComponent<MeshRenderer>().material.color 
            = GameController.instance.playerData.playerColor;

        Transform cameraBox = transform.Find("CameraBox");
        cameraBox.gameObject.AddComponent<Camera>();
    }

    void Awake()
    {
        instance = this;
        currentPriority = 0;
        isJump = true;
    }

    void Start()
    {
        transform.LookAt(new Vector3(0, 1, 0));
    }

    void Update()
    {

        if (!isLocalPlayer)
            return;

        MoveByArrow();

        if (!DOTween.IsTweening(transform))
        {
            currentPriority = 0;
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
        // Up and Down change the player move speed
        float verticalMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(0, 0, verticalMove);
        // Left and Right chang the rotation
        float playerRotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, playerRotation, 0);
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

            // Judge forward or backward move
            int verticalDirection = Input.GetAxis("Vertical") < 0 ? -1 : 1;

            Vector3 pos = transform.position;
            pos += verticalDirection * transform.forward * jumpDistance;
            transform.DOJump(new Vector3(pos.x, transform.position.y, pos.z), jumpPower, 1, jumpDuration)
                .OnComplete( () => {
                    isJump = true;
                });
        }
        
    }

    /// <summary>
    /// Move along the XZ plane (dismiss)
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
                    transform.DOMove(new Vector3(hit.point.x, transform.position.y, hit.point.z), moveDuration);
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
        if (!isJump)
        {
            transform.DOKill();
            isJump = true;
        }

        // Caculate the damage;
        if (col.gameObject.tag == "Player")
        {
            print("Player collision...");
            Vector3 collisionVector = (transform.position - col.transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(collisionVector * playerBounciness);
        }

        if (col.gameObject.tag == "Wall")
        {
            print("Wall collision...");
            Vector3 collisionVector = (transform.position - col.contacts[0].point).normalized;
            GetComponent<Rigidbody>().AddForce(collisionVector * wallBounciness);

            var hitCombat = GetComponent<Combat>();
            if (hitCombat != null)
            {
                hitCombat.TakeDamage(damageHealth);
            }
        }               

    }

}
