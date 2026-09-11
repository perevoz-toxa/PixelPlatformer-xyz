using Assets.Src.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Src.Components
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        private PlayerData _data;

        private void Start()
        {
            var session = FindObjectOfType<GameSession>();
            _data = session.Data.Clone();
        }

        public void Reload()
        {
            var session = FindObjectOfType<GameSession>();
            session.SetData(_data);

            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
