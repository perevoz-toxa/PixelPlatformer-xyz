using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.Src.Components
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
