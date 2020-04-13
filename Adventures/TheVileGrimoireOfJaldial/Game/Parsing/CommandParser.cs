
// CommandParser.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Parsing
{
	[ClassMappings(typeof(ICommandParser))]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, Framework.Parsing.ICommandParser
	{
		public virtual long DecorationId { get; set; }

		public override void Clear()
		{
			base.Clear();

			DecorationId = 0;
		}

		public override void ParseName()
		{
			base.ParseName();

			// Make note of the first Room decoration the player is referring to (if any)

			if (DecorationId == 0)
			{
				if ((gActorRoom.Uid == 1 || gActorRoom.Uid == 4) && ObjData.Name.Contains("gate", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 1;
				}
				else if (gActorRoom.IsFenceRoom() && ObjData.Name.Contains("fence", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 2;
				}
				else if (gActorRoom.IsGroundsRoom() && ObjData.Name.ContainsAny(new string[] { "foliage", "trees", "forest", "weeds", "plants", "grass", "lichen", "moss" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 3;
				}
				else if (gActorRoom.IsGroundsRoom() && gActorRoom.Uid != 23 && ObjData.Name.ContainsAny(new string[] { "tombstone", "gravestone" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 4;
				}
				else if ((gActorRoom.Uid == 11 || gActorRoom.Uid == 16 || gActorRoom.Uid == 22) && ObjData.Name.ContainsAny(new string[] { "brook", "stream" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 5;
				}
				else if (gActorRoom.Uid == 12 && ObjData.Name.ContainsAny(new string[] { "pile", "offal" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 6;
				}
				else if (gActorRoom.Uid == 12 && ObjData.Name.Contains("rat", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 7;
				}
				else if (gActorRoom.Uid == 13 && ObjData.Name.ContainsAny(new string[] { "pile", "rocks", "pyramid" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 8;
				}
				else if (gActorRoom.Uid == 8 && ObjData.Name.Contains("elm", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 9;
				}
				else if (gActorRoom.Uid == 19 && ObjData.Name.ContainsAny(new string[] { "hole", "grave" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 10;
				}
				else if (gActorRoom.Uid == 20 && ObjData.Name.ContainsAny(new string[] { "skeleton", "animal", "creature" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 11;
				}
				else if (gActorRoom.Uid == 23 && ObjData.Name.Contains("pine", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 12;
				}
				else if (gActorRoom.Uid == 26 && ObjData.Name.ContainsAny(new string[] { "coffin", "hand", "skeleton" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 13;
				}
				else if (!gActorRoom.IsGroundsRoom() && gActorRoom.Type == RoomType.Indoors && ObjData.Name.ContainsAny(new string[] { "floor", "dust" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 14;
				}
				else if (gActorRoom.Uid == 56 && ObjData.Name.ContainsAny(new string[] { "heap", "pile", "bone" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 15;
				}
				else if (gActorRoom.IsBodyChamberRoom() && ObjData.Name.ContainsAny(new string[] { "body", "bodies", "internment", "opening", "chamber" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 16;
				}
				else if (gActorRoom.Uid == 62 && ObjData.Name.ContainsAny(new string[] { "fresco", "mural", "painting" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 17;
				}
				else if (gActorRoom.Uid == 62 && ObjData.Name.ContainsAny(new string[] { "rune", "writing", "inscription" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 18;
				}
				else if ((gActorRoom.Uid == 64 || gActorRoom.Uid == 65) && ObjData.Name.Contains("door", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 19;
				}
				else if (gActorRoom.Uid == 65 && ObjData.Name.ContainsAny(new string[] { "fluid", "blood" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 20;
				}
				else if ((gActorRoom.Uid == 64 || gActorRoom.Uid == 65) && ObjData.Name.ContainsAny(new string[] { "rune", "writing", "inscription" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 21;
				}
				else if (gActorRoom.Uid == 66 && ObjData.Name.ContainsAny(new string[] { "skeleton", "leather", "armor" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 22;
				}
				else if (gActorRoom.Uid == 66 && ObjData.Name.Contains("wall", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 23;
				}
				else if (gActorRoom.Uid == 68 && ObjData.Name.Contains("moss", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 24;
				}
				else if (gActorRoom.Uid == 69 && ObjData.Name.Contains("moss", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 25;
				}
				else if (gActorRoom.Uid == 69 && ObjData.Name.Contains("box", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 26;
				}
				else if (gActorRoom.Uid == 71 && ObjData.Name.Contains("alga", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 27;
				}
				else if (gActorRoom.Uid == 70 && ObjData.Name.Contains("groove", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 28;
				}
				else if (gActorRoom.Uid == 71 && ObjData.Name.Contains("bow", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 29;
				}
				else if (gActorRoom.Uid == 71 && ObjData.Name.Contains("hole", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 30;
				}
				else if (gActorRoom.Uid == 72 && ObjData.Name.ContainsAny(new string[] { "cloth", "strip" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 31;
				}
				else if (gActorRoom.Uid == 72 && ObjData.Name.ContainsAny(new string[] { "rock", "pile" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 32;
				}
				else if (gActorRoom.Uid == 73 && ObjData.Name.Contains("mummy", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 33;
				}
				else if (gActorRoom.Uid == 74 && ObjData.Name.ContainsAny(new string[] { "spider", "web" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 34;
				}
				else if (gActorRoom.Uid == 75 && ObjData.Name.ContainsAny(new string[] { "fresco", "mural", "painting" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 35;
				}
				else if (gActorRoom.Uid == 75 && ObjData.Name.ContainsAny(new string[] { "glyph", "writing", "inscription" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 36;
				}
				else if (gActorRoom.Uid == 76 && ObjData.Name.Contains("etching", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 37;
				}
				else if (gActorRoom.Uid == 77 && ObjData.Name.Contains("face", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 38;
				}
				else if (gActorRoom.Uid == 82 && ObjData.Name.ContainsAny(new string[] { "goblin", "body" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 39;
				}
				else if (gActorRoom.Uid == 82 && ObjData.Name.ContainsAny(new string[] { "chain", "armor" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 40;
				}
				else if (gActorRoom.Uid == 84 && ObjData.Name.ContainsAny(new string[] { "shiny", "substance", "slime" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 41;
				}
				else if (gActorRoom.Uid == 84 && ObjData.Name.ContainsAny(new string[] { "boot", "mound", "earth" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 42;
				}
				else if (gActorRoom.Uid == 86 && ObjData.Name.ContainsAny(new string[] { "goblin", "bodies", "body" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 43;
				}
				else if (gActorRoom.Uid == 86 && ObjData.Name.ContainsAny(new string[] { "spoor", "dung" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 44;
				}
				else if (gActorRoom.Uid == 87 && ObjData.Name.ContainsAny(new string[] { "fog", "mist" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 45;
				}
				else if (gActorRoom.Uid == 88 && ObjData.Name.ContainsAny(new string[] { "pick", "marks" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 46;
				}
				else if (gActorRoom.Uid == 89 && ObjData.Name.ContainsAny(new string[] { "tapestries", "tapestry" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 47;
				}
				else if ((gActorRoom.Uid == 90 || gActorRoom.Uid == 93) && ObjData.Name.ContainsAny(new string[] { "mining", "tool" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 48;
				}
				else if (gActorRoom.Uid == 95 && ObjData.Name.ContainsAny(new string[] { "skeletal", "arm" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 49;
				}
				else if (gActorRoom.Uid == 96 && ObjData.Name.ContainsAny(new string[] { "pit", "hole" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 50;
				}
				else if (gActorRoom.Uid == 101 && ObjData.Name.Contains("skeleton", StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 51;
				}
				else if (gActorRoom.Uid == 102 && ObjData.Name.ContainsAny(new string[] { "stain", "blood" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 52;
				}
				else if (gActorRoom.Uid == 103 && ObjData.Name.ContainsAny(new string[] { "etching", "carving" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 53;
				}
				else if (gActorRoom.Uid == 104 && ObjData.Name.ContainsAny(new string[] { "face", "mouth", "hole" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 54;
				}
				else if (gActorRoom.Uid == 105 && ObjData.Name.ContainsAny(new string[] { "pile", "bodies", "body" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 55;
				}
				else if (gActorRoom.Uid == 108 && ObjData.Name.ContainsAny(new string[] { "beach", "sea", "ocean" }, StringComparison.OrdinalIgnoreCase))
				{
					DecorationId = 56;
				}
			}
		}

		public override void CheckPlayerCommand(ICommand command, bool afterFinishParsing)
		{
			Debug.Assert(command != null);

			if (afterFinishParsing)
			{
				var waterWeirdMonster = gMDB[38];

				Debug.Assert(waterWeirdMonster != null);

				// Large fountain and water weird

				if (gDobjArtifact != null && gDobjArtifact.Uid != 24 && gIobjArtifact != null && gIobjArtifact.Uid == 24)
				{
					if (waterWeirdMonster.IsInRoom(gActorRoom))
					{
						gOut.Print("{0} won't let you get close enough to do that!", waterWeirdMonster.GetTheName(true));

						NextState = Globals.CreateInstance<IMonsterStartState>();
					}
					else if (!gGameState.WaterWeirdKilled)
					{
						gEngine.PrintEffectDesc(100);

						waterWeirdMonster.SetInRoom(gActorRoom);

						NextState = Globals.CreateInstance<IStartState>();
					}
					else
					{
						base.CheckPlayerCommand(command, afterFinishParsing);
					}
				}
				else
				{
					base.CheckPlayerCommand(command, afterFinishParsing);
				}
			}
			else
			{
				base.CheckPlayerCommand(command, afterFinishParsing);
			}
		}
	}
}
