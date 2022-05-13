using UnityEngine;

namespace Tilevania
{
    public class ScenePersist : MonoBehaviour
    {
        private void Awake()
        {
            var numScenePersists = FindObjectsOfType<ScenePersist>().Length;
        
            if (numScenePersists > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        
        public void ResetScenePersist()
        {
            Destroy(gameObject);
        }
    }
}