using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorStates;

public class DogPersonalityManager : MonoBehaviour {
    int _activityLevel;
    DogMovementController _bodyController;
    BehaviorState[] _states;
    BehaviorState currentState;

    public float ActivityLevel = .50f;
    public float CuriosityLevel = .40f;
    public float SocialLevel = 0.8f;
    public float ObedienceLevel = 0.2f;

    private bool stateUpdateInProgress = false;

    void Start()
    {
        this._bodyController = GetComponent<DogMovementController>();

        this._states = new BehaviorState[] {
            new MoveState(this),
            new LayState(this),
            new ExploreState(this),
            new SocialState(this)
        };

        currentState = _states[0];
        currentState.OnEnter();
    }

    public async void Update()
    {
        if(!stateUpdateInProgress)
        {
            stateUpdateInProgress = true;

            await currentState.Update();
            SpinTheWheel();

            stateUpdateInProgress = false;

            Debug.Log("Update");
        }
    }


    public DogMovementController GetBodyController()
    {
        return this._bodyController;
    }


    public void SpinTheWheel()
    {
        Debug.Log("WHEEL. OF. FORTUNE!!!");
        print("Current State: " + currentState.GetType().Name);

        ShuffleStates();
        
        foreach(var state in _states)
        {
            if(state != currentState)
            {
                print("State Candidate: " + state.GetType().Name);
                print("State Probability: " + state.Weight);

                var shouldChange = RandomHelper.RandomBoolean(state.Weight, squaredResponse: true);

                print("Jackpot??? " + shouldChange);

                if(shouldChange)
                {
                    currentState.OnExit();

                    state.OnEnter();
                    currentState = state;

                    return;
                }
            }
        }
    }

 
    void ShuffleStates()
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int i = 0; i < _states.Length; i++ )
        {
            BehaviorState tmp = _states[i];
            int r = Random.Range(i, _states.Length);
            _states[i] = _states[r];
            _states[r] = tmp;
        }
    }
}
