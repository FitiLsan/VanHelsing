PRAGMA foreign_keys = 0;

CREATE TABLE sqlitestudio_temp_table AS SELECT *
                                          FROM quest_objectives;

DROP TABLE quest_objectives;

CREATE TABLE quest_objectives (
    Id         INTEGER     PRIMARY KEY AUTOINCREMENT
                           NOT NULL,
    QuestId    INTEGER     REFERENCES quest (Id) ON DELETE CASCADE
                           NOT NULL,
    Type       INTEGER (3) NOT NULL,
    TargetId   INTEGER     NOT NULL,
    Amount     INTEGER     NOT NULL
                           DEFAULT (1),
    IsOptional INTEGER     NOT NULL
                           DEFAULT (0) 
);

INSERT INTO quest_objectives (
                                 Id,
                                 QuestId,
                                 Type,
                                 TargetId,
                                 Amount
                             )
                             SELECT Id,
                                    QuestId,
                                    Type,
                                    TargetId,
                                    Amount
                               FROM sqlitestudio_temp_table;

DROP TABLE sqlitestudio_temp_table;

PRAGMA foreign_keys = 1;
