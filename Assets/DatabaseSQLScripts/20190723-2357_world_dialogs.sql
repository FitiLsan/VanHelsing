-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-22 00:06:50
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_node;
DROP TABLE dialogue_node;
CREATE TABLE dialogue_node (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Npc_id INTEGER, Npc_text TEXT);
INSERT INTO dialogue_node (Id, Npc_id, Npc_text) SELECT Id, Npc_id, Npc_text FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-22 00:11:29
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_node;
DROP TABLE dialogue_node;
CREATE TABLE dialogue_node (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Npc_id INTEGER, Node_ID INTEGER, Npc_text TEXT);
INSERT INTO dialogue_node (Id, Npc_id, Npc_text) SELECT Id, Npc_id, Npc_text FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;