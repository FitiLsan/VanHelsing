-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 15:05:57
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_node;
DROP TABLE dialogue_node;
CREATE TABLE dialogue_node (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Node_ID INTEGER, npc_text TEXT, Npc_id INTEGER);
INSERT INTO dialogue_node (Id, Node_ID, Npc_id) SELECT Id, Node_ID, Npc_id FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 15:10:33
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_answers;
DROP TABLE dialogue_answers;
CREATE TABLE dialogue_answers (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Node_ID INTEGER, answer_text TEXT, To_node INTEGER, End_dialogue BOOLEAN, Npc_id INTEGER);
INSERT INTO dialogue_answers (Id, Node_ID, To_node, End_dialogue, Npc_id) SELECT Id, Node_ID, To_node, End_dialogue, Npc_id FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;