--
-- File generated with SQLiteStudio v3.2.1 on Âò èþë 16 02:52:59 2019
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: ddl_info
DROP TABLE IF EXISTS ddl_info;

CREATE TABLE ddl_info (
    Id    INTEGER PRIMARY KEY AUTOINCREMENT
                  NOT NULL,
    Patch TEXT    NOT NULL
                  DEFAULT (0) 
);


-- Table: equipment
DROP TABLE IF EXISTS equipment;

CREATE TABLE equipment (
    Entry         INTEGER PRIMARY KEY AUTOINCREMENT
                          NOT NULL,
    ItemId        INTEGER NOT NULL,
    Count         INTEGER DEFAULT (1) 
                          NOT NULL,
    TimeLeft      INTEGER NOT NULL
                          DEFAULT ( -1),
    ScriptUsed    INTEGER NOT NULL
                          DEFAULT (0),
    SpellCharges1 INTEGER NOT NULL
                          DEFAULT (0),
    SpellCharges2 INTEGER NOT NULL
                          DEFAULT (0),
    Durability    INTEGER NOT NULL
                          DEFAULT (0),
    Slot          INTEGER NOT NULL
                          DEFAULT (0) 
);


-- Table: inventory
DROP TABLE IF EXISTS inventory;

CREATE TABLE inventory (
    Entry         INTEGER PRIMARY KEY AUTOINCREMENT
                          NOT NULL,
    ItemId        INTEGER NOT NULL,
    Count         INTEGER DEFAULT (1) 
                          NOT NULL,
    TimeLeft      INTEGER NOT NULL
                          DEFAULT ( -1),
    ScriptUsed    INTEGER NOT NULL
                          DEFAULT (0),
    SpellCharges1 INTEGER NOT NULL
                          DEFAULT (0),
    SpellCharges2 INTEGER NOT NULL
                          DEFAULT (0),
    Durability    INTEGER NOT NULL
                          DEFAULT (0) 
);


-- Table: quest
DROP TABLE IF EXISTS quest;

CREATE TABLE quest (
    Id       INTEGER PRIMARY KEY AUTOINCREMENT
                     NOT NULL,
    QuestId  INTEGER NOT NULL,
    TimeLeft INTEGER NOT NULL
                     DEFAULT ( -1) 
);


-- Table: quest_objectives
DROP TABLE IF EXISTS quest_objectives;

CREATE TABLE quest_objectives (
    Id          INTEGER PRIMARY KEY AUTOINCREMENT
                        NOT NULL,
    ObjectiveId INTEGER NOT NULL,
    Value       INTEGER NOT NULL
                        DEFAULT (0) 
);


COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
