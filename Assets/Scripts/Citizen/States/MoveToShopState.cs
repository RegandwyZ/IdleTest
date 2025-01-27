using UI;

namespace Citizen.States
{
    public class MoveToShopState : ICitizenState
    {
        private readonly CitizenContext _context;
        private readonly CitizenStateMachine _stateMachine;

        public MoveToShopState(CitizenContext context, CitizenStateMachine stateMachine)
        {
            _context = context;
            _stateMachine = stateMachine;
        }

        public void OnEnter()
        {
            _context.PointsToShop = _context.CurrentShop.GetShopPoints();
            _context.CurrentPointToShopIndex = 0;
        }

        public void OnUpdate()
        {
            var index = _context.CurrentPointToShopIndex;
            var shopPoints = _context.PointsToShop;

            bool reached = _context.Controller.Movement.MoveTo(shopPoints[index].position);
            if (reached)
            {
                _context.CurrentPointToShopIndex++;

                if (_context.CurrentPointToShopIndex >= shopPoints.Length)
                {
                    var queue = _context.CurrentShop.GetQueuePoint(_context.Controller);
                    _context.QueuePoint = queue;

                    if (queue == null)
                    {
                        _context.Controller.SmileyController.ShowSmile(SmileType.Angry);
                        SetNextShop();
                        _stateMachine.SetState(CitizenState.MoveToCenterPoint);
                    }
                    else
                    {
                        _stateMachine.SetState(CitizenState.ToQueue);
                    }
                }
            }
        }

        public void OnExit()
        {
        }

        private void SetNextShop()
        {
            if (_context.CurrentShopIndex < _context.Shops.Length)
            {
                _context.CurrentShop = _context.Shops[_context.CurrentShopIndex];
                _context.CurrentShopIndex++;
            }
            else
            {
                _context.IsVisitedAllShops = true;
            }
        }
    }
}