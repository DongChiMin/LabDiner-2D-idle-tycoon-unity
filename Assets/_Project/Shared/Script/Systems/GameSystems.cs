using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LabDiner.Shared
{
    public class GameSystems : Singleton<GameSystems>
    {
        [Header("Sub Systems")]
        public LevelLoader levelLoader;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            // Gọi khởi tạo cho các hệ thống con ở đây
            levelLoader.Init();
            
            // Sau khi xong xuôi thì vào game
            UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
        }
    }
}
