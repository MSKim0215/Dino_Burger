using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterView
{
    [Header("Animation Info")]
    [SerializeField] private Animator animator;

    private readonly Dictionary<ICharacterState.BehaviourState, string> aimationCodeDict = new()
    {
        { ICharacterState.BehaviourState.Waiting, "animation,1" },
        { ICharacterState.BehaviourState.MoveSuccess, "animation,2" },
        { ICharacterState.BehaviourState.Order, "animation,3" },
        { ICharacterState.BehaviourState.OrderSuccess, "animation,11" },
        { ICharacterState.BehaviourState.OrderFailure, "animation,12" },
        { ICharacterState.BehaviourState.MoveFailure, "animation,13" },
        { ICharacterState.BehaviourState.Move, "animation,21" },
        { ICharacterState.BehaviourState.InterAction, "animation,27" },
    };

    private readonly char[] separator = { ',', ';' };

    public void PlayAnimation(ICharacterState.BehaviourState targetState) => SetInt(aimationCodeDict[targetState]);

    public bool IsPlayOverAnimation() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;

    private void SetInt(string parameter = "key,value")
    {
        string[] param = parameter.Split(separator);
        animator.SetInteger(param[0], Convert.ToInt32(param[1]));

        Debug.Log($"애니메이션 재생: {param[0]}, {param[1]}");
    }
}