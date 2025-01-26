namespace Citizen.States
{
    public class IdleState : ICitizenState
    {
        private readonly CitizenContext _context;
        private readonly CitizenStateMachine _stateMachine;

        public IdleState(CitizenContext context, CitizenStateMachine stateMachine)
        {
            _context = context;
            _stateMachine = stateMachine;
        }

        public void OnEnter()
        {
            _context.Controller.Animator.SetIdleAnimation();
        }

        public void OnUpdate()
        {
        }

        public void OnExit()
        {
        }
    }
}