using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animation))]
public class DemonMove: MonoBehaviour
{
    public float health = 20;
    public float speed = 1;
    
    CharacterController controller;
    Animation anim;

    bool alive = false;
    bool dead = false;
    bool attacking = false;

    public bool IsAlive
    {
        get { return alive; }
    }

    public void ApplyDamage(float value)
    {
        health -= value;
    }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animation>();
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            yield return new WaitForFixedUpdate();
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.005f, transform.position.z);
        }
    }

    IEnumerator Attack()
    {
        attacking = true;
        anim.Blend("Attack");
        yield return new WaitForSeconds(1);
        attacking = false;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!dead && !attacking && hit.collider.tag == "Player")
        {
            StartCoroutine(Attack());

        }
    }

    void Update()
    {
        if (!dead && health <= 0)
        {
            alive = false;
            dead = true;
            controller.enabled = false;
            anim.Stop("Walk");
            anim.Blend("Die");
            Destroy(gameObject, 2);
            StartCoroutine(Fade());
        }
        else
        {
            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
            if (alive)
            {
                if (pos.x >= 0 && pos.x <= 1 && pos.y >= 0 && pos.y <= 1 && pos.z >= 0)
                    controller.SimpleMove(transform.forward * speed);
                else
                    Destroy(gameObject);
            }
            else
            {
                if (!dead && !attacking && pos.x >= 0 && pos.x <= 1 && pos.y >= 0 && pos.y <= 1 && pos.z >= 0)
                {
                    alive = true;
                    anim.Blend("Walk");
                }
            }
        }
    }
}
