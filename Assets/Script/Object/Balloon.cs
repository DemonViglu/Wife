using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator animator;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    public void Idle()
    {
        if (rb.bodyType ==RigidbodyType2D.Static)return;
        rb.velocity=new Vector2(rb.velocity.x,0);
        rb.bodyType = RigidbodyType2D.Static;
        GetComponentInParent<Ball>().score++;
    }

    public void _Destroy()
    {
        this.gameObject.SetActive(false);
        animator.SetBool("boom", false);
    }
}
