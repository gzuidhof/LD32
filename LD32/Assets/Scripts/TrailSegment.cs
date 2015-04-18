﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailSegment : MonoBehaviour
{

    private Mesh mesh;
    private Material mat;
    public Shader shader;
    EdgeCollider2D col;

    void Awake()
    {
        mesh = new Mesh();
        mat = new Material(shader);
        mat.color = new Color(0, 0, 0, 0.8f);
    }


    public void Complete()
    {
        col = gameObject.AddComponent<EdgeCollider2D>();
    }

    void Start()
    {

    }

    void Update()
    {
        Draw();
    }

    void Draw()
    {
        Graphics.DrawMesh(mesh, transform.localToWorldMatrix, mat, 0);
    }

    public void AddLine(Vector3[] quad, bool tmp)
    {
        int vl = mesh.vertices.Length;

        Vector3[] vs = mesh.vertices;
        if (!tmp || vl == 0) vs = resizeVertices(vs, 4);
        else vl -= 4;

        vs[vl] = quad[0];
        vs[vl + 1] = quad[1];
        vs[vl + 2] = quad[2];
        vs[vl + 3] = quad[3];

        int tl = mesh.triangles.Length;

        int[] ts = mesh.triangles;
        if (!tmp || tl == 0) ts = resizeTriangles(ts, 6);
        else tl -= 6;
        ts[tl] = vl;
        ts[tl + 1] = vl + 1;
        ts[tl + 2] = vl + 2;
        ts[tl + 3] = vl + 1;
        ts[tl + 4] = vl + 3;
        ts[tl + 5] = vl + 2;

        mesh.vertices = vs;
        mesh.triangles = ts;
        mesh.RecalculateBounds();
    }

    Vector3[] resizeVertices(Vector3[] ovs, int ns)
    {
        Vector3[] nvs = new Vector3[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
        return nvs;
    }

    int[] resizeTriangles(int[] ovs, int ns)
    {
        int[] nvs = new int[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
        return nvs;
    }

}