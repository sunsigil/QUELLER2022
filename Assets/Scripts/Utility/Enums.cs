using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlLayer
{
    WORLD,
    MENU,
    POPUP
}

public enum InputCode
{
    CONFIRM,
    CANCEL,
    ACTION,
    UP,
    DOWN,
    LEFT,
    RIGHT,
    JOURNAL,
    INVENTORY,
    SWITCH_LEFT,
    SWITCH_RIGHT,
    CONSOLE,
    SCROLL_UP,
    SCROLL_DOWN
}

public enum StateSignal
{
    ENTER,
    TICK,
    FIXED_TICK,
    EXIT
}

public enum Faction
{
    HUMAN,
    DEVIL
}

public enum UsableState
{
    DORMANT,
    BLOCKED,
    ACTIVE
}
