﻿
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		protected virtual IList<IMonster> GetTrapMonsterList(long numMonsters, long roomUid)
		{
			var monsters = Globals.Engine.GetRandomMonsterList(numMonsters, m => m.IsCharacterMonster() || (m.Seen && m.IsInRoomUid(roomUid)));

			Debug.Assert(monsters != null);

			return monsters;
		}

		protected virtual void ApplyTrapDamage(IMonster monster, long numDice, long numSides, bool omitArmor)
		{
			var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
			{
				x.SetNextStateFunc = s => NextState = s;

				x.DfMonster = monster;

				x.OmitArmor = omitArmor;
			});

			combatSystem.ExecuteCalculateDamage(numDice, numSides);
		}

		public override void ProcessEvents(long eventType)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (eventType == PeAfterExtinguishLightSourceCheck)
			{
				var rl = Globals.Engine.RollDice01(1, 100, 0);

				// Spear trap

				if (gameState.Ro == 5 && rl < 20)
				{
					Globals.Engine.PrintEffectDesc(19);

					var monsters = GetTrapMonsterList(1, gameState.Ro);

					foreach (var m in monsters)
					{
						ApplyTrapDamage(m, 1, 6, false);

						if (gameState.Die == 1)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Loose rocks

				if (gameState.Ro == 11 && rl < 33)
				{
					Globals.Engine.PrintEffectDesc(20);

					var monsters = GetTrapMonsterList(1, gameState.Ro);

					foreach (var m in monsters)
					{
						ApplyTrapDamage(m, 1, 4, false);

						if (gameState.Die == 1)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Gas trap

				if (gameState.Ro == 16 && rl < 15)
				{
					Globals.Engine.PrintEffectDesc(21);

					var monsters = GetTrapMonsterList(3, gameState.Ro);

					foreach (var m in monsters)
					{
						ApplyTrapDamage(m, 2, 6, true);

						if (gameState.Die == 1)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Crossbow trap

				if (gameState.Ro == 32 && rl < 51)
				{
					Globals.Engine.PrintEffectDesc(22);

					var monsters = GetTrapMonsterList(1, gameState.Ro);

					foreach (var m in monsters)
					{
						ApplyTrapDamage(m, 1, 8, false);

						if (gameState.Die == 1)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Scything blade trap

				if (gameState.Ro == 57 && rl < 20)
				{
					Globals.Engine.PrintEffectDesc(23);

					var monsters = GetTrapMonsterList(1, gameState.Ro);

					foreach (var m in monsters)
					{
						ApplyTrapDamage(m, 2, 6, false);

						if (gameState.Die == 1)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}
			}

			base.ProcessEvents(eventType);

		Cleanup:

			;
		}
	}
}
