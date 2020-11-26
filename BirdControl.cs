using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class BirdControl : MonoBehaviour
{
    public delegate void PlayerDelegate();

    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;
    public Vector3 startPos;
    public Rigidbody2D rigidBody ;
    public float rotate = 1f;
    GameObject Objectt;
    public  int tiltSmooth = 1;
    private Quaternion upRotation;
    public Quaternion downRotation;
    public int force = 20;
    private GameManager game;
    
    
    void Start()
    {
        startPos = new Vector3(6f,7f);
        game = GameManager.Instance;
        rigidBody = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.Euler(0, 0, 0);
        upRotation = Quaternion.Euler(0, 0, 20);
        downRotation = Quaternion.Euler(0, 0, -70);
        rigidBody.simulated = false;
        
        
    }

    private void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameStarted()
    {
        
        rigidBody.velocity = Vector3.zero;
        rigidBody.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = new Vector3(-10,22);
        transform.rotation = Quaternion.identity;
    }

    
    void Update()
    {
        if(game.GameOver) return;

        if (Input.GetKey("space") || Input.GetMouseButton(0))
        {
            transform.rotation = upRotation;
            
            rigidBody.velocity = new Vector2(0f,1f*force);
            //rigidBody.AddForce(Vector2.up*force);
            //bird.velocity = Vector3.zero;
            //bird.AddForce(Vector2.right*10f);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation,downRotation,Time.deltaTime*tiltSmooth);
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        
        if (coll.gameObject.tag == "score zones")
        {
            OnPlayerScored(); // event sent to game manager
            
        }
        if (coll.gameObject.tag == "dead zones")
        {
            rigidBody.simulated = false;
            OnPlayerDied();
            
        }
    }
}
