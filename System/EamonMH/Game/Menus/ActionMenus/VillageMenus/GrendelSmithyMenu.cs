﻿
// GrendelSmithyMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using Eamon.Game.Utilities;
using EamonMH.Framework.Menus.ActionMenus;
using Enums = Eamon.Framework.Primitive.Enums;
using Classes = Eamon.Framework.Primitive.Classes;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class GrendelSmithyMenu : Menu, IGrendelSmithyMenu
	{
		protected virtual double? Rtio { get; set; }

		protected virtual long GetWeaponType()
		{
			RetCode rc;
			long i;

			Buf.Clear();

			var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

			for (i = 0; i < weaponValues.Count; i++)
			{
				var weapon = Globals.Engine.GetWeapons(weaponValues[(int)i]);

				Debug.Assert(weapon != null);

				Buf.AppendFormat("{0}{1}{2}={3}{4}",
					i == 0 ? Environment.NewLine : "",
					i != 0 ? ", " : "",
					(long)weaponValues[(int)i],
					weapon.MarcosName ?? weapon.Name,
					i == weaponValues.Count - 1 ? ": " : "");
			}

			Globals.Out.Write("{0}", Buf);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, null, Globals.MhEngine.IsCharWpnType, Globals.MhEngine.IsCharWpnType);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Debug.Assert(Buf.Length > 0);

			return Convert.ToInt64(Buf.Trim().ToString());
		}

		protected virtual void UpdateCharacterWeapon(long i, long ap, string name, long type, long complexity, long dice, long sides)
		{
			var cw = Globals.CreateInstance<Classes.ICharacterWeapon>(x =>
			{
				x.Name = name;
				x.IsPlural = false;
				x.PluralType = Enums.PluralType.None;
				x.ArticleType = Enums.ArticleType.None;
				x.Type = (Enums.Weapon)type;
				x.Complexity = complexity;
				x.Dice = dice;
				x.Sides = sides;
			});

			Globals.Character.SetWeapons(i, cw);

			Globals.Character.GetWeapons(i).Parent = Globals.Character;

			Globals.Character.StripPoundCharsFromWeaponNames();

			Globals.Character.AddPoundCharsToWeaponNames();

			Globals.Character.HeldGold -= ap;

			Globals.CharactersModified = true;
		}

		protected virtual void NotEnoughGold()
		{
			Globals.Out.Write("{0}\"Sorry, but you don't seem to have enough gold to pay for your weapon at this time.  Come back when you have enough.\"{0}", Environment.NewLine);
		}

		public override void Execute()
		{
			RetCode rc;
			long ap = 0;
			long ap0;
			long ap1;
			long i;

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			/* 
				Full Credit:  Derived wholly from Donald Brown's Classic Eamon

				File: MAIN HALL
				Line: 3010
			*/

			if (Rtio == null)
			{
				var c2 = Globals.Character.GetMerchantAdjustedCharisma();

				Rtio = Globals.Engine.GetMerchantRtio(c2);
			}

			i = Globals.Engine.FindIndex(Globals.Character.Weapons, w => !w.IsActive());

			if (i < 0)
			{
				Globals.Out.Write("{0}Grendel says, \"I'm sorry, but you're going to have to try and sell one of your weapons at the store to the north.  You know the law:  No more than four weapons per person!  Come back when you've sold a weapon.\"{0}", Environment.NewLine);

				goto Cleanup;
			}

			Globals.Out.Write("{0}Grendel says, \"Would you care to look at my stock of used weapons?  You can also order a custom weapon if you'd prefer.\"{0}", Environment.NewLine);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Globals.Out.Write("{0}U=Used weapon, C=Custom weapon, X=Exit: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.MhEngine.IsCharUOrCOrX, Globals.MhEngine.IsCharUOrCOrX);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			if (Buf.Length == 0 || Buf[0] == 'X')
			{
				goto Cleanup;
			}

			if (Buf[0] == 'U')
			{
				var weaponList = new List<string[]>()
				{
					null,
					new string[] { "Slaymor", "Elfkill" },
					new string[] { "Stinger", "Scrunch" },
					new string[] { "Centuri", "Falcoor" },
					new string[] { "Widower", "Flasher" },
					new string[] { "Slasher", "Freedom" }
				};

				Globals.Out.Write("{0}\"What type of weapon do you wish?\"{0}", Environment.NewLine);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				var j = (int)GetWeaponType();

				var weaponPrice = 500;

				ap0 = Globals.Engine.GetMerchantAskPrice(weaponPrice, (double)Rtio);

				weaponPrice = 1200;

				ap1 = Globals.Engine.GetMerchantAskPrice(weaponPrice, (double)Rtio);

				Globals.Out.Write("{0}\"I happen to have two in stock right now.\"{0}{0}1. {1} (2D8  / 12%) ...... {2} GP{0}2. {3} (2D16 / 24%) ..... {4} GP{0}", Environment.NewLine, weaponList[j][0], ap0, weaponList[j][1], ap1);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}Press the number of the weapon to buy or X to exit: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.MhEngine.IsChar1Or2OrX, Globals.MhEngine.IsChar1Or2OrX);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				if (Buf.Length == 0 || Buf[0] == 'X')
				{
					goto Cleanup;
				}

				var k = (int)Convert.ToInt64(Buf.Trim().ToString());

				ap = k == 1 ? ap0 : ap1;

				if (Globals.Character.HeldGold >= ap)
				{
					Globals.Out.Write("{0}\"Good choice!  A great bargain!\"{0}", Environment.NewLine);

					UpdateCharacterWeapon(i, ap, Globals.CloneInstance(weaponList[j][k - 1]), j, 12 * k, 2, 8 * k);

					goto Cleanup;
				}
				else
				{
					NotEnoughGold();

					goto Cleanup;
				}
			}
			else
			{
				Globals.Out.Write("{0}\"What do you want me to make?\"{0}", Environment.NewLine);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				var j = (int)GetWeaponType();

				var weapon = Globals.Engine.GetWeapons((Enums.Weapon)j);

				Debug.Assert(weapon != null);

				var wpnName = (weapon.MarcosName ?? weapon.Name).ToLower();

				Globals.Out.Write("{0}\"What name should I inscribe on it?\"{0}", Environment.NewLine);

				Globals.Out.Write("{0}Note: this should be a capitalized singular proper name (eg, Trollsfire){0}", Environment.NewLine);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}Enter weapon name: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.CharWpnNameLen, null, ' ', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var wpnName01 = Buf.Trim().ToString();

				Globals.Thread.Sleep(150);

				if (wpnName01 == "" || string.Equals(wpnName01, "NONE", StringComparison.OrdinalIgnoreCase))
				{
					wpnName01 = string.Format("Grendel{0}", wpnName);
				}

				wpnName01 = Globals.Engine.Capitalize(wpnName01.ToLower());

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}\"I do have limits of craftsmanship.\"{0}{0}    Complexity    Dice   Sides{0}      1%-50%       1-3    1-12{0}", Environment.NewLine);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}Enter complexity: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, ' ', '\0', false, null, null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var wpnComplexity = Convert.ToInt64(Buf.Trim().ToString());

				if (wpnComplexity < 1)
				{
					wpnComplexity = 1;
				}
				else if (wpnComplexity > 50)
				{
					wpnComplexity = 50;
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}Enter number of dice: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, ' ', '\0', false, null, null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var wpnDice = Convert.ToInt64(Buf.Trim().ToString());

				if (wpnDice < 1)
				{
					wpnDice = 1;
				}
				else if (wpnDice > 3)
				{
					wpnDice = 3;
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}Enter number of dice sides: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, ' ', '\0', false, null, null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var wpnSides = Convert.ToInt64(Buf.Trim().ToString());

				if (wpnSides < 1)
				{
					wpnSides = 1;
				}
				else if (wpnSides > 12)
				{
					wpnSides = 12;
				}

				var weaponPrice = wpnComplexity * wpnDice * wpnSides + 500;

				ap = Globals.Engine.GetMerchantAskPrice(weaponPrice, (double)Rtio);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}\"I can make you a {1}D{2} {3} with complexity of {4}% called {5} for {6} gold pieces.  Should I proceed?\"{0}", Environment.NewLine, wpnDice, wpnSides, wpnName, wpnComplexity, wpnName01, ap);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}Press Y for yes or N for no: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				if (Buf.Length == 0 || Buf[0] == 'N')
				{
					goto Cleanup;
				}

				if (Globals.Character.HeldGold >= ap)
				{
					Globals.Out.Write("{0}Grendel works on your weapon, often calling in wizards and weapon experts.  Finally he finishes.  \"I think you will be satisfied with this.\" he says modestly.{0}", Environment.NewLine);

					UpdateCharacterWeapon(i, ap, wpnName01, j, wpnComplexity, wpnDice, wpnSides);

					goto Cleanup;
				}
				else
				{
					NotEnoughGold();

					goto Cleanup;
				}
			}

		Cleanup:

			Globals.Out.Write("{0}\"Goodbye, {1}!  Come again.\"{0}", Environment.NewLine, Globals.Character.Name);

			Globals.In.KeyPress(Buf);
		}

		public GrendelSmithyMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
