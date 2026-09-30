using UnityEngine;
using UnityEngine.SceneManagement;


namespace Components.LevelManagement
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _nextSceneName;
        public void Exit()
        {
            SceneManager.LoadScene(_nextSceneName);
        }
    }
}
