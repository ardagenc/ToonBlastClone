using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAnimation : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer shadowSpriteRenderer;
    [SerializeField] public Sprite[] frames;
    [SerializeField] float frameRate;
    bool loop = true;
    bool isPlaying = false;

    private int currentFrame = 0;
    private float timer = 0;



    public bool IsPlaying { get => isPlaying; set => isPlaying = value; }

    public void Init(BlockData blockData)
    {
        spriteRenderer.sprite = blockData.defaultSprite;
        shadowSpriteRenderer.sprite = blockData.defaultSprite;
        frames = blockData.frames;
    }
    private void Update()
    {
        if (!IsPlaying) return;
        AnimationLoop(frames, frameRate, loop);            
        
    }
    public void AnimationLoop(Sprite[] frames, float frameRate, bool isLooping)
    {
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                if (isLooping)
                    currentFrame = 0;
                else
                    enabled = false;
            }


            spriteRenderer.sprite = frames[currentFrame];
        }
    }

    public void Play()
    {
        IsPlaying = true;
        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = frames[0];
    }
    public void Stop()
    {
        IsPlaying = false;
        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = frames[0];
    }

    public void SetAnimation(Sprite[] frames, float frameRate, bool isLooping)
    {
        this.frames = frames;
        this.frameRate = frameRate;
        loop = isLooping;
    }

}
