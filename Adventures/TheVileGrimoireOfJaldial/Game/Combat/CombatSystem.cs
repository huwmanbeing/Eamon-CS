
// CombatSystem.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.Combat;
using Enums = Eamon.Framework.Primitive.Enums;
using RTEnums = EamonRT.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, ICombatSystem
	{
		protected virtual bool ScoredCriticalHit { get; set; }

		protected override void PrintMiss()
		{
			MissDesc = null;

			var rl = gEngine.RollDice(1, 100, 0);

			if (OfMonster?.Uid == 43)
			{
				var jaldialMonster = OfMonster as Framework.IMonster;

				Debug.Assert(jaldialMonster != null);

				switch (jaldialMonster.AttackDesc)
				{
					case "cast{0} a lightning bolt at":

						MissDesc = "Missed";

						break;

					case "cast{0} an ice bolt at":

						MissDesc = rl > 50 ? "Dodged" : "Missed";

						break;

					case "mentally blast{0}":

						MissDesc = "Ineffective";

						break;
				}
			}

			if (!string.IsNullOrWhiteSpace(MissDesc))
			{
				gOut.Write("{0} --- {1}!", Environment.NewLine, MissDesc);
			}
			else
			{
				base.PrintMiss();
			}
		}

		protected override void RollToHitOrMiss()
		{
			ScoredCriticalHit = false;

			base.RollToHitOrMiss();

			// Bloodnettle always hits when draining blood

			if (OfMonster?.Uid == 20 && DfMonster.Uid == gGameState.BloodnettleVictimUid && _rl > _odds)
			{
				_rl = _odds;
			}
		}

		protected override void CalculateDamage()
		{
			var waterWeirdMonster = gMDB[38] as Framework.IMonster;

			Debug.Assert(waterWeirdMonster != null);

			// Bypass damage calculation and armor check for water weird envelopment

			if (OfMonster?.Uid == 38 && string.Equals(waterWeirdMonster.AttackDesc, "envelop{0}", StringComparison.OrdinalIgnoreCase))
			{
				CombatState = RTEnums.CombatState.CheckMonsterStatus;
			}
			else
			{
				base.CalculateDamage();
			}
		}

		protected override void CheckArmor()
		{
			var waterWeirdMonster = gMDB[38] as Framework.IMonster;

			Debug.Assert(waterWeirdMonster != null);

			// Bypass damage calculation and armor check for water weird envelopment

			if (OfMonster?.Uid == 38 && string.Equals(waterWeirdMonster.AttackDesc, "envelop{0}", StringComparison.OrdinalIgnoreCase))
			{
				CombatState = RTEnums.CombatState.CheckMonsterStatus;
			}
			else
			{
				var room = gRDB[gGameState.Ro];

				Debug.Assert(room != null);

				var artTypes = new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon };

				var immuneMonsterUids = new long[] { 8, 9, 14, 15, 16, 17 };

				var ac = OfWeapon != null ? OfWeapon.GetArtifactCategory(artTypes) : null;

				// Apply special defenses

				if (OfMonster?.Uid != 50 && !BlastSpell)
				{
					// Some monsters are immune to non-magical weapons

					if (immuneMonsterUids.Contains(DfMonster.Uid))
					{
						if (ac == null || ac.Field1 < 20)
						{
							if (DfMonster.IsInRoom(room))
							{
								gOut.Write("{0}{1}Weapon doesn't seem to affect {2}!", Environment.NewLine, OmitBboaPadding ? "" : "  ", room.IsLit() ? DfMonster.GetTheName() : "the unseen defender");
							}

							CombatState = RTEnums.CombatState.EndAttack;

							goto Cleanup;
						}
					}

					// Skeleton and crimson amoeba are resistant to non-club weapons (half damage)

					else if (DfMonster.Uid == 3 || DfMonster.Uid == 25)
					{
						if (ac == null || ac.Field2 != (long)Enums.Weapon.Club)
						{
							_d2 = (long)Math.Round((double)_d2 / 2.0);
						}
					}

					// Water weird is extremely resistant to non-club weapons (minimum damage)

					else if (DfMonster.Uid == 38)
					{
						if (ac == null || ac.Field2 != (long)Enums.Weapon.Club)
						{
							if (_d2 > 1)
							{
								_d2 = 1;
							}
						}
					}
				}

				// Bloodnettle always injures when draining blood

				if (OfMonster?.Uid == 20 && DfMonster.Uid == gGameState.BloodnettleVictimUid && _d2 < 1)
				{
					_d2 = 1;
				}

				base.CheckArmor();

			Cleanup:

				;
			}
		}

		protected override void CheckMonsterStatus()
		{
			var room = DfMonster.GetInRoom();

			Debug.Assert(room != null);

			var rl = gEngine.RollDice(1, 100, 0);

			// Apply special attacks

			if (OfMonster?.Uid == 9)
			{
				if (rl > 50)
				{
					if (room.IsLit() || DfMonster.IsCharacterMonster())
					{
						gOut.Write("{0}  {1}", Environment.NewLine, DfMonster.IsCharacterMonster() ? "You suddenly feel weaker!" : DfMonster.GetTheName(true) + " suddenly looks weaker!");
					}

					var stat = gEngine.GetStats(Stat.Hardiness);

					Debug.Assert(stat != null);

					DfMonster.Hardiness--;

					if (DfMonster.Hardiness < stat.MinValue)
					{
						DfMonster.Hardiness = stat.MinValue;
					}

					if (DfMonster.DmgTaken > DfMonster.Hardiness)
					{
						DfMonster.DmgTaken = DfMonster.Hardiness;
					}
				}
			}
			else if (OfMonster?.Uid == 14 || OfMonster?.Uid == 16)
			{
				if (rl > 60)
				{
					if (room.IsLit() || DfMonster.IsCharacterMonster())
					{
						gOut.Write("{0}  {1}", Environment.NewLine, DfMonster.IsCharacterMonster() ? "You suddenly feel less skillful!" : DfMonster.GetTheName(true) + " suddenly looks less skillful!");
					}

					// Only apply skill loss to the player character

					if (DfMonster.IsCharacterMonster())
					{
						var weaponValues = EnumUtil.GetValues<Weapon>();

						foreach (var wv in weaponValues)
						{
							var weapon = gEngine.GetWeapons(wv);

							Debug.Assert(weapon != null);

							gCharacter.ModWeaponAbilities(wv, -gEngine.RollDice(1, OfMonster?.Uid == 14 ? 4 : 2, 0));

							if (gCharacter.GetWeaponAbilities(wv) < weapon.MinValue)
							{
								gCharacter.SetWeaponAbilities(wv, weapon.MinValue);
							}
						}
					}
				}
			}
			else if (OfMonster?.Uid == 36)
			{
				// TODO
			}
			else if (OfMonster?.Uid == 38)
			{
				var waterWeirdMonster = OfMonster as Framework.IMonster;

				Debug.Assert(waterWeirdMonster != null);

				if (string.Equals(waterWeirdMonster.AttackDesc, "envelop{0}", StringComparison.OrdinalIgnoreCase))
				{
					var saved = DfMonster.IsCharacterMonster() ? gEngine.SaveThrow(Stat.Hardiness) : rl > 40;

					if (!saved)
					{
						if (DfMonster.IsCharacterMonster())
						{
							gOut.Write("{0}  You struggle, but {1} holds you down, drowning you!", Environment.NewLine, room.EvalLightLevel("the offender", OfMonster?.GetTheName()));
						}
						else
						{
							gOut.Write("{0}  {1} is enveloped by {2}, and drowns immediately!", Environment.NewLine, room.EvalLightLevel("The defender", DfMonster.GetTheName(true)), room.EvalLightLevel("the offender", OfMonster?.GetTheName()));
						}

						DfMonster.DmgTaken = DfMonster.Hardiness;
					}
					else
					{
						if (DfMonster.IsCharacterMonster())
						{
							gOut.Write("{0}  You manage to break free!", Environment.NewLine);
						}
						else
						{
							gOut.Write("{0}  {1} manages to break free of {2}'s grip!", Environment.NewLine, room.EvalLightLevel("The defender", DfMonster.GetTheName(true)), room.EvalLightLevel("the offender", OfMonster?.GetTheName()));
						}
					}
				}
			}

			base.CheckMonsterStatus();

			// Bloodnettle selects its next victim

			if (OfMonster?.Uid == 20 && !DfMonster.IsInLimbo() && gGameState.BloodnettleVictimUid == 0)
			{
				gGameState.BloodnettleVictimUid = DfMonster.Uid;
			}

		Cleanup:

			;
		}

		public override void ExecuteAttack()
		{
			var griffinMonster = Globals.MDB[40];

			Debug.Assert(griffinMonster != null);

			// Attacking baby griffins makes the parent angry

			if (DfMonster.Uid == 41 && !griffinMonster.IsInLimbo() && !gGameState.GriffinAngered)
			{
				if (griffinMonster.IsInRoomUid(gGameState.Ro) && griffinMonster.GetInRoom().IsLit())
				{
					gOut.Print("The parent griffin is enraged by your attacks on the griffin cubs!");
				}

				gGameState.GriffinAngered = true;
			}

			base.ExecuteAttack();
		}
	}
}
