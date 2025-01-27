namespace Citizen.States
{
    public class MoveToTrainPlaceState : ICitizenState
    {
        private readonly CitizenContext _context;
        private readonly CitizenStateMachine _stateMachine;

        public MoveToTrainPlaceState(CitizenContext context, CitizenStateMachine stateMachine)
        {
            _context = context;
            _stateMachine = stateMachine;
        }

        public void OnEnter()
        {
        }

        public void OnUpdate()
        {
            var reached = _context.Controller.Movement.MoveTo(_context.TargetTrainNearPoint);
            if (reached)
            {
                _stateMachine.SetState(CitizenState.Idle);
                _context.Controller.IsReadyToLeave = true;
            }
        }

        public void OnExit()
        {
        }
    }
}