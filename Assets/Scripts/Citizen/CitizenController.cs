
using PathSystem;
using Queue;
using Shop;
using UI;
using UnityEngine;



namespace Citizen
{
    public class CitizenController : MonoBehaviour
    {
        public bool IsReadyToLeave { get; set; }

        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private SmileyController _smileyController;
        [SerializeField] private CitizenAnimator _animator;
        
        public CitizenMovement Movement { get; private set; }
        private CitizenStateMachine StateMachine { get; set; }
        public SmileyController SmileyController => _smileyController;
        public CitizenAnimator Animator => _animator;
        public float MoveSpeed => _moveSpeed;
        public float RotationSpeed => _rotationSpeed;

        private void Awake()
        {
            Movement = GetComponent<CitizenMovement>();
            StateMachine = GetComponent<CitizenStateMachine>();
            
            Movement.Init(this);
            
            StateMachine.Init(this);
        }

        private void Update()
        {
            StateMachine.UpdateState();
        }
        
        public void SetData(ShopData[] shopData, Vector3 centerPoint)
        {
            StateMachine.SetData(shopData, centerPoint);
        }
        
        public void SetPathTo(CitizenPath citizenPathToMarket, CitizenPath citizenPathToTrain)
        {
            StateMachine.SetPathTo(citizenPathToMarket, citizenPathToTrain);
        }
        
        public void SetQueuePoint(QueuePoint newQueuePoint)
        {
            StateMachine.SetQueuePoint(newQueuePoint);
        }
    }
}