
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, IExamineCommand
	{
		public override void PlayerExecute()
		{
			// Large fountain

			if (gDobjArtifact != null && gDobjArtifact.Uid == 24)
			{
				var waterArtifact = gADB[40];

				Debug.Assert(waterArtifact != null);

				base.PlayerExecute();

				if (gDobjArtifact.DoorGate == null && waterArtifact.IsInLimbo() && !Enum.IsDefined(typeof(ContainerType), ContainerType) && gEngine.SaveThrow(Stat.Intellect))
				{
					gOut.Print("You find a secret door at the bottom of the fountain!");

					// Fountain cannot be both an InContainer and a DoorGate, an illegal combination - must empty contents

					var artifactList = gDobjArtifact.GetContainedList();

					foreach (var a in artifactList)
					{
						a.SetInRoom(gActorRoom);
					}

					// Fountain becomes a DoorGate

					gDobjArtifact.Type = ArtifactType.DoorGate;

					gDobjArtifact.Field1 = 117;

					gDobjArtifact.Field2 = -1;

					gDobjArtifact.Field3 = 0;

					gDobjArtifact.Field4 = 0;

					gDobjArtifact.Field5 = 0;

					gDobjArtifact.Desc += "  **101";

					gDobjArtifact.Synonyms = gDobjArtifact.Synonyms.Concat(new string[] { "secret door", "secret panel", "door", "panel" }).ToArray();
				}
			}
			else
			{
				base.PlayerExecute();
			}
		}

		public override void PlayerFinishParsing()
		{
			gCommandParser.ParseName();

			ContainerType = Prep != null ? Prep.ContainerType : (ContainerType)(-1);

			if (string.Equals(gCommandParser.ObjData.Name, "room", StringComparison.OrdinalIgnoreCase) || string.Equals(gCommandParser.ObjData.Name, "area", StringComparison.OrdinalIgnoreCase))
			{
				var command = Globals.CreateInstance<ILookCommand>();

				CopyCommandData(command);

				gCommandParser.NextState = command;
			}
			else if (gCommandParser.DecorationId > 0)
			{
				gCommandParser.NextState = null;

				var room = gRDB[gGameState.Ro];

				Debug.Assert(room != null);

				switch (gCommandParser.DecorationId)
				{
					case 1:

						gOut.Print("Those gates are made of rusted iron and are definitely out of repair.");

						break;

					case 2:

						gOut.Print("The fence is constructed of sturdy iron rods, each 9 feet tall, and sitting within a half-foot of the next.  Climbing would be foolhardy - the top of each has been filed to a sharp spear-like point.");

						break;

					case 3:

						gOut.Print("All plant life is lush and green, due to the frequent rainstorms that pass through the area.  The climate is perfect for such growth.");

						break;

					case 4:

						gOut.Print("You have little difficulty finding a typical tombstone to examine.  The normal appears to be made of granite, and come in all shapes and sizes, ranging from tiny (1 or 2 feet) to gigantic (over 12 feet).  Many have etchings that are still quite legible.");

						break;

					case 5:

						gOut.Print("The stream is small, and the water is clear.  Pebbles can be seen at the bottom.  The 1-foot wide stream causes the ground around it to become quite damp, if not outright soggy.");

						break;

					case 6:

						gOut.Print("You approach it, but start to gag and quickly retreat.  The smell makes your stomach churn.");

						break;

					case 7:

						gOut.Print("The rat has been devoured from the waist down and has made a bloody mess of the rock.  Maybe it was butchered by an owl or vulture.  Either way, it was probably killed a while ago.");

						break;

					case 8:

						gOut.Print("The rocks have probably been here for quite a while:  tall weeds grow up through and around them.  It stands about 5 feet tall.");

						break;

					case 9:

						gOut.Print("These elms are all rather old, over 100 years, you would guess.  Their long branches stretch out over the damp ground, an imposing image.  They are entirely bare, stripped of leaves.");

						break;

					case 10:

						gOut.Print("The grave, to your great surprise, has been dug out very recently, probably no more than {0} ago.  Hmm... it looks to be about your size.",
							gGameState.Day > 0 ? string.Format("{0} day{1}", gEngine.GetStringFromNumber(gGameState.Day, false, Globals.Buf), gGameState.Day != 1 ? "s" : "") :
							gGameState.Hour > 0 ? string.Format("{0} hour{1}", gEngine.GetStringFromNumber(gGameState.Hour, false, Globals.Buf), gGameState.Hour != 1 ? "s" : "") :
							string.Format("{0} minute{1}", gEngine.GetStringFromNumber(gGameState.Minute, false, Globals.Buf), gGameState.Minute != 1 ? "s" : ""));

						break;

					case 11:

						gOut.Print("It has been lying there for quite some time:  the bones are picked completely clean.  Not quite sure what type of animal it was, though.");

						break;

					case 12:

						gOut.Print("These things are ancient, but they look just like pines.  Very green.");

						break;

					case 13:

						gOut.Print("The coffin looks as though it has been expelled from the ground.  The skeleton inside still has rotting clothing on.");

						break;

					case 14:

						gOut.Print("The dust on the floor makes you choke; there's so much of it.  You wonder how long it's been since human feet walked these corridors.");

						break;

					case 15:

						gOut.Print("This pile of bones has been here for quite some time - they've been picked thoroughly clean.");

						break;

					case 16:

						gOut.Print("These openings were apparently once used for the storage of dead bodies.  Many of them have been sealed over with red bricks and mortar, and a few of these have crumbled away, revealing the remains of their occupants.  The remainder are empty.");

						break;

					case 17:

						gOut.Print("The frescoes which cover the walls convey a variety of ideas.  The majority deal with the passage from life into the \"deep sleep\" of death, then finally onto the afterlife in the netherworlds.  Many of them are of stunning artistic quality, yet some hint at dark secrets and seem oddly out of place with the others.");

						break;

					case 18:

						// TODO

						break;

					case 19:

						gOut.Print("The door is made of oak and is severely dilapidated.  The thing is rotted, almost entirely through in some places.");

						break;

					case 20:

						gOut.Print("It doesn't take much to figure out what that stuff is.  From the looks of it, the blood stain has been here for possibly years.");

						break;

					case 21:

						gOut.Print("Unfortunately, the inscription is in a foreign tongue and isn't of much help to you.");

						break;

					case 22:

						gOut.Print("The skeleton has been here quite a while, and so have the leathers.  They're of no use to you - age has taken its toll.");

						break;

					case 23:

						gOut.Print("The walls show signs of work, large holes, and small.  However, the work was never completed.");

						break;

					case 24:

						gOut.Print("The moss is brilliant green (almost phosphorescent) and grows in patches all about the room.  The growth on the ceiling hangs down over 2 feet in places.");

						break;

					case 25:

						gOut.Print("The mosses growing about in this room are soft and springy, and surprisingly well nourished.  The various colors lend to a strange visual effect where they have intergrown with each other.");

						break;

					case 26:

						gOut.Print("The box is empty, and there isn't much more to say about it.");

						break;

					case 27:

						gOut.Print("The algae in this room provide sufficient illumination to see.  It covers the upper parts of the walls and ceiling.  You try to pick some of it, but quickly discover that it loses its unique properties that way.");

						break;

					case 28:

						gOut.Print("The grooves are perfectly cut at a 45-degree angle.  There are no apparent tool marks, either.");

						break;

					case 29:

						gOut.Print("The bow is broken and of no use to you.");

						break;

					case 30:

						gOut.Print("The hole, which is several inches across, is far too small to fit into (and definitely too high up to reach), but when you stand directly under it, you can see {0}.",
							gGameState.IsDayTime() ? "blue skies above you" : "the dark nighttime sky above");

						break;

					case 31:

						gOut.Print("The strip of cloth is just that:  cloth.");

						break;

					case 32:

						gOut.Print("The rocks are small and, looking upward, have been dislodged from the ceiling.");

						break;

					case 33:

						gOut.Print("The mummy is ancient, probably several hundred years old.  You'd better not touch it - no knowing what types of diseases it might carry.");

						break;

					case 34:

						gOut.Print("They're just normal spiderwebs, dancing slowly in the slight breeze.");

						break;

					case 35:

						gOut.Print("The frescoes depict a variety of arcane rituals.  Some show scenes of bloody and obscene sacrifice before a pagan god, while others show the times and lifestyles of the people of the region.  If you remember your lessons correctly, these beliefs were held by a group of settlers who lived in the heart of the forested lands many years ago.  As you move eastward down the corridor, the workmanship and artistic quality become more precise.");

						break;

					case 36:

						// TODO

						break;

					case 37:

						gOut.Print("These fine etchings are expertly crafted and show few signs of age.  They deal with contrasting views on the topic of enlightenment.  Some feel there are many steps required to achieve it as a corporeal being, from simple meditation to advanced astral projection.  In contrast, others believe one's \"purpose\" is determined by the gods at birth, and thus not known until the individual in question passes on to the afterlife.  Paradoxically, only through death, could the spirit understand its ultimate purpose.");

						break;

					case 38:

						gOut.Print("You would suppose these faces belong to the ones who successfully met with destiny, and ultimately fulfilled their \"purpose\" in life (or rather in death), according to popular belief.  Strangely, you notice that many a visage bears the look of pain and suffering.");

						break;

					case 39:

						gOut.Print("The goblin's belongings have been rifled through, thoroughly searched. The body itself is hacked to pieces - missing an arm, which sits in one corner of the chamber, and a leg in the other corner.  There are severe slashes; blood covers articles of clothing and the cold stone floor.  Its throat has been slit.");

						break;

					case 40:

						gOut.Print("Don't bother - it doesn't look to be your size.  Besides, removing the body would be a grisly task, one which you could do without.");

						break;

					case 41:

						gOut.Print("The slime-like substance has an ochre color.");

						break;

					case 42:

						gOut.Print("The boot is old and made of animal skin (probably lizard).  The mound of earth is dried up, hard to tell where it came from.");

						break;

					case 43:

						gOut.Print("The dead goblins have been slain recently; you'd say no more than {0} days ago.  They appear to have been slashed severely, with well-placed strokes - blood lies splattered all over the walls and the floor.  All useful items have been taken from the bodies.",
							gEngine.GetStringFromNumber(gGameState.Day + 2, false, Globals.Buf));

						break;

					case 44:

						gOut.Print("The spoor trails the length of the floor, suggesting frequent usage.");

						break;

					case 45:

						gOut.Print("The fog is impossible to see through.");

						break;

					case 46:

						gOut.Print("The pickaxe marks are numerous, especially on the north and south walls.  The workmanship is decent, though you have seen better.  Chips and splinters of rock lie across the floor.");

						break;

					case 47:

						gOut.Print("They are magnificent, but many are rotted and motheaten to the point of worthlessness.");

						break;

					case 48:

						gOut.Print("There are numerous mining tools of various types:  picks, shovels, hammers, chisels.  Unfortunately, most are broken and are of no use to you.");

						break;

					case 49:

						gOut.Print("The arm is covered with rotting flesh.  Pretty nasty.");

						break;

					case 50:
					{
						gOut.Print("The pit is deep and dark.  You can't see the bottom even when you shine your light into it.  Better watch your step.");

						var rl = gEngine.RollDice(1, 100, 0);

						if (rl > 70)
						{
							var saved = gEngine.SaveThrow(0);

							gOut.Print("Whoops!  You are too close to the edge; the part of the floor on which you stand crumbles into dust.  {0}", saved ? "But luckily, you manage to leap aside, thus saving yourself." : "You tumble into the pit and perish on the way down.");

							if (!saved)
							{
								gGameState.Die = 1;

								gCommandParser.NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
								{
									x.PrintLineSep = true;
								});

								goto Cleanup;
							}
						}

						break;
					}

					case 51:

						gOut.Print("That skeleton has been there for quite some time.  The bones are so clean they appear to be polished.");

						break;

					case 52:

						gOut.Print("The stains on the eastern wall are obviously dried blood.{0}", gEngine.SaveThrow(Stat.Intellect) ? "  You notice that the stains abruptly end at a fine vertical crack in the wall.  Strange." : "");

						break;

					case 53:

						gOut.Print("The carvings are mostly individual works by separate artists.  However, they all seem to share a common theme - reincarnation.");

						break;

					case 54:
					{
						var saved = gEngine.SaveThrow(Stat.Intellect);

						var target = gEngine.GetRandomMonsterList(1, m => m.IsInRoom(room)).FirstOrDefault();

						Debug.Assert(target != null);

						var rl = gEngine.RollDice(1, 100, 0);

						gOut.Print("The face is that of an old man, tired and sad.  A long beard and bushy brows are evident features.{0}", 
							saved ? "  As you begin to examine it, you realize that the thing is trapped.  You'd best stay away from it." : 
							string.Format("  As you examine it, a crossbow bolt shoots out of the face's mouth, and strikes {0}!", 
								rl > 50 ? (target.IsCharacterMonster() ? "you" : target.GetTheName()) : "the opposite wall"));

						if (!saved && rl > 50)
						{
							var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
							{
								x.SetNextStateFunc = s => gCommandParser.NextState = s;

								x.DfMonster = target;
							});

							combatSystem.ExecuteCalculateDamage(1, 6);

							goto Cleanup;
						}

						break;
					}

					case 55:

						gOut.Print("That's a horrible sight.  There are at least six bodies, some human, some not, but all are badly mutilated and partially eaten.  They're stacked up like firewood in one corner of the room.");

						break;

					case 56:
					{
						var giantCrayfishMonster = gMDB[37];

						Debug.Assert(giantCrayfishMonster != null);

						if (!giantCrayfishMonster.IsInLimbo() || gGameState.GiantCrayfishKilled)
						{
							gOut.Print("You pause to take in the idyllic ocean vista.  Blue waves gently wash over the light brown sand, the cries of sea-birds echo from the cliffs above, and the smell of salt fills the air.");
						}
						else
						{
							gOut.Print("As you look at your surroundings, a large marine animal scuttles out from behind a giant rock, and quickly moves towards you!");

							giantCrayfishMonster.SetInRoom(room);

							var saved = gEngine.SaveThrow(Stat.Agility);

							if (!saved)
							{
								gOut.Print("You've been taken entirely by surprise.");

								Globals.GiantCrayfishSurprises = true;
							}
							else
							{
								gCommandParser.NextState = Globals.CreateInstance<IStartState>();
							}
						}

						break;
					}
				}

			Cleanup:

				if (gCommandParser.NextState == null)
				{
					gCommandParser.NextState = Globals.CreateInstance<IMonsterStartState>();
				}
			}
			else
			{
				gCommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
				{
					a => a.IsCarriedByCharacter() || a.IsInRoom(gActorRoom),
					a => a.IsEmbeddedInRoom(gActorRoom),
					a => a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(gActorRoom, gEngine.ExposeContainersRecursively),
					a => a.IsWornByCharacter()
				};

				if (!Enum.IsDefined(typeof(ContainerType), ContainerType))
				{
					gCommandParser.ObjData.RevealEmbeddedArtifactFunc = (r, a) => { };
				}

				gCommandParser.ObjData.ArtifactMatchFunc = PlayerArtifactMatch01;

				gCommandParser.ObjData.MonsterMatchFunc = PlayerMonsterMatch02;

				gCommandParser.ObjData.MonsterNotFoundFunc = PrintYouSeeNothingSpecial;

				PlayerResolveArtifact();
			}
		}
	}
}
