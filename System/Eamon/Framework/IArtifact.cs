﻿
// IArtifact.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	public interface IArtifact : IGameBase, IComparable<IArtifact>
	{
		#region Properties

		string StateDesc { get; set; }

		bool IsCharOwned { get; set; }

		bool IsPlural { get; set; }

		bool IsListed { get; set; }

		Enums.PluralType PluralType { get; set; }

		long Value { get; set; }

		long Weight { get; set; }

		long Location { get; set; }

		Classes.IArtifactClass[] Classes { get; set; }

		#endregion

		#region Methods

		Classes.IArtifactClass GetClasses(long index);

		string GetSynonyms(long index);

		void SetClasses(long index, Classes.IArtifactClass value);

		void SetSynonyms(long index, string value);

		bool IsCarriedByCharacter();

		bool IsCarriedByMonster();

		bool IsCarriedByContainer();

		bool IsWornByCharacter();

		bool IsWornByMonster();

		bool IsReadyableByCharacter();

		bool IsInRoom();

		bool IsEmbeddedInRoom();

		bool IsInLimbo();

		bool IsCarriedByMonsterUid(long monsterUid);

		bool IsCarriedByContainerUid(long containerUid);

		bool IsWornByMonsterUid(long monsterUid);

		bool IsReadyableByMonsterUid(long monsterUid);

		bool IsInRoomUid(long roomUid);

		bool IsEmbeddedInRoomUid(long roomUid);

		bool IsCarriedByMonster(IMonster monster);

		bool IsCarriedByContainer(IArtifact container);

		bool IsWornByMonster(IMonster monster);

		bool IsReadyableByMonster(IMonster monster);

		bool IsInRoom(IRoom room);

		bool IsEmbeddedInRoom(IRoom room);

		long GetCarriedByMonsterUid();

		long GetCarriedByContainerUid();

		long GetWornByMonsterUid();

		long GetInRoomUid();

		long GetEmbeddedInRoomUid();

		IMonster GetCarriedByMonster();

		IArtifact GetCarriedByContainer();

		IMonster GetWornByMonster();

		IRoom GetInRoom();

		IRoom GetEmbeddedInRoom();

		void SetCarriedByCharacter();

		void SetCarriedByMonsterUid(long monsterUid);

		void SetCarriedByContainerUid(long containerUid);

		void SetWornByCharacter();

		void SetWornByMonsterUid(long monsterUid);

		void SetInRoomUid(long roomUid);

		void SetEmbeddedInRoomUid(long roomUid);

		void SetInLimbo();

		void SetCarriedByMonster(IMonster monster);

		void SetCarriedByContainer(IArtifact container);

		void SetWornByMonster(IMonster monster);

		void SetInRoom(IRoom room);

		void SetEmbeddedInRoom(IRoom room);

		bool IsInRoomLit();

		bool IsEmbeddedInRoomLit();

		bool IsFieldStrength(long value);

		long GetFieldStrength(long value);

		bool IsWeapon(Enums.Weapon weapon);

		bool IsAttackable();

		bool IsAttackable01(ref Classes.IArtifactClass ac);

		bool IsUnmovable();

		bool IsUnmovable01();

		bool IsGold();

		bool IsTreasure();

		bool IsWeapon();

		bool IsWeapon01();

		bool IsMagicWeapon();

		bool IsContainer();

		bool IsLightSource();

		bool IsDrinkable();

		bool IsReadable();

		bool IsDoorGate();

		bool IsEdible();

		bool IsBoundMonster();

		bool IsWearable();

		bool IsArmor();

		bool IsShield();

		bool IsDisguisedMonster();

		bool IsDeadBody();

		bool IsUser1();

		bool IsUser2();

		bool IsUser3();

		T EvalPlural<T>(T singularValue, T pluralValue);

		T EvalInRoomLightLevel<T>(T darkValue, T lightValue);

		T EvalEmbeddedInRoomLightLevel<T>(T darkValue, T lightValue);

		Classes.IArtifactClass GetArtifactClass(Enums.ArtifactType artifactType);

		Classes.IArtifactClass GetArtifactClass(Enums.ArtifactType[] artifactTypes, bool classArrayPrecedence = true);

		IList<Classes.IArtifactClass> GetArtifactClasses(Enums.ArtifactType[] artifactTypes);

		RetCode SetArtifactClassCount(long count);

		RetCode SyncArtifactClasses(Classes.IArtifactClass artifactClass);

		RetCode SyncArtifactClasses();

		RetCode AddStateDesc(string stateDesc, bool dupAllowed = false);

		RetCode RemoveStateDesc(string stateDesc);

		IList<IArtifact> GetContainedList(Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		RetCode GetContainerInfo(ref long count, ref long weight, bool recurse = false);

		string BuildValue(long bufSize, char fillChar, long offset, IField field);

		#endregion
	}
}
