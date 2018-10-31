using System.Threading.Tasks;

namespace BehaviorStates
{
    abstract class BehaviorState
    {
        protected float _weight;

        public int MAX_WEIGHT = 1;

        protected DogPersonalityManager _personalityManager;

        protected DogMovementController _dogController;

        public float Weight
        {
            get
            {
                return _weight;
            }
        }

        public BehaviorState(DogPersonalityManager personalityController)
        {
            _dogController = personalityController.GetBodyController();
            _personalityManager = personalityController;
        }

        public abstract void OnEnter();

        public virtual void OnExit()
        {

        }

        public virtual async Task Update() {}

        protected void InverseWeight()
        {
            this._weight = MAX_WEIGHT - this._weight;
        }

        protected void SetMinTime(float time)
        {

        }

        protected void RandomlySetLookAt()
        {
            if(_personalityManager.SocialLevel > 0.35f)
            {
                _dogController.LookAt = RandomHelper.RandomBoolean(probabilityOfTrue: _personalityManager.SocialLevel, squaredResponse: true);
            }
            else
            {
                _dogController.LookAt = false;
            }
        }
    }
}