using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class Character : MonoBehaviour {

    // Use this for initialization
    [Header("Move")]
    [SerializeField]
    float speed = 20f;

    public bool IsWalking { get { return movement.Direction != MoveDirection.NONE; } }
    Movement movement = new Movement();

    protected Animator animator;

	void Start () {
        animator = GetComponent<Animator>();

        OnStart();
    }

    protected virtual void OnStart()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        ProcessMoveInput();
        ProcessSkillInput();
    }

    public virtual bool CanMovable()
    {
        return true;
    }

    void ProcessMoveInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartMove(MoveDirection.UP);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            StopMove(MoveDirection.UP);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartMove(MoveDirection.DOWN);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            StopMove(MoveDirection.DOWN);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartMove(MoveDirection.LEFT);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            StopMove(MoveDirection.LEFT);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartMove(MoveDirection.RIGHT);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            StopMove(MoveDirection.RIGHT);
        }

        if (CanMovable() && IsWalking)
        {
            float newX = 0;
            float newY = 0;

            var distance = speed * Time.deltaTime;

            if (movement.HasDirection(MoveDirection.UP))
            {
                newY += distance;
            }
            if (movement.HasDirection(MoveDirection.DOWN))
            {
                newY -= distance;
            }
            if (movement.HasDirection(MoveDirection.LEFT))
            {
                newX -= distance;
            }
            if (movement.HasDirection(MoveDirection.RIGHT))
            {
                newX += distance;
            }

            transform.Translate(new Vector2(newX, newY));

            SetFace();
        }
    }

    bool dragonCombo = false;
    Coroutine comboCoroutine;
    protected virtual void ProcessSkillInput()
    {
    }

    void SetFace()
    {
        if (movement.HasDirection(MoveDirection.UP))
        {
            animator.SetFloat("FaceY", 1f);
        }
        if (movement.HasDirection(MoveDirection.DOWN))
        {
            animator.SetFloat("FaceY", -1f);
        }
        if (movement.HasDirection(MoveDirection.LEFT))
        {
            animator.SetFloat("FaceX", -1f);
        }
        if (movement.HasDirection(MoveDirection.RIGHT))
        {
            animator.SetFloat("FaceX", 1f);
        }

        if (movement.HasDirection(MoveDirection.HORIZONTAL) == false)
        {
            animator.SetFloat("FaceX", 0f);
        }
        if (movement.HasDirection(MoveDirection.VERTICAL) == false)
        {
            animator.SetFloat("FaceY", 0f);
        }
    }

    void StartMove(MoveDirection dir)
    {
        movement.AddDirection(dir);

        switch (dir)
        {
            case MoveDirection.UP:
                {
                    movement.RemoveDirection(MoveDirection.DOWN);
                }
                break;
            case MoveDirection.DOWN:
                {
                    movement.RemoveDirection(MoveDirection.UP);
                }
                break;
            case MoveDirection.LEFT:
                {
                    movement.RemoveDirection(MoveDirection.RIGHT);
                }
                break;
            case MoveDirection.RIGHT:
                {
                    movement.RemoveDirection(MoveDirection.LEFT);
                }
                break;
        }

        if(CanMovable())
        {
            animator.SetBool("Walk", true);
        }
    }

    void StopMove(MoveDirection dir)
    {
        movement.RemoveDirection(dir);

        if(movement.Direction == MoveDirection.NONE)
        {
            animator.SetBool("Walk", false);
        }
    }

    
}
