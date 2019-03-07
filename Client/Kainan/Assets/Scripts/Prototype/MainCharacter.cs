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

public class MainCharacter : Character {
    [Header("Skill")]
    [SerializeField]
    bool movableWhenPlayingSkillMotion = true;
    [SerializeField]
    float comboDelaySeconds = 0.5f;

    bool IsSkillPlaying = false;
    Movement movement = new Movement();
    TransformMode currentMode = TransformMode.NORMAL;

    protected override void OnStart()
    {
        Camera.main.transform.SetParent(transform);

        animator.SetInteger("Mode", (int)currentMode);

        foreach (var skillState in animator.GetBehaviours<SkillStateMachineBehaviour>())
        {
            skillState.OnStart += OnSkillStart;
            skillState.OnCompleted += OnSkillCompleted;
        }
    }

    public override bool CanMovable()
    {
        return movableWhenPlayingSkillMotion || IsSkillPlaying == false;
    }

    bool dragonCombo = false;
    Coroutine comboCoroutine;
    protected override void ProcessSkillInput()
    {
        if (IsSkillPlaying)
            return;

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
                    PlaySkill(SkillType.SKILL_1);
                    dragonCombo = true;
                }
                else
                {
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
        StartCoroutine(PlaySkillCoroutine(skillType));
    }

    IEnumerator PlaySkillCoroutine(SkillType skillType)
    {
        if(IsWalking)
        {
            animator.SetBool("Walk", false);
            yield return null;
        }

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
