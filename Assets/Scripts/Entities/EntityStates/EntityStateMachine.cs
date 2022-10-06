using Grid.Pathfinding;
using UnityEngine;

namespace Entities
{
    public class EntityStateMachine : Entity, IListensToGameState
    {
        protected Transform playerRef;
        protected Vector3 targetLocation;

        [Header("State Machine Extension")]
        [SerializeField] protected string chaseTag = "player";
        [SerializeField] protected float speed;
        [SerializeField] protected EntityState currentState;
        [SerializeField] protected EntityAttack attackScript;
        [SerializeField] protected BaseHealthClass healthScript;
        [SerializeField] protected Rigidbody rb;
        [SerializeField] protected LayerMask entityLayer;
        [SerializeField] protected Color pathGizmoColor;

        [SerializeField] protected TerrainWeights[] defaultTerrainWeights;

        protected EffectsManager audioManager;

        public float Speed => speed;

        //Perhaps not a good thing but if we ever want to inflict interrupting states like stunning,
        //we could just set it here
        public EffectsManager AudioManager => audioManager;
        public TerrainWeights[] DefaultTerrainWeights => defaultTerrainWeights;
        public EntityState CurrentState { get => currentState; set { currentState = value; } }
        public Rigidbody RigidbodyContext => rb;
        public EntityAttack AttackScript => attackScript;
        public BaseHealthClass HealthScript => healthScript;
        public Transform Target { get => playerRef; set { playerRef = value; } }
        public LayerMask EntityLayer => entityLayer;
        public Vector3 TargetLocation { get => targetLocation; set { targetLocation = value; } }
        public Color PathGizmoColor => pathGizmoColor;
        public string ChaseTag => chaseTag;
        public bool IsTargetAlive => playerRef.gameObject.activeInHierarchy;

        protected override void Start()
        {
            base.Start();
            audioManager = EffectsManager.instance;
        }

        private void OnEnable()
        {
            currentState.EnterState(this);
        }

        private void Update()
        {
            currentState.UpdateState();
        }

        private void FixedUpdate()
        {
            currentState.FixedUpdateState();
        }

        public void SetCurrentState(EntityState newState)
        {
            currentState.ExitState();
            currentState = newState;
            currentState.EnterState(this);
        }

        public void OnGameStateChanged(bool won)
        {
            enabled = false;
        }
    }
}