using UnityEngine;
using System.Threading.Tasks;

namespace BehaviorStates
{
    class MoveState : BehaviorState
    {
        protected DogMovementController.MoveType moveType;

        public MoveState(DogPersonalityManager personalityManager) : base(personalityManager)
        {
            _weight = personalityManager.ActivityLevel;
        }

        public override void OnEnter()
        {
            RandomlySetLookAt();
            
            if(_personalityManager.ObedienceLevel < 0.5f && RandomHelper.RandomBoolean(1f - _personalityManager.ObedienceLevel, squaredResponse: true))
            {
                moveType = DogMovementController.MoveType.BUTT_WIPE;
            }
            else if(_weight > 0.4f && RandomHelper.RandomBoolean(_weight, squaredResponse: true)) //maximize range as it increases
            {
                moveType = DogMovementController.MoveType.RUN;
            }
            else moveType = DogMovementController.MoveType.WALK;
        }

        public override async Task Update()
        {
            Debug.Log("Move Update!");

            if(_dogController.ReachedDestination)
            {
                //random delay
                int delay = (int)((1000) * Random.Range(3, 10) * (MAX_WEIGHT-Weight)); //longer = lazier
                await Task.Delay(delay);

                _dogController.RandomMove(moveType);
            }

            while(!_dogController.ReachedDestination) { 
                await Task.Delay(15);
            }

            await OnDestinationReached();

            Debug.Log("Done movin");
        }

        protected virtual async Task OnDestinationReached()
        {
            if(RandomHelper.RandomBoolean(_personalityManager.CuriosityLevel * (1f - _personalityManager.ActivityLevel), sqrtResponse: true))
            {
                _dogController.Sniff();

                await Task.Delay((int)(150 * Random.Range(4, 40) * _personalityManager.CuriosityLevel * (1f-_personalityManager.ActivityLevel)));
            }
        }
    }
}