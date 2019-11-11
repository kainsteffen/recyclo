using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class GoopMonster : EnemyBase
{
    public Transform target;
    public Transform shootingPoint;
    public float shootingCooldown;
    public float shootingTimer;
    public float shootForce;

    public SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState animationState;
    public Skeleton skeleton;

    public GameObject projectile;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        shootingTimer = shootingCooldown;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        animationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
        skeletonAnimation.state.SetAnimation(0, "Goopy Goop Goop Idle", true);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (/*target &&*/ shootingTimer < 0)
        {
            Shoot(-Vector2.right);
            StartCoroutine(PlayAnimation());
        }
        else
        {
            shootingTimer -= Time.deltaTime;
        }
    }

    public void Shoot(Vector3 direction)
    {
        GameObject newProjectile = Instantiate(projectile, shootingPoint.position, transform.rotation);
        newProjectile.GetComponent<Rigidbody2D>().AddForce(direction * shootForce);
        shootingTimer = shootingCooldown;
    }

    IEnumerator PlayAnimation()
    {
        var track = skeletonAnimation.state.SetAnimation(0, "Goopy Goop Goop Idle", false);
        yield return new WaitForSpineAnimationComplete(track);
        track = skeletonAnimation.state.SetAnimation(0, "Goopy Goop Spit Start", false);
        yield return new WaitForSpineAnimationComplete(track);
        track = skeletonAnimation.state.SetAnimation(0, "Goopy Goop Spit Mid and End", false);
        yield return new WaitForSpineAnimationComplete(track);
    }

}
