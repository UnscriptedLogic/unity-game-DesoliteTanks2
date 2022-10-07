using LevelManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class SceneChanger : MonoBehaviour
    {
        public const int MAIN_MENU = 0;
        public const int GAME_SCENE = 1;

        //[SerializeField] private float transitionTime = 3f;
        [SerializeField] private GameObject transitionPage;

        public void MoveToScene(LevelDetailsSO levelDetail)
        {
            SceneManager.LoadScene(levelDetail.ModeName, LoadSceneMode.Single);
        }

        public void ChangeScene(int sceneIndex)
        {
            SceneManager.LoadSceneAsync(sceneIndex);

            //LeanTween.moveY(transitionPage, 0f, transitionTime).setOnComplete(() =>
            //{
            //    SceneManager.LoadScene(sceneIndex);
            //});
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}