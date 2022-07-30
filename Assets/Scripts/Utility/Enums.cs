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
    UP,
    DOWN,
    LEFT,
    RIGHT,
	JUMP,
	RHAND,
	LHAND,
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
