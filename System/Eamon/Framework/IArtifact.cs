﻿
// IArtifact.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface IArtifact : IGameBase, IComparable<IArtifact>
	{
		#region Properties

		/// <summary></summary>
		string StateDesc { get; set; }

		/// <summary></summary>
		bool IsCharOwned { get; set; }

		/// <summary></summary>
		bool IsPlural { get; set; }

		/// <summary></summary>
		bool IsListed { get; set; }

		/// <summary></summary>
		PluralType PluralType { get; set; }

		/// <summary></summary>
		long Value { get; set; }

		/// <summary></summary>
		long Weight { get; set; }

		/// <summary></summary>
		long Location { get; set; }

		/// <summary></summary>
		ArtifactType Type { get; set; }

		/// <summary></summary>
		long Field1 { get; set; }

		/// <summary></summary>
		long Field2 { get; set; }

		/// <summary></summary>
		long Field3 { get; set; }

		/// <summary></summary>
		long Field4 { get; set; }

		/// <summary></summary>
		long Field5 { get; set; }

		/// <summary></summary>
		IArtifactCategory Gold { get; }

		/// <summary></summary>
		IArtifactCategory Treasure { get; }

		/// <summary></summary>
		IArtifactCategory Weapon { get; }

		/// <summary></summary>
		IArtifactCategory MagicWeapon { get; }

		/// <summary></summary>
		IArtifactCategory GeneralWeapon { get; }

		/// <summary></summary>
		IArtifactCategory Container { get; }

		/// <summary></summary>
		IArtifactCategory LightSource { get; }

		/// <summary></summary>
		IArtifactCategory Drinkable { get; }

		/// <summary></summary>
		IArtifactCategory Readable { get; }

		/// <summary></summary>
		IArtifactCategory DoorGate { get; }

		/// <summary></summary>
		IArtifactCategory Edible { get; }

		/// <summary></summary>
		IArtifactCategory BoundMonster { get; }

		/// <summary></summary>
		IArtifactCategory Wearable { get; }

		/// <summary></summary>
		IArtifactCategory DisguisedMonster { get; }

		/// <summary></summary>
		IArtifactCategory DeadBody { get; }

		/// <summary></summary>
		IArtifactCategory User1 { get; }

		/// <summary></summary>
		IArtifactCategory User2 { get; }

		/// <summary></summary>
		IArtifactCategory User3 { get; }

		/// <summary></summary>
		IArtifactCategory[] Categories { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IArtifactCategory GetCategories(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetSynonyms(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetCategories(long index, IArtifactCategory value);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetSynonyms(long index, string value);

		/// <summary></summary>
		/// <returns></returns>
		bool IsCarriedByCharacter();

		/// <summary></summary>
		/// <returns></returns>
		bool IsCarriedByMonster();

		/// <summary></summary>
		/// <returns></returns>
		bool IsCarriedByContainer();

		/// <summary></summary>
		/// <returns></returns>
		bool IsWornByCharacter();

		/// <summary></summary>
		/// <returns></returns>
		bool IsWornByMonster();

		/// <summary></summary>
		/// <returns></returns>
		bool IsReadyableByCharacter();

		/// <summary></summary>
		/// <returns></returns>
		bool IsInRoom();

		/// <summary></summary>
		/// <returns></returns>
		bool IsEmbeddedInRoom();

		/// <summary></summary>
		/// <returns></returns>
		bool IsInLimbo();

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		/// <returns></returns>
		bool IsCarriedByMonsterUid(long monsterUid);

		/// <summary></summary>
		/// <param name="containerUid"></param>
		/// <returns></returns>
		bool IsCarriedByContainerUid(long containerUid);

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		/// <returns></returns>
		bool IsWornByMonsterUid(long monsterUid);

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		/// <returns></returns>
		bool IsReadyableByMonsterUid(long monsterUid);

		/// <summary></summary>
		/// <param name="roomUid"></param>
		/// <returns></returns>
		bool IsInRoomUid(long roomUid);

		/// <summary></summary>
		/// <param name="roomUid"></param>
		/// <returns></returns>
		bool IsEmbeddedInRoomUid(long roomUid);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		bool IsCarriedByMonster(IMonster monster);

		/// <summary></summary>
		/// <param name="container"></param>
		/// <returns></returns>
		bool IsCarriedByContainer(IArtifact container);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		bool IsWornByMonster(IMonster monster);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		bool IsReadyableByMonster(IMonster monster);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <returns></returns>
		bool IsInRoom(IRoom room);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <returns></returns>
		bool IsEmbeddedInRoom(IRoom room);

		/// <summary></summary>
		/// <returns></returns>
		long GetCarriedByMonsterUid();

		/// <summary></summary>
		/// <returns></returns>
		long GetCarriedByContainerUid();

		/// <summary></summary>
		/// <returns></returns>
		long GetWornByMonsterUid();

		/// <summary></summary>
		/// <returns></returns>
		long GetInRoomUid();

		/// <summary></summary>
		/// <returns></returns>
		long GetEmbeddedInRoomUid();

		/// <summary></summary>
		/// <returns></returns>
		IMonster GetCarriedByMonster();

		/// <summary></summary>
		/// <returns></returns>
		IArtifact GetCarriedByContainer();

		/// <summary></summary>
		/// <returns></returns>
		IMonster GetWornByMonster();

		/// <summary></summary>
		/// <returns></returns>
		IRoom GetInRoom();

		/// <summary></summary>
		/// <returns></returns>
		IRoom GetEmbeddedInRoom();

		/// <summary></summary>
		void SetCarriedByCharacter();

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		void SetCarriedByMonsterUid(long monsterUid);

		/// <summary></summary>
		/// <param name="containerUid"></param>
		void SetCarriedByContainerUid(long containerUid);

		/// <summary></summary>
		void SetWornByCharacter();

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		void SetWornByMonsterUid(long monsterUid);

		/// <summary></summary>
		/// <param name="roomUid"></param>
		void SetInRoomUid(long roomUid);

		/// <summary></summary>
		/// <param name="roomUid"></param>
		void SetEmbeddedInRoomUid(long roomUid);

		/// <summary></summary>
		void SetInLimbo();

		/// <summary></summary>
		/// <param name="monster"></param>
		void SetCarriedByMonster(IMonster monster);

		/// <summary></summary>
		/// <param name="container"></param>
		void SetCarriedByContainer(IArtifact container);

		/// <summary></summary>
		/// <param name="monster"></param>
		void SetWornByMonster(IMonster monster);

		/// <summary></summary>
		/// <param name="room"></param>
		void SetInRoom(IRoom room);

		/// <summary></summary>
		/// <param name="room"></param>
		void SetEmbeddedInRoom(IRoom room);

		/// <summary></summary>
		/// <returns></returns>
		bool IsInRoomLit();

		/// <summary></summary>
		/// <returns></returns>
		bool IsEmbeddedInRoomLit();

		/// <summary></summary>
		/// <param name="value"></param>
		/// <returns></returns>
		bool IsFieldStrength(long value);

		/// <summary></summary>
		/// <param name="value"></param>
		/// <returns></returns>
		long GetFieldStrength(long value);

		/// <summary></summary>
		/// <param name="weapon"></param>
		/// <returns></returns>
		bool IsWeapon(Weapon weapon);

		/// <summary></summary>
		/// <returns></returns>
		bool IsAttackable();

		/// <summary></summary>
		/// <param name="ac"></param>
		/// <returns></returns>
		bool IsAttackable01(ref IArtifactCategory ac);

		/// <summary></summary>
		/// <returns></returns>
		bool IsUnmovable();

		/// <summary></summary>
		/// <returns></returns>
		bool IsUnmovable01();

		/// <summary></summary>
		/// <returns></returns>
		bool IsArmor();

		/// <summary></summary>
		/// <returns></returns>
		bool IsShield();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldShowContentsWhenExamined();

		/// <summary></summary>
		/// <returns></returns>
		string GetProvidingLightDesc();

		/// <summary></summary>
		/// <returns></returns>
		string GetReadyWeaponDesc();

		/// <summary></summary>
		/// <returns></returns>
		string GetBrokenDesc();

		/// <summary></summary>
		/// <returns></returns>
		string GetEmptyDesc();

		/// <summary></summary>
		/// <param name="singularValue"></param>
		/// <param name="pluralValue"></param>
		/// <returns></returns>
		T EvalPlural<T>(T singularValue, T pluralValue);

		/// <summary></summary>
		/// <param name="darkValue"></param>
		/// <param name="lightValue"></param>
		/// <returns></returns>
		T EvalInRoomLightLevel<T>(T darkValue, T lightValue);

		/// <summary></summary>
		/// <param name="darkValue"></param>
		/// <param name="lightValue"></param>
		/// <returns></returns>
		T EvalEmbeddedInRoomLightLevel<T>(T darkValue, T lightValue);

		/// <summary></summary>
		/// <param name="artifactType"></param>
		/// <returns></returns>
		IArtifactCategory GetArtifactCategory(ArtifactType artifactType);

		/// <summary></summary>
		/// <param name="artifactTypes"></param>
		/// <param name="categoryArrayPrecedence"></param>
		/// <returns></returns>
		IArtifactCategory GetArtifactCategory(ArtifactType[] artifactTypes, bool categoryArrayPrecedence = true);

		/// <summary></summary>
		/// <param name="artifactTypes"></param>
		/// <returns></returns>
		IList<IArtifactCategory> GetArtifactCategories(ArtifactType[] artifactTypes);

		/// <summary></summary>
		/// <param name="count"></param>
		/// <returns></returns>
		RetCode SetArtifactCategoryCount(long count);

		/// <summary></summary>
		/// <param name="artifactCategory"></param>
		/// <returns></returns>
		RetCode SyncArtifactCategories(IArtifactCategory artifactCategory);

		/// <summary></summary>
		/// <returns></returns>
		RetCode SyncArtifactCategories();

		/// <summary></summary>
		/// <param name="stateDesc"></param>
		/// <param name="dupAllowed"></param>
		/// <returns></returns>
		RetCode AddStateDesc(string stateDesc, bool dupAllowed = false);

		/// <summary></summary>
		/// <param name="stateDesc"></param>
		/// <returns></returns>
		RetCode RemoveStateDesc(string stateDesc);

		/// <summary></summary>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IList<IArtifact> GetContainedList(Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="count"></param>
		/// <param name="weight"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		RetCode GetContainerInfo(ref long count, ref long weight, bool recurse = false);

		#endregion
	}
}
