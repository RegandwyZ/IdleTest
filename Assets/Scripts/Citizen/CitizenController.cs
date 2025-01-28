using Queue;
using Shop;
using Systems.PathSystem;
using UI;
using UnityEngine;


namespace Citizen
{
    [RequireComponent(typeof(CitizenMovement))]
    [RequireComponent(typeof(CitizenStateMachine))]
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


        public void ConfigureCitizen()
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
            var context = StateMachine.Context;
            context.Shops = shopData;

            CalculateRandomizedCenterPoint(centerPoint, context);
            StateMachine.SetState(CitizenState.MoveToMarketPlace);
        }

        private void CalculateRandomizedCenterPoint(Vector3 centerPoint, CitizenContext context)
        {
            var randomX = Random.Range(-1.5f, 1.5f);
            var randomZ = Random.Range(-1.5f, 1.5f);

            context.CenterPoint = centerPoint + new Vector3(randomX, 0, randomZ);
        }

        public void SetPathTo(CitizenPath pathToMarket, CitizenPath pathToTrain)
        {
            var context = StateMachine.Context;
            context.PointsToTown = pathToMarket.GetWayPoints();
            context.PointsToTrain = pathToTrain.GetWayPoints();

            context.CurrentPointToTownIndex = 0;
            context.CurrentPointToTrainIndex = 0;

            Animator.SetSpeedAnimation(_moveSpeed);
        }

        public void SetQueuePoint(QueuePoint newQueuePoint)
        {
            var context = StateMachine.Context;
            context.QueuePoint = newQueuePoint;
            StateMachine.SetState(CitizenState.ToQueue);
        }
    }
}