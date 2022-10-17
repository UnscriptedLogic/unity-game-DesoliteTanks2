using Core;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LevelManagement
{
    public class LS_LevelSetUp : LevelState
    {
        public LS_LevelSetUp(LevelStateContext context, LevelStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            if (context.PlayerStart != null)
            {
                if (context.PlayerPrefab != null)
                {
                    context.Player = EntityManager.emInstance.CreateEntity(context.PlayerPrefab, "Protectors").transform;
                    context.Player.SetPositionAndRotation(context.PlayerStart.position, context.PlayerStart.rotation);
                    context.Player.gameObject.SetActive(true);
                    
                    context.CameraController.trackTarget = context.Player.transform;
                }
            }

            ExitState();
        }

        public override void UpdateState() { }
        public override void ExitState() => context.StateAfterSetUp();

        protected virtual void LoadMap(string gamemap)
        {
            SceneManager.LoadSceneAsync(gamemap, LoadSceneMode.Additive);
        }

        protected virtual void LoadMap(string gamemap, UnityAction<Scene, LoadSceneMode> OnLevelLoaded)
        {
            SceneManager.LoadSceneAsync(gamemap, LoadSceneMode.Additive);
            SceneManager.sceneLoaded += OnLevelLoaded;
        }
    }
}