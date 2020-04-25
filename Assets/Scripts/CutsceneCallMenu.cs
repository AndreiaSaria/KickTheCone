using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneCallMenu : MonoBehaviour
{
    public void CallTutorial()
    {
        SceneManager.LoadScene(2);
    }
}
