using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(DemonMove))]
public class DemonFollow : MonoBehaviour
{
    public Transform target;

    DemonMove demon;

    void Awake()
    {
        demon = GetComponent<DemonMove>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (demon.IsAlive)
        {
            if (target != null && target.position.z < transform.position.z)
            {
                transform.LookAt(target, Vector3.up);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * 0.8f);
            }
        }
    }
}
