using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;


public enum CoworkerState
{
    DESTINATION,
    WANDER,
    IDLE
}

public class CoworkerStateMachine : MonoBehaviour
{
    CoworkerState state;
    Coworker cowoker;

    public static int numOfStates = 3;

    public void Awake()
    {
        cowoker = GetComponent<Coworker>();
        state = CoworkerState.IDLE;
        cowoker.Idle();
    }

    public CoworkerState GetState()
    {
        return state;
    }

    public bool TryUpdateState(CoworkerState newState)
    {
        bool success = true;
        switch (state)
        {
            //case CoworkerState.WORKING:
            //    success = updateFromWorking(newState);
            //    break;
            case CoworkerState.DESTINATION:
                success = updateFromDestination(newState);
                break;

            case CoworkerState.IDLE:
                success = updateFromIdle(newState);
                break;
            case CoworkerState.WANDER:
                success = updateFromWander(newState);
                break;
            default:
                break;
        }

        return success || (!success && newState == state);
    }

    //private bool updateFromWorking(CoworkerState newState)
    //{
    //    bool success = false;
    //    print(newState.ToString());
    //    if (newState == CoworkerState.DESTINATION)
    //    {
    //        cowoker.GoToDestination();
    //        success = true;
    //    }else if (newState == CoworkerState.WANDER)
    //    {
    //        cowoker.Wander();
    //        success = true;
    //    }else if(newState == CoworkerState.TALKING)
    //    {
    //        cowoker.StartTalking();
    //        success = true;
    //    }

    //    if (success)
    //    {
    //        state = newState;
    //        print("Successfully changed from working");
    //    }
    //    return success || (!success && newState == state); ;
    //}

    private bool updateFromDestination(CoworkerState newState)
    {
        bool success = false;
        if(newState == CoworkerState.IDLE)
        {
            cowoker.Idle();
            success = true;
        }if (newState == CoworkerState.WANDER) { 
            cowoker.Wander(); 
            success = true; 
        }if(newState == CoworkerState.DESTINATION)
        {
            cowoker.GoToDestination();
            success = true;
        }

        if (success)
        {
            state = newState;
        }
        return success || (!success && newState == state); 
    }

    private bool updateFromWander(CoworkerState newState) {
        bool success = false;
        if(newState == CoworkerState.DESTINATION)
        {
            cowoker.GoToDestination();
            success = true;
        }else if(newState == CoworkerState.IDLE)
        {
            cowoker.Idle();
            success = true;
        }

        if (success)
        {
            state = newState;
        }
        return success || (!success && newState == state);
    }



    private bool updateFromIdle(CoworkerState newState)
    {
        bool success = false;

        if (newState == CoworkerState.DESTINATION)
        {
            cowoker.GoToDestination();
            success = true;
        } 
        if(newState == CoworkerState.WANDER)
        {
            cowoker.Wander();
            success = true; 
        }



        if (success)
        {
            state = newState;
        }
        return success || (!success && newState == state);
    }

}
