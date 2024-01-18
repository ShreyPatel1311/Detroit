using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoFinishied : MonoBehaviour
{
    [SerializeField] private VideoPlayer vp;

    private int count = 0;

    private void Awake()
    {
        vp = GetComponent<VideoPlayer>();
        vp.Play();
    }

    private void Update()
    {
        if (vp.isPrepared)
        {
            if(count == 0)
            {
                vp.Play();
                count = 1;
            }
            else if(!vp.isPlaying && count == 1)
            {
                SceneManager.LoadScene("Level 1");
            }
        }
    }
}
