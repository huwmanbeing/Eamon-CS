# Eamon-CS
### The Wonderful World of Eamon (C# Branch)

This is Eamon CS (ECS), a C# port of the classic Eamon roleplaying game that debuted on the Apple II.  Eamon was created by Donald Brown, but there have been many versions over the years, on a variety of computer systems.  ECS is the production version of Eamon AC (EAC), a prototype intended to extract the game from BASIC.  EAC has been obsoleted in favor of this Eamon, which hopefully will be the definitive version for the C family of languages.

#### Prerequisites

Eamon CS requires .NET 4.5+ on Windows and Mono 4.6.2+ on Unix.  All modern Windows platforms come with the latest .NET runtime installed, but for Unix you may have to do a manual Mono install depending on your distribution.

#### Installing

There is no formal installer for Eamon CS.  To obtain a copy of this repository (which contains a full set of binaries) you can either do a Git Clone using Visual Studio 2017 or, more simply, download a .zip file using the green Clone Or Download button above.

If you are on Windows and choose the second option, prior to unzipping the file, you should right click on it, select Properties and click the Unblock check box (or button) in the lower right corner of the form.  Then click Apply and OK.  This will improve the gameplay experience by eliminating security warning message boxes.

It is possible to have multiple ECS repositories present on your system; provided they are in separate directories, they will not interfere with each other.  This can be useful in upgrade scenarios.  Unfortunately, the big improvements for Eamon CS 1.3.0 once again changed the data files (but much for the better).  You can now run EamonDD 1.X.0 along side EamonDD 1.3.0 and transfer your characters more easily (but still manually).

#### Playing

ECS programs are launched using a collection of batch files (or shell scripts in Unix) that are located under the QuickLaunch directory.  You can create a shortcut to this folder on your desktop for easy access to the system.

#### Contributing

Like all Eamons, ECS allows you to create adventures with no programming involved, via the EamonDD data file editor.  But for the intrepid game designer, the engine is infinitely extensible, using typical C# subclassing mechanisms.  The documentation at this point is lacking but there are multiple example adventures that can be recompiled in Debug mode and stepped through to gain a better understanding of the system.  The README.htm file (which will be rebuilt ASAP) goes through this in more detail.

If you are interested in contributing to the Eamon CS project, or you wish to port your own game, or build a new one, please contact me.  I can provide insight if there are areas of the code that need clarification.  Eamon has always been an ideal programmer's learning tool; if you build a game you aren't just contributing to the system, you're honing your skills as a C# developer while having fun doing it!

#### Roadmap

The current plan is to produce one fully polished game per quarter (until I run out of suitable candidates) beginning in Q3 2017.  These are ported BASIC games; new games will take longer, as they are showcases for the ECS engine.  There is no timeframe for when new games will be built.  If you have an old BASIC game that you'd like to see ported and are willing to assist in that task (just through your insight) you'll get priority.  Otherwise, the emphasis here is quality over quantity.

There are currently plans to port ECS to Android and, if that is successful, iOS.

There are a vast number of 3rd party technologies that can seamlessly integrate with ECS (being a C# system), some of which may push the game into places its never been.  Stay tuned and see what comes of it.

#### License

Eamon CS is released as free software under the GNU General Public License.  See LICENSE.txt in the Documentation directory for more details.

