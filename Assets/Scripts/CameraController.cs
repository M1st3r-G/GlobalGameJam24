using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Rigidbody2D))]
public class CameraController : MonoBehaviour
{
    //ComponentReferences
    private Rigidbody2D rb;
    private Camera cam;
    //Params
    //Temps
    private List<PlayerController> players;
    //Public

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        cam = GetComponent<Camera>();
        players = new List<PlayerController>();
    }

    private void FixedUpdate() {
        Vector2 targetPosition = FindPosition(out float camSize);
        cam.orthographicSize = camSize;
        if (((Vector2)transform.position - targetPosition).magnitude > 0.1f)
        {
            rb.velocity = targetPosition - (Vector2)transform.position;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private Vector2 FindPosition(out float size) {
        Vector2 pos = new Vector2();
        foreach (PlayerController p in players)
        {
            if (p.transform.position.y <= -10) continue;
            pos += (Vector2) p.transform.position;
        }

        pos /= players.Count;

        size = 0f;
        foreach (PlayerController p in players)
        {
            size = Mathf.Max(size, Mathf.Abs(p.transform.position.x - pos.x));
        }

        if (size < 5f) size = 5f;
        return pos;
    }
    
    private void OnPlayerLeave(PlayerController player) {
        players.Remove(player);
    }

    private void RefreshList(PlayerController player) => players.Add(player);
    private void OnEnable() {
        GameManager.OnPlayerJoin += RefreshList;
        GameManager.OnPlayerLeave += OnPlayerLeave;
    }

    private void OnDisable() {
        GameManager.OnPlayerJoin -= RefreshList;
        GameManager.OnPlayerLeave -= OnPlayerLeave;
    }
}