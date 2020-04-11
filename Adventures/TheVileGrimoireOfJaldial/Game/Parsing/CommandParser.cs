
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
				if ((gActorRoom.Uid == 1 || gActorRoom.Uid == 4) && ObjData.Name.IndexOf("gate", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 1;
				}
				else if (gActorRoom.IsFenceRoom() && ObjData.Name.IndexOf("fence", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 2;
				}
				else if (gActorRoom.IsGroundsRoom() && (ObjData.Name.IndexOf("foliage", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("trees", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("forest", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("weeds", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("plants", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("grass", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("lichen", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("moss", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 3;
				}
				else if (gActorRoom.IsGroundsRoom() && gActorRoom.Uid != 23 && (ObjData.Name.IndexOf("tombstone", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("gravestone", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 4;
				}
				else if ((gActorRoom.Uid == 11 || gActorRoom.Uid == 16 || gActorRoom.Uid == 22) && (ObjData.Name.IndexOf("brook", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("stream", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 5;
				}
				else if (gActorRoom.Uid == 12 && (ObjData.Name.IndexOf("pile", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("offal", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 6;
				}
				else if (gActorRoom.Uid == 12 && ObjData.Name.IndexOf("rat", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 7;
				}
				else if (gActorRoom.Uid == 13 && (ObjData.Name.IndexOf("pile", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("rocks", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("pyramid", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 8;
				}
				else if (gActorRoom.Uid == 8 && ObjData.Name.IndexOf("elm", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 9;
				}
				else if (gActorRoom.Uid == 19 && (ObjData.Name.IndexOf("hole", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("grave", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 10;
				}
				else if (gActorRoom.Uid == 20 && (ObjData.Name.IndexOf("skeleton", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("animal", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("creature", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 11;
				}
				else if (gActorRoom.Uid == 23 && ObjData.Name.IndexOf("pine", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 12;
				}
				else if (gActorRoom.Uid == 26 && (ObjData.Name.IndexOf("coffin", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("hand", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("skeleton", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 13;
				}
				else if (!gActorRoom.IsGroundsRoom() && gActorRoom.Type == RoomType.Indoors && (ObjData.Name.IndexOf("floor", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("dust", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 14;
				}
				else if (gActorRoom.Uid == 56 && (ObjData.Name.IndexOf("heap", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("pile", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("bone", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 15;
				}
				else if (gActorRoom.IsBodyChamberRoom() && (ObjData.Name.IndexOf("body", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("bodies", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("internment", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("opening", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("chamber", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 16;
				}
				else if (gActorRoom.Uid == 62 && (ObjData.Name.IndexOf("fresco", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("mural", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("painting", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 17;
				}
				else if (gActorRoom.Uid == 62 && (ObjData.Name.IndexOf("rune", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("writing", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("inscription", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 18;
				}
				else if ((gActorRoom.Uid == 64 || gActorRoom.Uid == 65) && ObjData.Name.IndexOf("door", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 19;
				}
				else if (gActorRoom.Uid == 65 && (ObjData.Name.IndexOf("fluid", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("blood", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 20;
				}
				else if ((gActorRoom.Uid == 64 || gActorRoom.Uid == 65) && (ObjData.Name.IndexOf("rune", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("writing", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("inscription", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 21;
				}
				else if (gActorRoom.Uid == 66 && (ObjData.Name.IndexOf("skeleton", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("leather", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("armor", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 22;
				}
				else if (gActorRoom.Uid == 66 && ObjData.Name.IndexOf("wall", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 23;
				}
				else if (gActorRoom.Uid == 68 && ObjData.Name.IndexOf("moss", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 24;
				}
				else if (gActorRoom.Uid == 69 && ObjData.Name.IndexOf("moss", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 25;
				}
				else if (gActorRoom.Uid == 69 && ObjData.Name.IndexOf("box", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 26;
				}
				else if (gActorRoom.Uid == 71 && ObjData.Name.IndexOf("alga", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 27;
				}
				else if (gActorRoom.Uid == 70 && ObjData.Name.IndexOf("groove", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 28;
				}
				else if (gActorRoom.Uid == 71 && ObjData.Name.IndexOf("bow", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 29;
				}
				else if (gActorRoom.Uid == 71 && ObjData.Name.IndexOf("hole", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 30;
				}
				else if (gActorRoom.Uid == 72 && (ObjData.Name.IndexOf("cloth", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("strip", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 31;
				}
				else if (gActorRoom.Uid == 72 && (ObjData.Name.IndexOf("rock", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("pile", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 32;
				}
				else if (gActorRoom.Uid == 73 && ObjData.Name.IndexOf("mummy", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 33;
				}
				else if (gActorRoom.Uid == 74 && (ObjData.Name.IndexOf("spider", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("web", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 34;
				}
				else if (gActorRoom.Uid == 75 && (ObjData.Name.IndexOf("fresco", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("mural", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("painting", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 35;
				}
				else if (gActorRoom.Uid == 75 && (ObjData.Name.IndexOf("glyph", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("writing", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("inscription", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 36;
				}
				else if (gActorRoom.Uid == 76 && ObjData.Name.IndexOf("etching", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 37;
				}
				else if (gActorRoom.Uid == 77 && ObjData.Name.IndexOf("face", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 38;
				}
				else if (gActorRoom.Uid == 82 && (ObjData.Name.IndexOf("goblin", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("body", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 39;
				}
				else if (gActorRoom.Uid == 82 && (ObjData.Name.IndexOf("chain", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("armor", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 40;
				}
				else if (gActorRoom.Uid == 84 && (ObjData.Name.IndexOf("shiny", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("substance", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("slime", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 41;
				}
				else if (gActorRoom.Uid == 84 && (ObjData.Name.IndexOf("boot", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("mound", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("earth", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 42;
				}
				else if (gActorRoom.Uid == 86 && (ObjData.Name.IndexOf("goblin", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("bodies", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("body", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 43;
				}
				else if (gActorRoom.Uid == 86 && (ObjData.Name.IndexOf("spoor", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("dung", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 44;
				}
				else if (gActorRoom.Uid == 87 && (ObjData.Name.IndexOf("fog", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("mist", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 45;
				}
				else if (gActorRoom.Uid == 88 && (ObjData.Name.IndexOf("pick", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("marks", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 46;
				}
				else if (gActorRoom.Uid == 89 && (ObjData.Name.IndexOf("tapestries", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("tapestry", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 47;
				}
				else if ((gActorRoom.Uid == 90 || gActorRoom.Uid == 93) && (ObjData.Name.IndexOf("mining", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("tool", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 48;
				}
				else if (gActorRoom.Uid == 95 && (ObjData.Name.IndexOf("skeletal", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("arm", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 49;
				}
				else if (gActorRoom.Uid == 96 && (ObjData.Name.IndexOf("pit", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("hole", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 50;
				}
				else if (gActorRoom.Uid == 101 && ObjData.Name.IndexOf("skeleton", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					DecorationId = 51;
				}
				else if (gActorRoom.Uid == 102 && (ObjData.Name.IndexOf("stain", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("blood", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 52;
				}
				else if (gActorRoom.Uid == 103 && (ObjData.Name.IndexOf("etching", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("carving", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 53;
				}
				else if (gActorRoom.Uid == 104 && (ObjData.Name.IndexOf("face", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("mouth", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("hole", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 54;
				}
				else if (gActorRoom.Uid == 105 && (ObjData.Name.IndexOf("pile", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("bodies", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("body", StringComparison.OrdinalIgnoreCase) >= 0))
				{
					DecorationId = 55;
				}
				else if (gActorRoom.Uid == 108 && (ObjData.Name.IndexOf("beach", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("sea", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("ocean", StringComparison.OrdinalIgnoreCase) >= 0))
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
