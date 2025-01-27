using UnityEngine;

namespace Citizen.States
{
    public class MoveToTrainState : ICitizenState
    {
        private readonly CitizenContext _context;
        private readonly CitizenStateMachine _stateMachine;

        public MoveToTrainState(CitizenContext context, CitizenStateMachine stateMachine)
        {
            _context = context;
            _stateMachine = stateMachine;
        }

        public void OnEnter()
        {
            if (_context.PointsToTrain == null || _context.PointsToTrain.Length == 0)
                return;

            var baseTrainPoint = _context.PointsToTrain[^1].position;

            var randomX = Random.Range(-2f, -10f);
            var randomZ = Random.Range(-1f, 1f);
            _context.TargetTrainNearPoint = baseTrainPoint + new Vector3(randomX, 0, randomZ);
        }

        public void OnUpdate()
        {
            if (_context.CurrentPointToTrainIndex >= _context.PointsToTrain.Length)
            {
                _stateMachine.SetState(CitizenState.MoveToTrainPlacePoint);
                return;
            }

            var target = _context.PointsToTrain[_context.CurrentPointToTrainIndex].position;
            bool reached = _context.Controller.Movement.MoveTo(target);

            if (reached)
            {
                _context.CurrentPointToTrainIndex++;

                if (_context.CurrentPointToTrainIndex >= _context.PointsToTrain.Length)
                {
                    _stateMachine.SetState(CitizenState.MoveToTrainPlacePoint);
                }
            }
        }

        public void OnExit()
        {
        }
    }
}