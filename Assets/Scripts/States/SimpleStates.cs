using System.Threading.Tasks;
using UnityEngine;

namespace BehaviorStates
{
    class BarkState : BehaviorState
    {
        public BarkState(DogPersonalityManager controler) : base(controler)
        {
            _weight = 0;
        }

        public override void OnEnter()
        {
            
        }
    }

    class IdleState: BehaviorState
    {
        public IdleState(DogPersonalityManager controller): base(controller)
        {
            _weight = 0;
        }

        public override void OnEnter()
        {

        }
    }

    class LayState: BehaviorState
    {
        //TODO: decaying probability as state continues on

        public LayState(DogPersonalityManager controller): base(controller)
        {
            _weight = controller.ActivityLevel;
            InverseWeight();
        }
        
        public override void OnEnter()
        {
            RandomlySetLookAt();
            _dogController.Lay();
        }

        public override async Task Update()
        {
            await Task.Delay((int)(Random.Range(6, 20) * 1200 * _weight));
        }
    }
}