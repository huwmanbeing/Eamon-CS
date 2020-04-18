﻿
// SearchCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class SearchCommand : EamonRT.Game.Commands.Command, Framework.Commands.ISearchCommand
	{
		public override void PlayerExecute()
		{
			gOut.Print("Okay, you're doing a thorough search now...");

			gGameState.Minute += 20;      // TODO: move time passing logic to centralized location for all Commands

			var notFoundDesc = "You find nothing new.";

			var foundDesc = "You find something!";

			var found = false;

			var fountainSecretDoorFound = false;

			var saved = gEngine.SaveThrow(Stat.Intellect);

			var saved02 = gEngine.SaveThrow(Stat.Intellect);

			var waterArtifact = gADB[40];

			Debug.Assert(waterArtifact != null);

			if (gDobjArtifact != null)
			{
				var crystalGobletArtifact = gADB[12];

				Debug.Assert(crystalGobletArtifact != null);

				var crimsonCloakArtifact = gADB[19];

				Debug.Assert(crimsonCloakArtifact != null);

				var goldPiecesArtifact = gADB[20];

				Debug.Assert(goldPiecesArtifact != null);

				var pouchContainingStonesArtifact = gADB[21];

				Debug.Assert(pouchContainingStonesArtifact != null);

				var griffinEggArtifact = gADB[22];

				Debug.Assert(griffinEggArtifact != null);

				var bookOfRunesArtifact = gADB[27];

				Debug.Assert(bookOfRunesArtifact != null);

				// Dragon's treasure hoard

				if (gDobjArtifact.Uid == 11)
				{
					if (saved && crystalGobletArtifact.IsInLimbo())
					{
						foundDesc = "You find an interesting item!";

						crystalGobletArtifact.SetInRoom(gActorRoom);

						found = true;
					}
				}

				// Beholder's treasure hoard

				else if (gDobjArtifact.Uid == 18)
				{
					if (saved && crimsonCloakArtifact.IsInLimbo())
					{
						foundDesc = "After tearing the hoard apart, you find some interesting items!";

						crimsonCloakArtifact.SetInRoom(gActorRoom);

						goldPiecesArtifact.SetInRoom(gActorRoom);

						pouchContainingStonesArtifact.SetInRoom(gActorRoom);

						found = true;
					}
				}

				// Large nest

				else if (gDobjArtifact.Uid == 23)
				{
					if (saved && griffinEggArtifact.IsInLimbo())
					{
						foundDesc = "You find a secret compartment, in which something is hidden!";

						griffinEggArtifact.SetCarriedByContainer(gDobjArtifact);

						found = true;
					}
				}

				// Large fountain

				else if (gDobjArtifact.Uid == 24)
				{
					if (waterArtifact.IsInLimbo())
					{
						if (saved && !gGameState.GetSecretDoors(12))
						{
							foundDesc = "You find a secret door at the bottom of the fountain!";

							gGameState.SetSecretDoors(12, true);

							found = true;

							fountainSecretDoorFound = true;
						}
					}
					else
					{
						notFoundDesc = "There is a large pool of water in this fountain.  You can't see the murky bottom.";
					}
				}

				// Wooden throne

				else if (gDobjArtifact.Uid == 26)
				{
					if (saved && bookOfRunesArtifact.IsInLimbo())
					{
						foundDesc = "You find a secret compartment in the tree stump!";

						bookOfRunesArtifact.SetInRoom(gActorRoom);

						found = true;
					}
				}
			}
			else
			{
				var tapestryArtifact = gADB[2];

				Debug.Assert(tapestryArtifact != null);

				var lanternArtifact = gADB[39];

				Debug.Assert(lanternArtifact != null);

				if (gActorRoom.Uid == 36)
				{
					if (saved && lanternArtifact.IsInLimbo())
					{
						foundDesc = "After scouring the area, you find something of interest!";

						lanternArtifact.SetInRoom(gActorRoom);

						found = true;
					}
				}
				else if (gActorRoom.Uid == 54)
				{
					if (saved && !gGameState.GetSecretDoors(1))
					{
						gGameState.SetSecretDoors(1, true);

						found = true;
					}
					else if (saved02 && !gGameState.GetSecretDoors(2))
					{
						gGameState.SetSecretDoors(2, true);

						found = true;
					}
				}
				else if (gActorRoom.Uid == 55)
				{
					if (saved && !gGameState.GetSecretDoors(1))
					{
						gGameState.SetSecretDoors(1, true);

						found = true;
					}
				}
				else if (gActorRoom.Uid == 56)
				{
					if (saved && !gGameState.GetSecretDoors(2))
					{
						gGameState.SetSecretDoors(2, true);

						found = true;
					}
					else if (saved02 && !gGameState.GetSecretDoors(4))
					{
						gGameState.SetSecretDoors(4, true);

						found = true;
					}
				}
				else if (gActorRoom.Uid == 58)
				{
					if (saved && !gGameState.GetSecretDoors(3))
					{
						gGameState.SetSecretDoors(3, true);

						found = true;
					}
				}
				else if (gActorRoom.Uid == 63)
				{
					if (saved && !gGameState.GetSecretDoors(3))
					{
						gGameState.SetSecretDoors(3, true);

						found = true;
					}
				}
				else if (gActorRoom.Uid == 68)
				{
					if (saved && !gGameState.GetSecretDoors(4))
					{
						gGameState.SetSecretDoors(4, true);

						found = true;
					}
				}
				else if (gActorRoom.Uid == 74)
				{
					if (saved && !gGameState.GetSecretDoors(5))
					{
						gGameState.SetSecretDoors(5, true);

						found = true;
					}
					else if (saved02 && !gGameState.GetSecretDoors(6))
					{
						gGameState.SetSecretDoors(6, true);

						found = true;
					}
				}
				else if (gActorRoom.Uid == 87)
				{
					if (saved && !gGameState.GetSecretDoors(7))
					{
						gGameState.SetSecretDoors(7, true);

						found = true;
					}
				}
				else if (gActorRoom.Uid == 89)
				{
					if (saved && tapestryArtifact.IsInLimbo())
					{
						foundDesc = "You find an interesting tapestry!";

						tapestryArtifact.SetInRoom(gActorRoom);

						found = true;
					}
				}
				else if (gActorRoom.Uid == 100)
				{
					if (saved && !gGameState.GetSecretDoors(9))
					{
						gGameState.SetSecretDoors(9, true);

						found = true;
					}
				}
				else if (gActorRoom.Uid == 101)
				{
					if (saved && !gGameState.GetSecretDoors(8))
					{
						gGameState.SetSecretDoors(8, true);

						found = true;
					}
				}
				else if (gActorRoom.Uid == 102)
				{
					if (saved && !gGameState.GetSecretDoors(11))
					{
						gGameState.SetSecretDoors(11, true);

						found = true;
					}
				}
				else if (gActorRoom.Uid == 115)
				{
					if (saved && !gGameState.GetSecretDoors(10))
					{
						gGameState.SetSecretDoors(10, true);

						found = true;
					}
				}
				else if (gActorRoom.Uid == 116)
				{
					if (waterArtifact.IsInLimbo())
					{
						if (saved && !gGameState.GetSecretDoors(12))
						{
							gGameState.SetSecretDoors(12, true);

							found = true;

							fountainSecretDoorFound = true;
						}
					}
				}
			}

			if (found)
			{
				gOut.Print(foundDesc);

				NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				gOut.Print(notFoundDesc);
			}

			// Large fountain becomes a DoorGate

			if (fountainSecretDoorFound)
			{
				var fountainArtifact = gADB[24];

				Debug.Assert(fountainArtifact != null);

				// Fountain cannot be both an InContainer and a DoorGate, an illegal combination - must empty contents

				var artifactList = fountainArtifact.GetContainedList();

				foreach (var a in artifactList)
				{
					a.SetInRoom(gActorRoom);
				}

				fountainArtifact.Type = ArtifactType.DoorGate;

				fountainArtifact.Field1 = 117;

				fountainArtifact.Field2 = -1;

				fountainArtifact.Field3 = 0;

				fountainArtifact.Field4 = 0;

				fountainArtifact.Field5 = 0;

				fountainArtifact.Synonyms = fountainArtifact.Synonyms.Concat(new string[] { "secret door", "secret panel", "door", "panel" }).ToArray();
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			if (gCommandParser.CurrToken < gCommandParser.Tokens.Length)
			{
				gCommandParser.ParseName();

				if (!string.Equals(gCommandParser.ObjData.Name, "room", StringComparison.OrdinalIgnoreCase) && !string.Equals(gCommandParser.ObjData.Name, "area", StringComparison.OrdinalIgnoreCase) && !(gActorRoom.Uid == 89 && gCommandParser.ObjData.Name.ContainsAny(new string[] { "tapestries", "tapestry" }, StringComparison.OrdinalIgnoreCase)))
				{
					PlayerResolveArtifact();
				}
			}
		}

		public SearchCommand()
		{
			SortOrder = 440;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "SearchCommand";

			Verb = "search";

			Type = CommandType.Interactive;
		}
	}
}