using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Src.Components
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
