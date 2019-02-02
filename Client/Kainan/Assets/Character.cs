using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    // Use this for initialization
    Animator animator;
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.W))
        {
            animator.SetTrigger("Back");
            animator.SetBool("Walk", true);
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("Walk", false);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            animator.SetTrigger("Front");
            animator.SetBool("Walk", true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("Walk", false);
        }
    }
}
