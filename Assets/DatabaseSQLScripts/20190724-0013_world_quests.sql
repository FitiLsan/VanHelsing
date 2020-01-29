-- Queries executed on database world (D:/Dev/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Date and time of execution: 2019-07-24 00:12:00
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM quest_rewards;
DROP TABLE quest_rewards;
CREATE TABLE quest_rewards (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, QuestId INTEGER REFERENCES quest (Id) ON DELETE CASCADE NOT NULL, RewardType INTEGER (3) NOT NULL DEFAULT (1), RewardObjectType INTEGER (3) NOT NULL DEFAULT (1), RewardObjectId INTEGER NOT NULL DEFAULT (0), RewardObjectCount INTEGER NOT NULL DEFAULT (1));
INSERT INTO quest_rewards (Id, QuestId, RewardType, RewardObjectType, RewardObjectId, RewardObjectCount) SELECT Id, QuestId, RewardType, RewardObjectType, RewardObjectId, RewardObjectCount FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Queries executed on database world (D:/Dev/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Date and time of execution: 2019-07-24 00:12:06
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM quest_requirements;
DROP TABLE quest_requirements;
CREATE TABLE quest_requirements (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, TargetQuestId INTEGER REFERENCES quest (Id) ON DELETE CASCADE NOT NULL, RequiredQuest INTEGER REFERENCES quest (Id) ON DELETE CASCADE NOT NULL);
INSERT INTO quest_requirements (Id, TargetQuestId, RequiredQuest) SELECT Id, TargetQuestId, RequiredQuest FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Queries executed on database world (D:/Dev/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Date and time of execution: 2019-07-24 00:12:15
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM quest_poi;
DROP TABLE quest_poi;
CREATE TABLE quest_poi (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, QuestId INTEGER REFERENCES quest (Id) NOT NULL, ZoneId INTEGER NOT NULL, X DOUBLE NOT NULL, Y DOUBLE NOT NULL, MarkerType INTEGER (3) NOT NULL DEFAULT (1));
INSERT INTO quest_poi (Id, QuestId, ZoneId, X, Y, MarkerType) SELECT Id, QuestId, ZoneId, X, Y, MarkerType FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Queries executed on database world (D:/Dev/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Date and time of execution: 2019-07-24 00:12:18
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM quest_objectives_locale_ru;
DROP TABLE quest_objectives_locale_ru;
CREATE TABLE quest_objectives_locale_ru (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ObjectiveId INTEGER REFERENCES quest_objectives (Id) ON DELETE CASCADE NOT NULL, Title TEXT NOT NULL DEFAULT Задача);
INSERT INTO quest_objectives_locale_ru (Id, ObjectiveId, Title) SELECT Id, ObjectiveId, Title FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Queries executed on database world (D:/Dev/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Date and time of execution: 2019-07-24 00:12:21
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM quest_objectives;
DROP TABLE quest_objectives;
CREATE TABLE quest_objectives (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, QuestId INTEGER REFERENCES quest (Id) ON DELETE CASCADE NOT NULL, Type INTEGER (3) NOT NULL, TargetId INTEGER NOT NULL, Amount INTEGER NOT NULL DEFAULT (1));
INSERT INTO quest_objectives (Id, QuestId, Type, TargetId, Amount) SELECT Id, QuestId, Type, TargetId, Amount FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Queries executed on database world (D:/Dev/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Date and time of execution: 2019-07-24 00:12:24
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM quest_locale_ru;
DROP TABLE quest_locale_ru;
CREATE TABLE quest_locale_ru (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, QuestId INTEGER REFERENCES quest (Id) ON DELETE CASCADE NOT NULL DEFAULT (0), Title TEXT NOT NULL DEFAULT "Название квеста", Description TEXT NOT NULL DEFAULT "Описание квеста");
INSERT INTO quest_locale_ru (Id, QuestId, Title, Description) SELECT Id, QuestId, Title, Description FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Queries executed on database world (D:/Dev/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Date and time of execution: 2019-07-24 00:12:27
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM quest;
DROP TABLE quest;
CREATE TABLE quest (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, MinLevel INTEGER DEFAULT (1) NOT NULL, QuestLevel INTEGER DEFAULT (1) NOT NULL, TimeAllowed INTEGER DEFAULT (- 1) NOT NULL, ZoneId INTEGER DEFAULT (1) NOT NULL, RewardExp INTEGER NOT NULL DEFAULT (0), RewardMoney INTEGER NOT NULL DEFAULT (0), StartDialogId INTEGER NOT NULL DEFAULT (0), EndDialogId INTEGER DEFAULT (0) NOT NULL);
INSERT INTO quest (Id, MinLevel, QuestLevel, TimeAllowed, ZoneId, RewardExp, RewardMoney, StartDialogId, EndDialogId) SELECT Id, MinLevel, QuestLevel, TimeAllowed, ZoneId, RewardExp, RewardMoney, StartDialogId, EndDialogId FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;