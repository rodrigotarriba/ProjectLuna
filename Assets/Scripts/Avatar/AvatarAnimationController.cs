using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;


public class AvatarAnimationController : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private Animator animator;




    //subscribe and unsubscribe to the new input reference
    private void OnEnable()
    {
        move.action.started += StartWalking;
        move.action.canceled += StopWalking;
    }

    private void OnDisable()
    {
        move.action.started -= StartWalking;
        move.action.canceled -= StopWalking;
    }


    
    private void StartWalking(InputAction.CallbackContext obj)
    {

        bool isMovingForward = move.action.ReadValue<Vector2>().y > 0;

        Debug.Log($"{move.action.ReadValue<Vector2>().y}");


        if (isMovingForward)
        {
            //start the animation and set the speed to 1
            animator.SetBool("isWalking", true);
            animator.SetFloat("animSpeed", 1);
        }
        else
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("animSpeed", -1);
        }
    }

    private void StopWalking(InputAction.CallbackContext obj)
    {
        animator.SetBool("isWalking", false);
        animator.SetFloat("animSpeed", 0);
    }






    //    public Action<InputAction.CallbackContext> StartWalking { get; private set; }



    //    public Action<InputAction.CallbackContext> StopWalking { get; private set; }
    //
    //

}

