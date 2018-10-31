using UnityEngine;

namespace BehaviorStates
{
    class ExploreState : MoveState
    {
        bool origLookAtValue = true;

        public ExploreState(DogPersonalityManager manager) : base(manager)
        {
            _weight = Mathf.Pow(_personalityManager.CuriosityLevel,3) *  manager.ActivityLevel;
        }

        public override void OnEnter()
        {
            origLookAtValue = _dogController.LookAt; //disable look at during explore mdoe
            _dogController.LookAt = false;

            moveType = DogMovementController.MoveType.SNIFF_AND_WALK;
        }

        public override void OnExit()
        {
            _dogController.LookAt = origLookAtValue; //restore look at
        }
    }
}