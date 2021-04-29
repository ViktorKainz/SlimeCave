using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveHorizontal;
    private float moveVertical;
    public float speed;
    public Animator animator;
    
    
    public Rigidbody2D player;
    public Camera cam;
    private Vector2 movement;
    private Vector2 mousePos;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal") * speed;
        moveVertical = Input.GetAxis("Vertical") * speed;
        
        movement = new Vector2(moveHorizontal, moveVertical);

        player.velocity = movement * speed;
        animator.SetFloat("Speed", Mathf.Abs(Mathf.Abs(player.velocity.x) > Mathf.Abs(player.velocity.y) ? player.velocity.x : player.velocity.y));

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - player.position;
        float angle =(float) (Math.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 0f);
        player.rotation = angle;
        Debug.Log(angle + " " + mousePos.ToString());
    }   
}
