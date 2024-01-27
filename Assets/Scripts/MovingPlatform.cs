using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour {
    //ComponentReferences
    private Rigidbody2D rb;
    [SerializeField] private EdgeCollider2D line;
    //Params
    [SerializeField] private float timeForLine;
    [SerializeField] private float waitTime;
    //Temps
    private Vector2[] points;
    private Vector2 movement;
    //Public
    public Vector2 Movement => movement;
    
     
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        points = GetPoints();
    }

    private void FixedUpdate() {
        Vector2 destination = Vector2.Lerp(points[0], points[1], (Mathf.PingPong(Time.time, timeForLine + waitTime)- waitTime/2f)/timeForLine);
        rb.MovePosition(destination);
        movement = (destination - (Vector2) transform.position) / Time.deltaTime;
    }

    private Vector2[] GetPoints() => line.points;
}