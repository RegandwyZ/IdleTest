using UI;

namespace Citizen.States
{
    public class TradingState : ICitizenState
    {
        private readonly CitizenContext _context;
        private readonly CitizenStateMachine _stateMachine;

        public TradingState(CitizenContext context, CitizenStateMachine stateMachine)
        {
            _context = context;
            _stateMachine = stateMachine;
        }

        public void OnEnter()
        {
            _context.Controller.Animator.SetIdleAnimation();
            _context.CurrentShop.StartTrade(OnTradeComplete);
        }

        public void OnUpdate()
        {
            
        }

        public void OnExit()
        {
            
        }

        private void OnTradeComplete()
        {
            _context.CurrentShop.ReleaseQueuePoint(_context.QueuePoint);
            _context.QueuePoint = null;
            _context.Controller.SmileyController.ShowSmile(SmileType.Happy);
            
            SetNextShop();
            
            _stateMachine.SetState(CitizenState.MoveToCenterPoint);
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