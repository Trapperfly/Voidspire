using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BulletType
{
    Bullet,     //Standard bullet, some punch, nothing special, free fire rate
    Laser,      //Innately fast, no punch, free fire rate
    Wave,       //Innately slow, slows down during flight, good punch, pierces, slower fire rate
    Rocket,     //Speeds up during flight, explosion knockback, cannot pierce or bounce, slow fire rate
    Needle,     //Small slow bullets that innately homes, damages target after a few seconds, very fast fire rate
    Railgun,    //Instant hit, leaves a visual trail, pierces, very slow fire rate
    Mine,       //Slows down to a stop during flight, long longevity, cannot pierce or bounce, very slow fire rate
    Hammer,     //Innate spread and multiple bullets, hard punch, very slow fire rate, pushes your ship
    Cluster,    //Slow shot, explodes into spread of bullets during flight, cannot bounce, slow fire rate
    Arrow,      //Fires large metal arrows that stick into targets to adjust their centre of mass, cannot pierce or bounce, slow fire rate
    Mirage,     //
    Grand,      //Large slower shot, very hard punch, very slow fire rate, pushes your ship, amount adds damage instead
    Void,       //No bullet, but creates a pulling singularity at mouse cursor
    Beam,       //Continous beam, bounces off walls
    Blade,      //Shoots blade drones, slight homing, innate bounce towards new targets
}
public class GunType : MonoBehaviour
{
    
    
}
