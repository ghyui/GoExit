using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    // Use this for initialization
    [Header("Animation")]
    [SerializeField]
    float speed = 20f;
    Vector2 face;
    bool bWalk;

    Animator animator;

	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.W))
        {
            SetCharacterFace(0f, 1f);
            Walk();
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            WalkStop();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            SetCharacterFace(0f, -1f);
            Walk();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            WalkStop();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetCharacterFace(-1f, 0f);
            Walk();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            WalkStop();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetCharacterFace(1f, 0f);
            Walk();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            WalkStop();
        }

        if(bWalk)
        {
            var currentPosition = transform.position;
            float newX = transform.position.x;
            float newY = transform.position.y;

            var distance = speed * Time.deltaTime;

            if (face.x > 0)
                newX += distance;
            else if (face.x < 0)
                newX -= distance;
            else if (face.y > 0)
                newY += distance;
            else if (face.y < 0)
                newY -= distance;

            transform.position = new Vector2(newX, newY);
        }
    }

    void SetCharacterFace(float x, float y)
    {
        animator.SetFloat("FaceX", x);
        animator.SetFloat("FaceY", y);

        face = new Vector2(x, y);
    }

    int walkingCount = 0;
    void Walk()
    {
        bWalk = true;
        animator.SetBool("Walk", true);
        ++walkingCount;
    }

    void WalkStop()
    {
        if(--walkingCount> 0)
            return;

        bWalk = false;
        animator.SetBool("Walk", false);
    }
}
