using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    //ComponentReferences
    [SerializeField] private EdgeCollider2D line;
    //Params
    [SerializeField] private float timeForLine;
    [SerializeField] private float waitTime;
    //Temps
    private Vector2[] points;
    //Public
    public Vector2 Movement { get; private set; }


    private void Awake() {
        points = line.points;
        StartCoroutine(MoveToPoint());
    }

    private IEnumerator MoveToPoint() {
        int rounds = 0;
        while (true) {
            float counter = 0;
            while (counter / timeForLine < 1) {
                counter += Time.deltaTime;
                Vector2 destination = Vector2.Lerp(points[rounds], points[(rounds + 1) % points.Length],
                    counter / timeForLine);
                transform.position = destination;
                Movement = (destination - (Vector2) transform.position) / Time.deltaTime;
                yield return null;
            }
            rounds++;
            if (rounds == points.Length) rounds = 0;
            yield return new WaitForSeconds(waitTime);
        }
    }
}