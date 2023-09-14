using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Prefix
{
    Gatling,    //Ramps up firerate AND spread when trigger is held
    Precise,    //Gets more precise when trigger is held, when max, pierce
    Homing,     //Shots home aggressively
    Swarm,      //Smaller and more shots that home slightly
    Piercing,   //Pierces infinite targets
    Bore,       //Pierces few targets but damage is multiplied per pierce, initial damage reduced
    TwoBurst,   //Shoots in bursts of two with little delay per burst
    SevenBurst, //Shoots in bursts of seven with long delay per burst
    Shotgun,    //Shoots many shots at once in an arc
    Bouncy,     //Shots bounce on objects and looses no speed on bounce
    BounceShot, //Shots bounce to other targets, but gets smaller and weaker on bounce
    Expanding,  //Shots get larger and more damaging during flight
    Fusion,     //Long charge-up with concentrated burst shot
    Flurry,     //Large spread with more projectiles
    Shooter,    //Fires as fast as you can pull the trigger
    Grim,       //Ramps up damage and speed the longer the trigger is held, release to fire
    Frenzy,     //Charge up and fire in rapid succession
    Dual,       //Each shot has a paralell shot
    Fiery,      //Last target hit is ignited
    Inferno,    //Every target is ignited, reduce damage, increased firerate
}
public class GunPrefix : MonoBehaviour
{
    
}
