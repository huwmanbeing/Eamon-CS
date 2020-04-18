
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeBeforeCommandPromptPrint)
			{
				var characterMonster = gMDB[gGameState.Cm];

				Debug.Assert(characterMonster != null);

				var room = characterMonster.GetInRoom();

				Debug.Assert(room != null);

				// Describe secret doors

				if (room.IsLit())
				{
					if (room.Uid == 54 && (gGameState.GetSecretDoors(1) || gGameState.GetSecretDoors(2)))
					{
						gOut.Print(gGameState.GetSecretDoors(1) && gGameState.GetSecretDoors(2) ? "You've found secret doors leading north and south." : gGameState.GetSecretDoors(1) ? "You've found a secret door leading north." : "You've found a secret door leading south.");
					}
					else if (room.Uid == 55 && gGameState.GetSecretDoors(1))
					{
						gOut.Print("You've found a door leading south.");
					}
					else if (room.Uid == 56 && (gGameState.GetSecretDoors(2) || gGameState.GetSecretDoors(4)))
					{
						gOut.Print(gGameState.GetSecretDoors(2) && gGameState.GetSecretDoors(4) ? "You've found secret doors leading north and east." : gGameState.GetSecretDoors(2) ? "You've found a secret door leading north." : "You've found a secret door leading east.");
					}
					else if (room.Uid == 58 && gGameState.GetSecretDoors(3))
					{
						gOut.Print("You've found a secret door leading east.");
					}
					else if (room.Uid == 63 && gGameState.GetSecretDoors(3))
					{
						gOut.Print("You've found a secret door leading west.");
					}
					else if (room.Uid == 68 && gGameState.GetSecretDoors(4))
					{
						gOut.Print("You've found a secret door leading west.");
					}
					else if (room.Uid == 74 && (gGameState.GetSecretDoors(5) || gGameState.GetSecretDoors(6)))
					{
						gOut.Print(gGameState.GetSecretDoors(5) && gGameState.GetSecretDoors(6) ? "You've discovered secret doors leading north and south." : gGameState.GetSecretDoors(5) ? "You've discovered a secret door leading north." : "You've discovered a secret door leading south.");
					}
					else if (room.Uid == 87 && gGameState.GetSecretDoors(7))
					{
						gOut.Print("You've discovered a trapdoor leading down into darkness.");
					}
					else if (room.Uid == 100 && gGameState.GetSecretDoors(9))
					{
						gOut.Print("You've discovered a secret door leading north.");
					}
					else if (room.Uid == 101 && gGameState.GetSecretDoors(8))
					{
						gOut.Print("You've discovered a secret door leading west.");
					}
					else if (room.Uid == 102 && gGameState.GetSecretDoors(11))
					{
						gOut.Print("You've found a secret door leading east.");
					}
					else if (room.Uid == 115 && gGameState.GetSecretDoors(10))
					{
						gOut.Print("You've found a secret door leading north.");
					}
					else if (room.Uid == 116 && gGameState.GetSecretDoors(12))
					{
						gOut.Print("You've found a secret panel in the fountain bottom - you may go down.");
					}
				}

				if (ShouldPreTurnProcess())
				{
					var efreetiMonster = gMDB[50];

					Debug.Assert(efreetiMonster != null);

					// Efreeti goes poof

					if (!efreetiMonster.IsInLimbo() && (!efreetiMonster.IsInRoom(room) || !efreetiMonster.CheckNBTLHostility()) && gEngine.RollDice(1, 100, 0) <= 50)
					{
						if (efreetiMonster.IsInRoom(room) && room.IsLit())
						{
							gOut.Print("{0}{1} vanishes into thin air.", efreetiMonster.GetTheName(true), efreetiMonster.Friendliness == Friendliness.Friend ? ", seeing that you aren't in any immediate danger," : "");
						}

						efreetiMonster.SetInLimbo();
					}

					var cloakAndCowlArtifact = gADB[44];

					Debug.Assert(cloakAndCowlArtifact != null);

					// Dark hood and small glade

					if (cloakAndCowlArtifact.IsInLimbo())
					{
						var darkHoodMonster = gMDB[21];

						Debug.Assert(darkHoodMonster != null);

						if (!darkHoodMonster.IsInLimbo())
						{
							var darkHoodInPlayerRoom = darkHoodMonster.IsInRoom(room);

							var darkHoodVanishes = false;

							if (gGameState.IsDayTime())
							{
								darkHoodMonster.SetInLimbo();

								darkHoodVanishes = true;
							}
							else if (room.Uid != 23 && !darkHoodMonster.IsInRoomUid(23))
							{
								darkHoodMonster.SetInRoomUid(23);

								darkHoodVanishes = true;
							}

							if (darkHoodInPlayerRoom && darkHoodVanishes)
							{
								gOut.Print("{0} suddenly vanishes, seemingly into thin air.", darkHoodMonster.GetTheName(true));
							}
						}
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
