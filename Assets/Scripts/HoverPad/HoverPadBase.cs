using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPadBase : MonoBehaviour
{

    //Defines the current state of this pad, whether it has been touched or not.
    [SerializeField] public HoverState hoverState;
    private HoverState previousState;

    //Particle animation to signal pad state
    private ParticleSystem[] hoverParticleRing;

    //which foot!
    public bool leftFootTouching = false;
    public bool rightFootTouching = false;

    public void Awake()
    {
        hoverParticleRing = GetComponentsInChildren<ParticleSystem>();
        previousState = HoverState.idle;

    }

    public void LateUpdate()
    {
        CheckHoverState();

        leftFootTouching = false;
        rightFootTouching = false;
    }

    /// <summary>
    /// Check if the current hover remains in the same state, which determines animations and other factors later on.
    /// </summary>
    public void CheckHoverState()
    {
        //Are none of the foot touching?
        if (!leftFootTouching && !rightFootTouching)
        {
            foreach (var particle in hoverParticleRing)
            {
                if (particle.isPlaying) particle.Stop();
                previousState = HoverState.noFeetDown;
                break;
            }
        }

        //Is the left and the right touching?
        if (leftFootTouching && rightFootTouching)
        {
            foreach (var particle in hoverParticleRing)
            {
                if (!particle.isPlaying) particle.Play();
                previousState = HoverState.oneFootDown;
            }
        }

        //Is either foot touching?
        if (leftFootTouching || rightFootTouching)
        {

            foreach (var particle in hoverParticleRing)
            {
                Debug.Log("should be playing now");
                if (!particle.isPlaying) particle.Play();
                previousState = HoverState.oneFootDown;
            }
        }

    }

}



public enum HoverState
{
    idle,
    noFeetDown,
    oneFootDown,
    twoFeetDown,
}