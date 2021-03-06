﻿using UnityEngine;
using System.Collections;

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraControl : MonoBehaviour
{

    public static CameraControl instance;

    public float snappiness = 0.15f;
    public float zoomSnappiness = 0.10f;

    public float dragSpeed = 220f;
    public float scrollSpeed = 750f;

    public Plane gamePlane;

    public Vector3 lastRayIntersect;

    private float lastZoom;
    public float zoomSustain = 190f;
    private Rigidbody _rigidbody;

    public bool enableMovement = true;

    // Use this for initialization
    void Start()
    {
        gamePlane = new Plane(Vector3.zero, Vector3.up, Vector3.right);
        _rigidbody = GetComponent<Rigidbody>();
        instance = this;
    }

    void Awake()
    {
        instance = this;
    }


    void LateUpdate()
    {
        Camera.main.orthographicSize = Mathf.Abs(transform.position.z);
    }

    void Update()
    {

        Vector3 velocity = Vector3.zero;
        bool mouseAvailable = true;

        //Drag
        if ((Input.GetKey(KeyCode.Mouse0) && mouseAvailable) || Input.GetKey(KeyCode.Mouse2))
        {
            //Raycast into plane that makes up the game field
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float dist;
            if (gamePlane.Raycast(ray, out dist))
            {
                if (lastRayIntersect != Vector3.zero)
                {
                    velocity = new Vector3(lastRayIntersect.x - ray.GetPoint(dist).x, lastRayIntersect.y - ray.GetPoint(dist).y, 0f);
                }
                lastRayIntersect = ray.GetPoint(dist);
            }
            else
            {
                lastRayIntersect = Vector3.zero;
            }

        }
        else
        {
            lastRayIntersect = Vector3.zero;
        }


        if (!enableMovement) velocity = Vector3.zero;

        float z = Input.GetAxis("Mouse ScrollWheel");
        z = Mathf.Clamp(z, -0.25f, 0.25f);
        z = Mathf.Lerp(lastZoom, z, Time.deltaTime * zoomSustain);


        _rigidbody.velocity = new Vector3(
            Mathf.Lerp(_rigidbody.velocity.x, velocity.x * dragSpeed, snappiness),
            Mathf.Lerp(_rigidbody.velocity.y, velocity.y * dragSpeed, snappiness),
            Mathf.Lerp(_rigidbody.velocity.z, z * scrollSpeed, zoomSnappiness));

        if (transform.position.z > -1.6f && _rigidbody.velocity.z > 0f)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, 0f);
        }
        if (transform.position.z > -1f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
        }

        if (transform.position.z < -43f && _rigidbody.velocity.z < 0f)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, 0f);
        }
        if (transform.position.z < -44f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -44f);
        }




        lastZoom = z;
    }
}