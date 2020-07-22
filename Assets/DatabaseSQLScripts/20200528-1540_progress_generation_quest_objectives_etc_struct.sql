PRAGMA foreign_keys = 0;
CREATE TABLE generation_quest_locale_ru (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, QuestId INTEGER REFERENCES quest (Id) ON DELETE CASCADE NOT NULL DEFAULT (0), Title TEXT NOT NULL DEFAULT "Title", Description TEXT NOT NULL DEFAULT "Description");
INSERT INTO generation_quest_locale_ru (Id, QuestId, Title, Description) SELECT Id, QuestId, Title, Description FROM quest_locale_ru;
DROP TABLE quest_locale_ru;
PRAGMA foreign_keys = 1;

PRAGMA foreign_keys = 0;
CREATE TABLE generation_quest_objectives_locale_ru (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ObjectiveId INTEGER REFERENCES quest_objectives (Id) ON DELETE CASCADE NOT NULL, Title TEXT NOT NULL DEFAULT Title);
INSERT INTO generation_quest_objectives_locale_ru (Id, ObjectiveId, Title) SELECT Id, ObjectiveId, Title FROM quest_objectives_locale_ru;
DROP TABLE quest_objectives_locale_ru;
PRAGMA foreign_keys = 1;

PRAGMA foreign_keys = 0;
CREATE TABLE generation_quest_poi (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, QuestId INTEGER REFERENCES quest (Id) NOT NULL, ZoneId INTEGER NOT NULL, X DOUBLE NOT NULL, Y DOUBLE NOT NULL, MarkerType INTEGER (3) NOT NULL DEFAULT (1));
INSERT INTO generation_quest_poi (Id, QuestId, ZoneId, X, Y, MarkerType) SELECT Id, QuestId, ZoneId, X, Y, MarkerType FROM quest_poi;
DROP TABLE quest_poi;
PRAGMA foreign_keys = 1;

PRAGMA foreign_keys = 0;
CREATE TABLE generation_quest_requirements (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, TargetQuestId INTEGER REFERENCES quest (Id) ON DELETE CASCADE NOT NULL, RequiredQuest INTEGER REFERENCES quest (Id) ON DELETE CASCADE NOT NULL);
INSERT INTO generation_quest_requirements (Id, TargetQuestId, RequiredQuest) SELECT Id, TargetQuestId, RequiredQuest FROM quest_requirements;
DROP TABLE quest_requirements;
PRAGMA foreign_keys = 1;

PRAGMA foreign_keys = 0;
CREATE TABLE generation_quest_rewards (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, QuestId INTEGER REFERENCES quest (Id) ON DELETE CASCADE NOT NULL, RewardType INTEGER (3) NOT NULL DEFAULT (1), RewardObjectType INTEGER (3) NOT NULL DEFAULT (1), RewardObjectId INTEGER NOT NULL DEFAULT (0), RewardObjectCount INTEGER NOT NULL DEFAULT (1));
INSERT INTO generation_quest_rewards (Id, QuestId, RewardType, RewardObjectType, RewardObjectId, RewardObjectCount) SELECT Id, QuestId, RewardType, RewardObjectType, RewardObjectId, RewardObjectCount FROM quest_rewards;
DROP TABLE quest_rewards;
PRAGMA foreign_keys = 1;