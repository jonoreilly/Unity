﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Ball : MonoBehaviour {

    public float speed = 30;

    private Rigidbody2D rigidBody;

    private AudioSource audioSource;

	// Use this for initialization
	void Start () {

        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = Vector2.right * speed;
		
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        //LeftPaddle or RightPaddle

        if((col.gameObject.name == "LeftPaddle") ||
           (col.gameObject.name == "RightPaddle"))
        {
            HandlePaddleHit(col);
        }


            //WallBottom or WallBot

        if ((col.gameObject.name == "WallBot") ||
            (col.gameObject.name == "WallTop"))
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.wallBloop);
        }

        //LeftGoal or RightGoal

        if ((col.gameObject.name == "LeftGoal") ||
            (col.gameObject.name == "RightGoal"))
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.goalBloop);

            if (col.gameObject.name == "LeftGoal")
            {
                IncreaseTextUIScore("RightScoreUI");
            }
            if (col.gameObject.name == "RightGoal")
            {
                IncreaseTextUIScore("LeftScoreUI");
            }

            //ToDo Update Score UI

            transform.position = new Vector2(0, 0);
        }
    }



    void HandlePaddleHit(Collision2D col)
    {
        float y = BallHitPaddleWhere(transform.position,
                                     col.transform.position,
                                     col.collider.bounds.size.y);

        Vector2 dir = new Vector2();

        if (col.gameObject.name == "LeftPaddle")
        {
            dir = new Vector2(1, y).normalized;     
        }

        if ( col.gameObject.name == "RightPaddle")
        {
            dir = new Vector2(-1, y).normalized;
        }

        rigidBody.velocity = dir * speed;

        SoundManager.Instance.PlayOneShot(SoundManager.Instance.hitPaddleBloop);
    }

    float BallHitPaddleWhere(Vector2 ball, Vector2 paddle, float paddleHeight)
    {
        return (ball.y - paddle.y) / paddleHeight;
    }

    void IncreaseTextUIScore(string textUIName)
    {
        var textUIComp = GameObject.Find(textUIName).GetComponent<Text>();

        int score = int.Parse(textUIComp.text);

        score++;

        textUIComp.text = score.ToString();
     }
}
