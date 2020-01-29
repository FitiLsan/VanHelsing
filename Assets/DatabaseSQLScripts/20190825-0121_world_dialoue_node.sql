-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-08-25 00:24:59
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_answers;
DROP TABLE dialogue_answers;
CREATE TABLE dialogue_answers (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Node_ID INTEGER, Answer_text TEXT, To_node INTEGER, End_dialogue BOOLEAN, Npc_id INTEGER REFERENCES npc (id), Start_quest INTEGER DEFAULT (0), End_quest INTEGER DEFAULT (0), Quest_ID INTEGER DEFAULT (0));
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;