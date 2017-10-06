﻿
// RegistrationDeskMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using Eamon.Game.Utilities;
using EamonMH.Framework.Menus.ActionMenus;
using EamonMH.Framework.Menus.HierarchicalMenus;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class RegistrationDeskMenu : Menu, IRegistrationDeskMenu
	{
		protected virtual double? Rtio { get; set; }

		protected virtual string AdventureName { get; set; }

		protected virtual long NumChances { get; set; }

		protected virtual void BadCharacterName()
		{
			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			NumChances--;

			Globals.Out.Write("{0}{1}{0}",
				Environment.NewLine,
				NumChances == 12 ? "He pulls out a sword and begins to sharpen it, saying \"Ye'd best be givin' me yer name laddie, if ye know wots good fer ye!!!\"" :
				NumChances == 11 ? "\"I've 'bout had me fill o' yer sick sensa 'umor!!  Now gimme yer name!!\"" :
				NumChances == 10 ? string.Format("The man cuts one of your fingers off!!  He then eats it!!!!{0}{0}Then he says, \"Are ye ready t' talk now?\"", Environment.NewLine) :
				NumChances > 0 ? "The man cuts off another finger!!!  He eats this one too!!" :
				string.Format("The man starts slowly, \"Well, ye be outta fingers!\"{0}{0}The man then spins around and runs you through with a speed you have never seen before!  (and never will again.)", Environment.NewLine));
		}

		protected virtual void RecallCharacterFromAdventure()
		{
			RetCode rc;

			var filesets = Globals.Database.FilesetTable.Records;

			foreach (var fileset in filesets)
			{
				rc = Globals.PushDatabase();

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (!string.IsNullOrWhiteSpace(fileset.FilesetFileName) && !string.Equals(fileset.FilesetFileName, "NONE", StringComparison.OrdinalIgnoreCase))
				{
					var fsfn = Globals.Path.Combine(fileset.WorkDir, fileset.FilesetFileName);

					rc = Globals.Database.LoadFilesets(fsfn, printOutput: false);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					RecallCharacterFromAdventure();
				}
				else
				{
					var chrfn = Globals.Path.Combine(fileset.WorkDir, "FRESHMEAT.XML");

					if (Globals.File.Exists(Globals.GetPrefixedFileName(chrfn)))
					{
						rc = Globals.Database.LoadCharacters(Globals.GetPrefixedFileName(chrfn), printOutput: false);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						var character = Globals.Database.CharacterTable.Records.FirstOrDefault();

						if (character != null && character.Uid == Globals.Character.Uid && string.Equals(character.Name, Globals.Character.Name, StringComparison.OrdinalIgnoreCase))
						{
							AdventureName = fileset.Name;

							Globals.Character.Status = Enums.Status.Alive;

							Globals.CharactersModified = true;

							Globals.TransferProtocol.RecallCharacterFromAdventure(fileset.WorkDir, Globals.FilePrefix, fileset.PluginFileName);
						}
					}
				}

				rc = Globals.PopDatabase();

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (Globals.Character.Status != Enums.Status.Adventuring)
				{
					break;
				}
			}
		}

		protected virtual void CreateCharacter(ICharacter character, IValidateArgs args)
		{
			RetCode rc;

			Debug.Assert(character != null && args != null);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Globals.Out.Write("{0}He hits his forehead and says, \"Ah, ye must be new here!  Well, wait just a minute and I'll bring someone out to take care of ye.\"{0}", Environment.NewLine);

			Globals.Out.Write("{0}The Irishman says, \"First I must know whether ye be male or female.  Which are ye?\"{0}", Environment.NewLine);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Globals.Out.Write("{0}You give him your gender ({1}=Male, {2}=Female): ",
				Environment.NewLine,
				(long)Enums.Gender.Male,
				(long)Enums.Gender.Female);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, null, Globals.Engine.IsChar0Or1, Globals.Engine.IsChar0Or1);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			character.Gender = (Enums.Gender)Convert.ToInt64(Buf.Trim().ToString());

			Debug.Assert(character.ValidateField(character.GetField("Gender"), args));

			Globals.Thread.Sleep(150);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			character.Uid = Globals.Database.GetCharacterUid();

			character.IsUidRecycled = true;

			character.Status = Enums.Status.Alive;

			var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

			for (var i = 0; i < weaponValues.Count; i++)
			{
				var wv = weaponValues[i];

				var weapon = Globals.Engine.GetWeapons(wv);

				Debug.Assert(weapon != null);

				character.SetWeaponAbilities(wv, Convert.ToInt64(weapon.EmptyVal));
			}

			character.HeldGold = 200;

			Globals.Out.Write("{0}The Irishman walks away and in walks a tall man of possibly Elvish descent.{0}{0}He studies you for a moment and says, \"Here is a booklet of instructions for you to read, and your prime attributes are--{0}", Environment.NewLine);

			var statValues = EnumUtil.GetValues<Enums.Stat>();

			while (true)
			{
				for (var i = 0; i < statValues.Count; i++)
				{
					var sv = statValues[i];

					var stat = Globals.Engine.GetStats(sv);

					Debug.Assert(stat != null);

					Globals.Engine.RollDice(3, 8, 0, ref character.Stats[(long)sv]);

					Globals.Out.Write("{0}{1,27}{2}{3}",
						Environment.NewLine,
						string.Format("{0}: ", stat.Name),
						character.GetStats(sv),
						i == statValues.Count - 1 ? string.Format("\"{0}", Environment.NewLine) : "");
				}

				if (character.GetStats(Enums.Stat.Intellect) + character.GetStats(Enums.Stat.Hardiness) + character.GetStats(Enums.Stat.Agility) < 39 || character.Stats.Sum() < 52)
				{
					Globals.Out.Write("{0}\"You are such a poor excuse for an adventurer that we will allow you to commit suicide.\"{0}", Environment.NewLine);

					Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

					Globals.Out.Write("{0}Press Y to commit suicide or N to continue: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Thread.Sleep(150);

					if (Buf.Length > 0 && Buf[0] == 'Y')
					{
						Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

						Globals.Out.Write("{0}\"We resurrect you again and your prime attributes are--{0}", Environment.NewLine);
					}
					else
					{
						break;
					}
				}
				else
				{
					break;
				}
			}

			rc = Globals.Database.AddCharacter(character);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.CharactersModified = true;

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Globals.Out.Write("{0}Press R to read instructions or T to give them back: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.MhEngine.IsCharROrT, Globals.MhEngine.IsCharROrT);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			if (Buf.Length > 0 && Buf[0] == 'R')
			{
				Globals.Out.WriteLine("{0}You read the instructions and they say--{0}", Environment.NewLine);

				Globals.Engine.PrintTitle("INFORMATION ABOUT THE WORLD OF EAMON", false);

				Globals.Out.Write("{0}You will have to buy a weapon.  Your chance to hit with it will be determined by the weapon complexity, your ability in that weapon class, how heavy your armor is, and the difference in agility between you and your enemy.{0}{0}The five classes of weapons (and your current abilities with each) are--{0}", Environment.NewLine);

				Globals.Out.Write("{0}{1,28}", Environment.NewLine, "Club/Mace......20%");

				Globals.Out.Write("{0}{1,28}", Environment.NewLine, "Spear..........10%");

				Globals.Out.Write("{0}{1,28}", Environment.NewLine, "Axe.............5%");

				Globals.Out.Write("{0}{1,28}", Environment.NewLine, "Sword...........0%");

				Globals.Out.WriteLine("{0}{1,28}", Environment.NewLine, "Bow...........-10%");

				Globals.Out.Write("{0}Every time you score a hit in battle, your ability in the weapon class may go up by 2%, if a random number from 1-100 is less than your chance to miss!{0}", Environment.NewLine);

				Globals.In.KeyPress(Buf);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}There are four armor types and you may also carry a shield if you do not use a two-handed weapon.  These protections will absorb hits placed upon you (almost always!) but they lower your chance to hit.  The protections are--{0}", Environment.NewLine);

				Globals.Out.Write("{0}{1,61}", Environment.NewLine, "Armor          Hits Absorbed        Odds Adjustment");

				Globals.Out.Write("{0}{1,61}", Environment.NewLine, "---------------------------------------------------");

				Globals.Out.Write("{0}{1,56}", Environment.NewLine, "None .................0 ....................0%");

				Globals.Out.Write("{0}{1,56}", Environment.NewLine, "Leather ..............1 ..................-10%");

				Globals.Out.Write("{0}{1,56}", Environment.NewLine, "Chain ................2 ..................-20%");

				Globals.Out.Write("{0}{1,56}", Environment.NewLine, "Plate ................5 ..................-60%");

				Globals.Out.WriteLine("{0}{1,56}", Environment.NewLine, "Shield ...............1 ...................-5%");

				Globals.Out.Write("{0}You will develop an Armor Expertise, which will go up when you hit a blow wearing armor and your expertise is less than the armor you are wearing.  No matter how high your Armor Expertise is, however, the net effect of armor will never increase your chance to hit.{0}", Environment.NewLine);

				Globals.In.KeyPress(Buf);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}You can carry weight up to ten times your hardiness, or, {1} Gronds.  (A measure of weight, one Grond = 10 Dos.){0}{0}Additionally, your hardiness tells how many points of damage you can survive.  Therefore, you can be hit with {2} 1-point blows before you die.{0}{0}You will not be told how many blows you have taken.  You will be merely told such things as--{0}{0}   \"Wow!  That one hurt!\"{0}or \"You don't feel very well.\"{0}{0}Your charisma ({3}) affects how the citizens of Eamon react to you.  You affect a monster's friendliness rating by your charisma less ten, difference times two ({4}%).{0}{0}You start off with 200 gold pieces, which you will want to spend on supplies for your first adventure.  You will get a lower price for items if your charisma is high.{0}",
					Environment.NewLine,
					character.GetWeightCarryableGronds(),
					character.GetStats(Enums.Stat.Hardiness),
					character.GetStats(Enums.Stat.Charisma),
					character.GetCharmMonsterPct());

				Globals.In.KeyPress(Buf);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}After you begin to accumulate wealth, you may want to put some of your money into the bank, where it cannot be stolen.  However it is a good idea to always carry some gold with you for use in bargaining and ransom situations.{0}{0}You should also hire a Wizard to teach you some magic spells.  Your intellect ({1}) affects your ability to learn both skills and spells.  There are four spells you can learn--{0}{0}Blast: Throw a magical blast at your enemies to inflict damage.{0}Heal : Remove damage from your body.{0}Speed: Double your agility for a short time.{0}Power: This unpredictable spell is different in each adventure.{0}{0}Other types of spells may be available in various adventures, and items may have special properties.  However, these will only work in the adventure where they were found.  Thus it is best (and you have no choice but to) sell all items found in adventures except for weapons and armor.{0}",
					Environment.NewLine,
					character.GetStats(Enums.Stat.Intellect));

				Globals.In.KeyPress(Buf);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}

			Globals.Out.Write("{0}The man behind the desk takes back the instructions and says, \"It is now time for you to start your life.\"  He makes an odd sign with his hand and says, \"Live long and prosper.\"{0}{0}You now wander into the Main Hall...{0}", Environment.NewLine);

			Globals.In.KeyPress(Buf);

			Globals.Character = character;
		}

		protected virtual void SelectCharacter()
		{
			RetCode rc;

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Globals.Out.Write("{0}You are greeted there by a burly Irishman who looks at you with a scowl and asks you, \"What's yer name?\"{0}", Environment.NewLine);

			var character = Globals.CreateInstance<ICharacter>();

			var args = Globals.CreateInstance<IValidateArgs>();

			var menu = Globals.CreateInstance<IMainHallMenu>();

			var effect = null as IEffect;

			while (true)
			{
				Rtio = null;

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}You give him your name (type it in now): ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.CharNameLen, null, ' ', '\0', false, null, null, Globals.Engine.IsCharAnyButDquoteCommaColon, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				character.Name = Buf.Trim().ToString();

				Globals.Thread.Sleep(150);

				if (character.ValidateField(character.GetField("Name"), args))
				{
					Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

					if (effect == null)
					{
						long effectUid = 0;

						Globals.Engine.RollDice(1, Globals.Database.GetEffectsCount(), 0, ref effectUid);

						effect = Globals.EDB[effectUid];
					}

					Globals.Out.Write("{0}He starts looking through his book, while muttering something about {1}{0}", Environment.NewLine, effect != null ? effect.Desc : "not having enough snappy comments.");

					Globals.Character = Globals.Database.CharacterTable.Records.Where(c => string.Equals(c.Name, character.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

					if (Globals.Character == null)
					{
						Globals.Out.Write("{0}He eventually looks at you and says, \"Yer name's na in here.  Have ye given it to me aright?\"{0}", Environment.NewLine);

						Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

						Globals.Out.Write("{0}Press Y for yes or N for no: ", Environment.NewLine);

						Buf.Clear();

						rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						Globals.Thread.Sleep(150);

						if (Buf.Length > 0 && Buf[0] == 'Y')
						{
							character.Name = Globals.Engine.Capitalize(character.Name);

							Globals.CharacterName = character.Name;

							CreateCharacter(character, args);

							menu.Execute();

							goto Cleanup;
						}
						else
						{
							BadCharacterName();
						}
					}
					else
					{
						/* 
							Full Credit:  Derived wholly from Donald Brown's Classic Eamon

							File: MAIN HALL
							Line: 2020
						*/

						if (Rtio == null)
						{
							var c2 = Globals.Character.GetMerchantAdjustedCharisma();

							Rtio = Globals.Engine.GetMerchantRtio(c2);
						}

						Globals.CharacterName = Globals.Character.Name;

						if (Globals.Character.Status == Enums.Status.Alive)
						{
							Globals.Out.Write("{0}Finally he looks up and says, \"Ah, here ye be.  Well, go and have fun in the hall.\"{0}", Environment.NewLine);

							Globals.In.KeyPress(Buf);

							menu.Execute();

							goto Cleanup;
						}
						else if (Globals.Character.Status == Enums.Status.Adventuring)
						{
							Globals.Out.Write("{0}The burly Irishman stares at you intently and says, \"If ye really be {1}, it means ye've been recalled from yer adventure with the help of a local wizard, and fer a fee.  Is this true?\"{0}{0}(Warning:  Any saved games for that adventure will be deleted!){0}",
								Environment.NewLine,
								Globals.Character.Name);

							Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

							Globals.Out.Write("{0}Press Y for yes or N for no: ", Environment.NewLine);

							Buf.Clear();

							rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

							Debug.Assert(Globals.Engine.IsSuccess(rc));

							Globals.Thread.Sleep(150);

							if (Buf.Length > 0 && Buf[0] == 'Y')
							{
								Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

								AdventureName = "";

								RecallCharacterFromAdventure();

								if (Globals.Character.Status != Enums.Status.Adventuring)
								{
									var ap = Globals.Engine.GetMerchantAskPrice(Constants.RecallPrice, (double)Rtio);

									Globals.Out.Write("{0}Pointing to an entry in his registry, the Irishman exclaims, \"Says 'ere the wizard found ye in {1} and charged {2} gold coins for his services{3}!\"{0}",
										Environment.NewLine,
										AdventureName.Length > 0 ? AdventureName : Globals.Engine.UnknownName,
										ap,
										Globals.Character.HeldGold + Globals.Character.BankGold < ap ? ", but ye didn't have enough to pay the fee, even after he put a lien on yer bank account!  Uh-oh, he said he'd get even soon" :
										Globals.Character.HeldGold < ap ? "!  He even put a lien on yer bank account to ensure full payment" :
										"");

									Globals.Character.HeldGold -= ap;

									if (Globals.Character.HeldGold < 0)
									{
										Globals.Character.BankGold += Globals.Character.HeldGold;

										Globals.Character.HeldGold = 0;

										if (Globals.Character.BankGold < 0)
										{
											Globals.Character.HeldGold = Globals.Character.BankGold;

											Globals.Character.BankGold = 0;
										}
									}

									if (Globals.Character.HeldGold < Constants.MinGoldValue)
									{
										Globals.Character.HeldGold = Constants.MinGoldValue;
									}
									else if (Globals.Character.HeldGold > Constants.MaxGoldValue)
									{
										Globals.Character.HeldGold = Constants.MaxGoldValue;
									}

									Globals.CharactersModified = true;

									Globals.Out.Write("{0}Finally he looks up and says, \"Welcome back from yer adventure.  Now go and have fun in the hall.\"{0}", Environment.NewLine);

									Globals.In.KeyPress(Buf);

									menu.Execute();

									goto Cleanup;
								}
								else
								{
									Globals.Out.Write("{0}Pointing to an entry in his registry, the Irishman exclaims, \"Says 'ere the wizard couldn't locate {1} in any known adventure!\"{0}{0}(You will have to manually change {2} status using EamonDD.){0}",
										Environment.NewLine,
										Globals.Character.Name,
										Globals.Character.EvalGender("his", "her", "its"));
								}
							}
						}
						else
						{
							Debug.Assert(Globals.Character.Status == Enums.Status.Dead);

							Globals.Out.Write("{0}The burly Irishman gets a sad look in his eyes and says, \"Ye can't be {1}, {2} be dead.  Now who'r ye again?\"{0}",
								Environment.NewLine,
								Globals.Character.Name,
								Globals.Character.EvalGender("he", "she", "it"));
						}
					}
				}
				else
				{
					BadCharacterName();
				}

				if (NumChances == 0)
				{
					Globals.In.KeyPress(Buf);

					goto Cleanup;
				}
			}

		Cleanup:

			;
		}

		public override void Execute()
		{
			SelectCharacter();
		}

		public RegistrationDeskMenu()
		{
			Buf = Globals.Buf;

			AdventureName = "";

			NumChances = 13;
		}
	}
}
