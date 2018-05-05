﻿
// CombatSystem.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, EamonRT.Framework.Combat.ICombatSystem
	{
		protected override void PrintHealthStatus()
		{
			// Alt "death" for Hokas, Larkspur, and Cargo-Stealing Lil

			var monsterDies = DfMonster.IsDead();

			if (DfMonster.Uid == 4 && monsterDies)
			{
				if (!BlastSpell)
				{
					Globals.Out.WriteLine();
				}

				Globals.Engine.PrintEffectDesc(149, false);
			}
			else if (DfMonster.Uid == 36 && monsterDies)
			{
				if (!BlastSpell)
				{
					Globals.Out.WriteLine();
				}

				Globals.Engine.PrintEffectDesc(151, false);
			}
			else if (DfMonster.Uid == 37 && monsterDies)
			{
				if (!BlastSpell)
				{
					Globals.Out.WriteLine();
				}

				Globals.Engine.PrintEffectDesc(150, false);
			}
			else
			{
				base.PrintHealthStatus();
			}
		}
	}
}
