using UnityEngine;
using UnityEngine.UI;

public class OneFrameAnimationsControl : MonoBehaviour
{
    [SerializeField] private Sprite[] animationsFrames;
    [SerializeField] private Image image;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool setTransparentWhenStart;
    private int currentFrameIndex;


    private void Start()
    {
        if (setTransparentWhenStart)
            SetFrame(-1);
    }

    public void SetFrame(int frameIndex)
    {

        if(frameIndex == -1)
        {
            if (image != null)
            {
                image.color = new Color(0, 0, 0, 0);
                image.sprite = null;
            }
            else if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(0, 0, 0, 0);
                spriteRenderer.sprite = null;
            }
            else 
                return;

            currentFrameIndex = frameIndex;
                
        }
        else if(animationsFrames.Length >= frameIndex)
        {
            if (image != null)
            {
                image.color = new Color(1, 1, 1, 1);
                image.sprite = animationsFrames[frameIndex];
            }  
            else if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(1,1,1,1);
                spriteRenderer.sprite = animationsFrames[frameIndex];
            }
            else
                return;

            currentFrameIndex = frameIndex;
        }
    }

    public int GetCurrentFrameIndex() => currentFrameIndex;
    public int GetFramesAmount() => animationsFrames.Length;
}
