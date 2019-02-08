using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public enum SkillType
{
    SKILL_1,
    SKILL_2,
    TRANSFORM,
}

public enum TransformMode
{
    NORMAL,
    DRAGON,
}


public class Character : MonoBehaviour {

    // Use this for initialization
    [Header("Move")]
    [SerializeField]
    float speed = 20f;

    [Header("Skill")]
    [SerializeField]
    bool movableWhenPlayingSkillMotion = true;
    [SerializeField]
    float comboDelaySeconds = 0.5f;

    bool IsSkillPlaying = false;
    bool IsWalking { get { return movement.Direction != MoveDirection.NONE; } }
    Movement movement = new Movement();
    TransformMode currentMode = TransformMode.NORMAL;

    Animator animator;

	void Start () {
        animator = GetComponent<Animator>();
        animator.SetInteger("Mode", (int)currentMode);

        foreach (var skillState in animator.GetBehaviours<SkillStateMachineBehaviour>())
        {
            skillState.OnStart += OnSkillStart;
            skillState.OnCompleted += OnSkillCompleted;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        ProcessMoveInput();
        ProcessSkillInput();
    }

    bool CanMovable()
    {
        return movableWhenPlayingSkillMotion || IsSkillPlaying == false;
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
    void ProcessSkillInput()
    {
        if(currentMode == TransformMode.NORMAL)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlaySkill(SkillType.SKILL_1);
            }
            if (Input.GetMouseButtonDown(1))
            {
                PlaySkill(SkillType.SKILL_2);
            }
        }
        else if(currentMode == TransformMode.DRAGON)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (dragonCombo == false)
                {
                    Debug.Log("Skill1");
                    PlaySkill(SkillType.SKILL_1);
                    dragonCombo = true;
                }
                else
                {
                    Debug.Log("Skill2");
                    PlaySkill(SkillType.SKILL_2);
                    dragonCombo = false;
                    if(comboCoroutine != null)
                        StopCoroutine(comboCoroutine);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlaySkill(SkillType.TRANSFORM);
        }
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

    void PlaySkill(SkillType skillType)
    {
        if (IsSkillPlaying)
            return;

        animator.SetBool("Walk", false);

        switch (skillType)
        {
            case SkillType.SKILL_1:
                {
                    animator.SetTrigger("Skill1");
                }
                break;
            case SkillType.SKILL_2:
                {
                    animator.SetTrigger("Skill2");
                }
                break;
            case SkillType.TRANSFORM:
                {
                    animator.SetTrigger("Transform");

                    currentMode = currentMode == TransformMode.NORMAL ? TransformMode.DRAGON : TransformMode.NORMAL;
                    animator.SetInteger("Mode", (int)currentMode);
                }
                break;
        }
    }

    void OnSkillStart()
    {
        IsSkillPlaying = true;
    }

    void OnSkillCompleted()
    {
        if(IsWalking)
            animator.SetBool("Walk", true);

        if(dragonCombo)
        {
            comboCoroutine = StartCoroutine(SetTimer(comboDelaySeconds, 
                () => {
                    Debug.Log("combo time over");
                    dragonCombo = false;
                }));
        }

        IsSkillPlaying = false;
    }

    IEnumerator SetTimer(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);

        if (action != null)
            action();
    }
}
