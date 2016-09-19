using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AutoAttack : MonoBehaviour
{
    public float Damage = 20;
    public float Range = 3;
    public float AttackRate = 1;

    float cooldown = 0;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    IEnumerator ApplyDamage(float value, DemonMove demon)
    {
        yield return new WaitForSeconds(0.5f);
        demon.ApplyDamage(value);
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        //Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Vector3 fwd = transform.forward;
        Vector3 origin = transform.position + transform.up;
        Debug.DrawRay(origin, fwd * Range, Color.red);
        if (cooldown <= 0)
        {
            if (Physics.Raycast(origin, fwd, out hit, Range))
            {
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    cooldown = 1.0f / AttackRate;
                    animator.SetTrigger("Melee Attack");
                    var demon = hit.collider.GetComponent<DemonMove>();
                    StartCoroutine(ApplyDamage(Damage, demon));
                }
            }
        }
        else
        {
            cooldown -= Time.deltaTime;
        } 
    }
}
