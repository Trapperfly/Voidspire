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
    Custom, //Lower min-zoom, higher max-zoom, faster update speed. Available map channels: general and friendly
    Lookout, //High max-zoom, slower update speed. Available map channels: Most of the general ones (Mining, action, etc)
    Radio, //General zoom. Available maps: Most of them, plus some special (Diplomacy)
    Gamma, //Generally bad zoom. Available maps: Only the best ones (Diplomacy, broad (all), )
    Default
}

public enum Frequencies
{
    //Friendly, //Just combined and civilization
    General, //Factions and combined
    Factions, //Factions and individual factions
    Transmitters, //Events and stores
    //Mining, //Yield and debris
    //Action, //Events and factions
    Diplomat, //Civ, event, shop, pirate
    Broad, //Combined, factions, shops, events, civ, debris
    Default
}
