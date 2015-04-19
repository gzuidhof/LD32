﻿using UnityEngine;
using System.Collections;


public class Game : MonoBehaviour {

    public DrawTrail drawer;
    private CameraControl cameraControl;
    private FollowCamera followCamera;

    public Transform spawnPoint;
    public Transform cameraStartPoint;
    public GameObject player;

    public static Game instance;

    public GameObject explosionPrefab;

    public bool gameOver = false;


    public enum Mode
    {
        Launch,
        View
    }
    public Mode mode = Mode.Launch;


    void Awake()
    {
        cameraControl = Camera.main.GetComponent<CameraControl>();
        followCamera = Camera.main.GetComponent<FollowCamera>();
        instance = this;

    }



    public void ExplodePlayer()
    {
        gameOver = true;
        player.SetActive(false);
        Instantiate(explosionPrefab, player.transform.position, Quaternion.identity);
        followCamera.enabled = false;
        cameraControl.enableMovement = true;
    }

    void Launch()
    {

        foreach (TrailSegment s in TrailSegment.allSegments) {
            Destroy(s.gameObject);
        }
        TrailSegment.allSegments.Clear();

        mode = Mode.Launch;
        gameOver = false;
        //Put player on spawn
        player.transform.position = spawnPoint.position;
        player.transform.rotation = Quaternion.identity;
        var rb = player.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        player.SetActive(true);

        cameraControl.enableMovement = false;
        followCamera.enabled = true;
        drawer.enabled = true;
    }

    void View()
    {
        
        if (mode == Mode.Launch)
        {
            Camera.main.transform.position = cameraStartPoint.position;
        }

        mode = Mode.View;
        player.SetActive(false);

        cameraControl.enableMovement = true;
        followCamera.enabled = false;
        drawer.enabled = false;
        
    }


	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Launch();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            View();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Launch();
        }

	}
}
