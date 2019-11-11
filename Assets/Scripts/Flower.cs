using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class Flower : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState animationState;
    public Skeleton skeleton;

    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        animationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayAnimation()
    {
        TrackEntry track = skeletonAnimation.state.SetAnimation(0, "Bloom Start", false);
        yield return new WaitForSpineAnimationComplete(track);
        skeletonAnimation.state.SetAnimation(0, "Bloom Cycle", true);
    }
}
