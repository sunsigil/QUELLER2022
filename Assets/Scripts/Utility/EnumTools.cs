using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumTools
{
	public static E[] Elements<E>() where E : Enum
	{ return (E[])Enum.GetValues(typeof(E)); }

	public static string[] Names<E>() where E : Enum
	{ return Enum.GetNames(typeof(E)); }

	public static int Count<E>() where E : Enum
	{ return Elements<E>().Length; }

	public static string ToString<E>(E e) where E : Enum
	{ return Enum.GetName(typeof(E), e); }

	public static E FromString<E>(string s) where E : Enum
	{ return (E)Enum.Parse(typeof(E), s, true); }

	public static int Flag(int flags, Enum flag)
	{ return flags | (1 << Convert.ToInt32(flag)); }

	public static int Unflag(int flags, Enum flag)
	{ return flags & ~(1 << Convert.ToInt32(flag)); }

	public static bool IsFlagged(int flags, Enum flag)
	{ return (flags & (1 << Convert.ToInt32(flag))) > 0; }

	public static string FlagDump<E>(int flags) where E: Enum
	{
		string dump = "";

		foreach(E val in Elements<E>())
		{
			if(IsFlagged(flags, val))
			{
				dump += ToString(val);
				dump += " ";
			}
		}

		return dump;
	}
}
