using UnityEngine;
using System.Threading.Tasks;

namespace BehaviorStates
{
    class SocialState : BehaviorState
    {
        public SocialState(DogPersonalityManager manager) : base(manager)
        {
            _weight = Mathf.Pow(manager.SocialLevel, 2)  * manager.ActivityLevel;
        }

        public override void OnEnter()
        {
           _dogController.LookAt = true;
           _dogController.NavigateToCamera(DogMovementController.MoveType.RUN);
        }

        public override async Task Update()
        {
            Debug.Log("Move Update!");

            do {
                await Task.Delay(15);
            }
            while(!_dogController.ReachedDestination);

            for(int i=0; i<3; i++)
            {
                RandomAction();
                await Task.Delay((int)(Random.Range(5, 15) * 400 * _personalityManager.SocialLevel));
            }
        }

        public void RandomAction()
        {
            if(RandomHelper.RandomBoolean(_personalityManager.ActivityLevel, squaredResponse: true))
            {
                _dogController.IdleBark();
            }
            else
            {
                _dogController.Sit();
            }
        }
    }
}