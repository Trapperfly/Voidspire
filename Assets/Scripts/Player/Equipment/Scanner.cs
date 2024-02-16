using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Scanner", menuName = "Inventory/Equipment/Scanner")]
public class Scanner : Equipment
{
    public ScannerTypes type;
    public Frequencies frequency;
    public Vector2Int zoom;
    public float mapUpdateSpeed;
    public int mapUpdateAmount;


}

public enum ScannerTypes
{
    Custom,
    Lookout,
    Radio,
    Gamma,
    Default
}

public enum Frequencies
{
    Friendly, //Just combined and civilization
    General, //Factions and combined
    Factions, //Factions and individual factions
    Transmitters, //Events and stores
    Mining, //Yield and debris
    Action, //Events and factions
    Diplomat, //Civ, event, shop, pirate
    Broad, //Combined, factions, shops, events, civ, debris
    Default
}
