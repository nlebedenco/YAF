using UnityEngine;
using System.Collections;


using UnityStandardAssets.CrossPlatformInput;

public class Test : MonoBehaviour
{
    public float speed = 1.5f;
    private Vector3 target;

    void Start()
    {
        target = transform.position;
    }

    void Update()
    {

        Vector2 tap = new Vector2(CrossPlatformInputManager.GetAxis("TapX"), CrossPlatformInputManager.GetAxis("TapY"));
        if (tap.x != 0 || tap.y != 0)
        {
            Debug.LogFormat("Tap: {0}", tap);
            target = Camera.main .ScreenToWorldPoint(new Vector3(tap.x, tap.y, 10));
            Debug.LogFormat("World: {0}", target);
            target = new Vector3(target.x, transform.position.y, target.z);

        }

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
