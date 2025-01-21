using System;
using Queue;
using Shop;

namespace Character.States
{
    public class TradingState : ICharacterState
    {
        private readonly CharacterController _controller;
        private readonly CharacterAnimator _animator;
        private readonly ShopData _shopData;
        private readonly QueuePoint _queuePoint;
        private readonly Action _onTradeComplete;
        private bool _isTrading;

        public TradingState(
            CharacterController controller,
            CharacterAnimator animator,
            ShopData shopData,
            QueuePoint queuePoint,
            Action onTradeComplete)
        {
            _controller = controller;
            _animator = animator;
            _shopData = shopData;
            _queuePoint = queuePoint;
            _onTradeComplete = onTradeComplete;
        }

        public void Enter()
        {
            _animator.SetIdleAnimation();
            _isTrading = true;
            _shopData.StartTrade(OnTradeFinished);
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            
        }

        private void OnTradeFinished()
        {
            if (_isTrading)
            {
                _isTrading = false;
                _onTradeComplete?.Invoke();
            }
        }
    }
}