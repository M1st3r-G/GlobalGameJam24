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
                transform.position = Vector2.Lerp(points[rounds], points[(rounds + 1) % points.Length], counter / timeForLine);
                yield return null;
            }
            rounds++;
            if (rounds == points.Length) rounds = 0;
            yield return new WaitForSeconds(waitTime);
        }
    }
}