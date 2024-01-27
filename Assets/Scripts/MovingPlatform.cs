using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour {
    //ComponentReferences
    private Rigidbody2D rb;
    [SerializeField] private EdgeCollider2D line;
    //Params
    private Vector2[] points;
    [SerializeField] private float timeForLine;
    [SerializeField] private float waitTime;
    //Temps
    //Public
     
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        points = GetPoints();
    }

    private void FixedUpdate() {
        rb.MovePosition(Vector2.Lerp(points[0], points[1], (Mathf.PingPong(Time.time, timeForLine + waitTime)- waitTime/2f)/timeForLine));
    }

    private Vector2[] GetPoints() => line.points;
}