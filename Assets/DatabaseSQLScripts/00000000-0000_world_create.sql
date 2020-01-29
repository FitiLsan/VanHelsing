--
-- File generated with SQLiteStudio v3.2.1 on Вс июн 30 18:59:11 2019
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;

-- Table: quest
DROP TABLE IF EXISTS quest;

CREATE TABLE quest (
    Id            INTEGER PRIMARY KEY AUTOINCREMENT
                          NOT NULL,
    MinLevel      INTEGER DEFAULT (1),
    QuestLevel    INTEGER DEFAULT (1),
    TimeAllowed   INTEGER DEFAULT ( -1),
    ZoneId        INTEGER DEFAULT (1),
    RewardExp     INTEGER,
    RewardMoney   INTEGER,
    StartDialogId INTEGER,
    EndDialogId   INTEGER
);


-- Table: quest_locale_ru
DROP TABLE IF EXISTS quest_locale_ru;

CREATE TABLE quest_locale_ru (
    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
    QuestId     INTEGER REFERENCES quest (Id) ON DELETE CASCADE,
    Title       TEXT,
    Description TEXT
);


-- Table: quest_objectives
DROP TABLE IF EXISTS quest_objectives;

CREATE TABLE quest_objectives (
    Id       INTEGER     PRIMARY KEY AUTOINCREMENT
                         NOT NULL,
    QuestId  INTEGER     REFERENCES quest (Id) ON DELETE CASCADE,
    Type     INTEGER (3) NOT NULL,
    TargetId INTEGER     NOT NULL,
    Amount   INTEGER     NOT NULL
                         DEFAULT (1) 
);


-- Table: quest_objectives_locale_ru
DROP TABLE IF EXISTS quest_objectives_locale_ru;

CREATE TABLE quest_objectives_locale_ru (
    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
    ObjectiveId INTEGER REFERENCES quest_objectives (Id) ON DELETE CASCADE,
    Title       TEXT
);


-- Table: quest_poi
DROP TABLE IF EXISTS quest_poi;

CREATE TABLE quest_poi (
    Id         INTEGER     PRIMARY KEY AUTOINCREMENT
                           NOT NULL,
    QuestId    INTEGER     REFERENCES quest (Id),
    ZoneId     INTEGER     NOT NULL,
    X          DOUBLE      NOT NULL,
    Y          DOUBLE      NOT NULL,
    MarkerType INTEGER (3) NOT NULL
                           DEFAULT (1) 
);


-- Table: quest_requirements
DROP TABLE IF EXISTS quest_requirements;

CREATE TABLE quest_requirements (
    Id            INTEGER PRIMARY KEY AUTOINCREMENT,
    TargetQuestId INTEGER REFERENCES quest (Id) ON DELETE CASCADE,
    RequiredQuest INTEGER REFERENCES quest (Id) ON DELETE CASCADE 
);


-- Table: quest_rewards
DROP TABLE IF EXISTS quest_rewards;

CREATE TABLE quest_rewards (
    Id                INTEGER     PRIMARY KEY AUTOINCREMENT,
    QuestId           INTEGER     REFERENCES quest (Id) ON DELETE CASCADE,
    RewardType        INTEGER (3) NOT NULL,
    RewardObjectType  INTEGER (3) NOT NULL,
    RewardObjectId    INTEGER,
    RewardObjectCount INTEGER
);



PRAGMA foreign_keys = on;

-- Queries executed on database world (D:/Dev/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Date and time of execution: 2019-07-10 16:21:47
CREATE TABLE ddl_info (Id INTEGER PRIMARY KEY AUTOINCREMENT, Patch TEXT NOT NULL);

INSERT INTO ddl_info (Patch) VALUES ('00000000-0000_world_create.sql');