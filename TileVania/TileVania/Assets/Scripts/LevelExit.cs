using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    float time = 1f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            StartCoroutine(DelayLoadScene());
        }
    }

    IEnumerator DelayLoadScene()
    {
        yield return new WaitForSecondsRealtime(time);

        int sceneCurrentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = sceneCurrentIndex + 1;
        
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        FindObjectOfType<ScenePersist>().ResetPersist();
        SceneManager.LoadScene(nextSceneIndex);
    }
}
