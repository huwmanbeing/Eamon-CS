﻿
// AttackCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		protected virtual Classes.IArtifactCategory Ac { get; set; }

		protected virtual long Damage { get; set; }

		protected virtual void ProcessWallAttack()
		{
			Ac = DobjArtifact.GetCategories(0);

			Debug.Assert(Ac != null);

			var whereClauseFuncs = Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) <= 0 ?
				new Func<IMonster, bool>[] { m => m == ActorMonster, m => m.IsInRoom(ActorRoom) && m.Friendliness == Enums.Friendliness.Friend && ((m.Weapon > -1 && m.Weapon <= Globals.Database.GetArtifactsCount()) || m.CombatCode == Enums.CombatCode.NaturalWeapons) && m != ActorMonster } :
				new Func<IMonster, bool>[] { m => m == ActorMonster };

			var monsters = Globals.Engine.GetMonsterList(() => true, whereClauseFuncs);

			for (var i = 0; i < monsters.Count; i++)
			{
				var monster = monsters[i];

				Globals.Out.Write("{0}{1} {2}{3} the {4}!{5}",
					Environment.NewLine,
					monster == ActorMonster ? "You" : monster.GetDecoratedName03(true, true, false, true, Globals.Buf),
					monster == ActorMonster && BlastSpell ? "blast" : "attack",
					monster == ActorMonster ? "" : "s",
					DobjArtifact.Uid == 83 ? "back wall" : "glass walls",
					true /* monster == ActorMonster || i == monsters.Count - 1 */ ? Environment.NewLine : "");

				var dice = 0L;

				var sides = 0L;

				if (monster == ActorMonster && BlastSpell)
				{
					dice = 2;

					sides = 5;
				}
				else
				{
					var weapon = monster.Weapon > 0 ? Globals.ADB[monster.Weapon] : null;

					var wpnAc = weapon != null ? weapon.GetCategories(0) : null;

					dice = wpnAc != null ? wpnAc.Field3 : monster.NwDice;

					sides = wpnAc != null ? wpnAc.Field4 : monster.NwSides;
				}

				Damage += Globals.Engine.RollDice01(dice, sides, 0);
			}
		}

		protected override void PlayerExecute()
		{
			var effectUid = 0L;
			var n = 0L;

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if ((BlastSpell || ActorMonster.Weapon > 0) && DobjArtifact != null)
			{
				switch (DobjArtifact.Uid)
				{
					case 83:

						// Fake-looking back wall

						ProcessWallAttack();

						if (Damage > 19)
						{
							Damage = 19;
						}

						Ac.Field4 -= Damage;

						n = (long)Math.Round((double)Ac.Field4 / 20);

						effectUid = 63 - n * (n > 0 ? 1 : 0);

						Globals.Engine.PrintEffectDesc(effectUid);

						// First attack

						if (!gameState.FloorAttack)
						{
							Globals.Engine.PrintEffectDesc(64);

							var electrifiedFloorArtifact = Globals.ADB[85];

							Debug.Assert(electrifiedFloorArtifact != null);

							electrifiedFloorArtifact.SetInRoom(ActorRoom);

							gameState.FloorAttack = true;
						}

						// Broken!

						if (effectUid == 63)
						{
							Ac.Field4 = 0;

							DobjArtifact.SetInLimbo();

							var engravingArtifact = Globals.ADB[2];

							Debug.Assert(engravingArtifact != null);

							engravingArtifact.SetInLimbo();

							var rubbleArtifact = Globals.ADB[106];

							Debug.Assert(rubbleArtifact != null);

							rubbleArtifact.SetInRoom(ActorRoom);
						}

						NextState = Globals.CreateInstance<IMonsterStartState>();

						break;

					case 84:

						// Glass walls

						ProcessWallAttack();

						if (Damage > 10)
						{
							Damage = 10;
						}

						Ac.Field4 -= Damage;

						n = (long)Math.Round((double)Ac.Field4 / 10);

						effectUid = 69 - n * (n > 0 ? 1 : 0);

						Globals.Engine.PrintEffectDesc(effectUid);

						// Broken!

						if (effectUid == 69)
						{
							Ac.Field4 = 0;

							DobjArtifact.SetInLimbo();

							var ovalDoorArtifact = Globals.ADB[16];

							Debug.Assert(ovalDoorArtifact != null);

							ovalDoorArtifact.SetInLimbo();

							var shatteredGlassWallsArtifact = Globals.ADB[105];

							Debug.Assert(shatteredGlassWallsArtifact != null);

							shatteredGlassWallsArtifact.SetInRoom(ActorRoom);

							gameState.Sterilize = false;

							Globals.Engine.PrintEffectDesc(70);

							Globals.Out.Print("Enemies storm into the room!");

							var monsters = Globals.Engine.GetMonsterList(() => true, m => m.Uid >= 20 && m.Uid <= 22);

							foreach (var monster in monsters)
							{
								monster.SetInRoom(ActorRoom);
							}

							Globals.Engine.CheckEnemies();

							NextState = Globals.CreateInstance<IStartState>();
						}
						else
						{
							NextState = Globals.CreateInstance<IMonsterStartState>();
						}

						break;

					case 85:

						// Electrified floor

						Globals.Engine.PrintEffectDesc(65);

						DobjArtifact.SetInLimbo();

						var brokenFloorTrapArtifact = Globals.ADB[107];

						Debug.Assert(brokenFloorTrapArtifact != null);

						brokenFloorTrapArtifact.SetInRoom(ActorRoom);

						NextState = Globals.CreateInstance<IMonsterStartState>();

						break;

					case 89:

						// Dismantled worker android

						base.PlayerExecute();

						var plasticCardArtifact = Globals.ADB[82];

						Debug.Assert(plasticCardArtifact != null);

						if (plasticCardArtifact.IsInLimbo())
						{
							plasticCardArtifact.Desc = "Destroying the remains of the android reveals a small featureless card made out of a durable plastic.";

							Globals.Out.Print("{0}", plasticCardArtifact.Desc);

							plasticCardArtifact.SetInRoom(ActorRoom);

							plasticCardArtifact.Seen = true;
						}

						break;

					default:

						base.PlayerExecute();

						break;
				}
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
