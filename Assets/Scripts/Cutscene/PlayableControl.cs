using UnityEngine.Playables;
using UnityEngine;

public class PlayableControl : MonoBehaviour //Isso serve para resetar a timeline caso o player a queira rever
{
    //https://forum.unity.com/threads/reset-timeline-from-code.503904/ não funciona

    public PlayableDirector director;

    private void Start()
    {
        director.initialTime = 0;
        director.time = 0;
        director.Stop();
        director.Evaluate();
        director.Play();
    }

}
