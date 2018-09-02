using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MobMelee : Mob
{
    public AudioClip Hit;
    private void BasicLogic()
    {
        if (!dead.Died)
        {
            if (GameObject.Find(Manager.player1))
            {
                if (effectsAboveMob.Stunned && (playerMove.PlayerShooting.counter < effectsAboveMob.StunMoment + playerMove.PlayerShooting.StunDuration()))
                {
                    mobNavigation.nav.destination = transform.position;
                    mobNavigation.nav.enabled = false;
                    mobAvoidCollision.obstacle.enabled = true;
                    animation.Play(Indle.name);
                }
                if (playerMove.PlayerShooting.counter > effectsAboveMob.StunMoment + playerMove.PlayerShooting.StunDuration())
                    effectsAboveMob.Stunned = false;
            }
            if (!effectsAboveMob.Stunned && PlayerInRange(mobfightElements.PullMoba) && !playerHealth.Died)
            {
                if (!turnOnMob) mobNavigation.TurnOnOffMob(true);
                if (PlayerInRange(mobfightElements.fightRange))
                {
                        mobNavigation.TurnOnOffMob(false);
                        if (!playerHealth.Died)
                        {
                            animation.Play(Attack[random].name);
                            mobfightElements.Hitting = true;
                            if (!audioSource.isPlaying && animation[Attack[random].name].time > mobfightElements.timeOfSoundStrike[random]
                                && animation[Attack[random].name].time < mobfightElements.timeOfEndStrike[random] && !mobfightElements.Strike)
                                audioSource.Play();
                            if (animation[Attack[random].name].time > mobfightElements.czasUderzenia[random] && !mobfightElements.Strike)
                            {
                                if (GameObject.Find(Manager.player1))
                                {
                                    if (randomizeChance.Chance(playerMove.ChanceOnBlock)
                                        && !playerMove.animation.IsPlaying(playerMove.PlayerShooting.BladeStorm.name))
                                    {
                                        if (!playerMove.animation.IsPlaying(heroAnimations.punches[playerMove.Number].name))
                                        playerMove.animation.Play(playerMove.shieldBlock.name);
                                    playerHealth.DamageToPlayer(DefenseManager.DamageReduction(damage, playerAttributes.shieldBlockDamageReductionInPercent));
                                        player.gameObject.SendMessageUpwards("PlayerDefence", DefenseManager.DamageReduction(damage, playerAttributes.shieldBlockDamageReductionInPercent) + " (blocked " + (damage- DefenseManager.DamageReduction(damage, playerAttributes.shieldBlockDamageReductionInPercent)) + ")");
                                        playerMove.audioSource.clip = playerMove.sounds.ShieldBlock;
                                        playerMove.audioSource.time = 0;
                                        playerMove.audioSource.Play();
                                    }
                                    else
                                    {
                                        player.gameObject.SendMessageUpwards("DamageToPlayer", damage);
                                        if (!playerMove.animation.IsPlaying(playerMove.PlayerShooting.BladeStorm.name)
                                            && !playerMove.animation.IsPlaying(heroAnimations.punches[playerMove.Number].name))
                                            playerMove.animation.Play(playerMove.impacted.name);

                                    }
                                }
                                if (GameObject.Find(Manager.player))
                                    player.gameObject.SendMessageUpwards("DamageToPlayer", damage);
                                mobfightElements.Strike = true;
                            }
                            if (mobfightElements.Strike && animation[Attack[random].name].time >= animation[Attack[random].name].length - AnimationManager.Percent(animation[Attack[random].name].length, aPercent))
                                random = Random.Range(0, Attack.Length);
                            if (animation[Attack[random].name].time == 0)
                                mobfightElements.Strike = false;
                        }
                        else
                        {
                            turnOnMob = false;
                            mobNavigation.TurnOnOffMob(false);
                            animation.Play(Indle.name);
                        mobAvoidCollision.obstacle.enabled = false;
                        mobNavigation.nav.GetComponent<NavMeshAgent>().enabled = false;
                        }
                }
                else
                    Chase();
            }
            else
            {
                turnOnMob = false;
                mobNavigation.TurnOnOffMob(false);
                animation.Play(Indle.name);
            }
        }
        else
            dead.Dies();
    }
    protected override void Start ()
    {
        melee = true;
        base.Start();
        GetComponent<AudioSource>().clip = Hit;
    }
    void Update ()
    {
        BasicLogic();
    }
}
