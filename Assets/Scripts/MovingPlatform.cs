using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    //ComponentReferences
    [SerializeField] private EdgeCollider2D line;
    //Params
    [SerializeField] private float timeForLine;
    [SerializeField] private float waitTime;
    [SerializeField] private bool turnOnLastPoint;
    [SerializeField] private int startPoint;
    //Temps
    private Vector2[] points;
    //Public

    private void Awake() {
        points = line.points;
        StartCoroutine(MoveToPoint(startPoint));
    }

    private IEnumerator MoveToPoint(int start) {
        int rounds = start;
        while (true) {
            float counter = 0;
            while (counter / timeForLine < 1) {
                counter += Time.deltaTime;
                transform.position = Vector2.Lerp(points[rounds], points[(rounds + 1) % points.Length], counter / timeForLine);
                yield return null;
            }
            rounds++;
            if (rounds == points.Length) rounds = 0;
            if (rounds == points.Length - 1 && turnOnLastPoint) {
                StartCoroutine(Rotate(waitTime));
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator Rotate(float time) {
        float counter = 0;
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 180);
        
        while (counter / time < 1) {
            transform.rotation = Quaternion.Lerp(from, to, counter / time);
            counter += Time.deltaTime;
            yield return null;
        }
    }
}