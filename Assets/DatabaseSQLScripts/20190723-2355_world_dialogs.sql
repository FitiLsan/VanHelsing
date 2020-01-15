-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:01:44
CREATE TABLE dialogue_locale_ru (Id INTEGER PRIMARY KEY AUTOINCREMENT);

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:02:09
PRAGMA foreign_keys = 0;
CREATE TABLE dialogue_answers_locale_ru (Id INTEGER PRIMARY KEY AUTOINCREMENT);
INSERT INTO dialogue_answers_locale_ru (Id) SELECT Id FROM dialogue_locale_ru;
DROP TABLE dialogue_locale_ru;
PRAGMA foreign_keys = 1;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:02:35
CREATE TABLE dialogue_node_locale_ru (Id INTEGER PRIMARY KEY AUTOINCREMENT);

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:04:53
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_answers_locale_ru;
DROP TABLE dialogue_answers_locale_ru;
CREATE TABLE dialogue_answers_locale_ru (Id INTEGER PRIMARY KEY AUTOINCREMENT, answer_text TEXT);
INSERT INTO dialogue_answers_locale_ru (Id) SELECT Id FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:05:33
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_node_locale_ru;
DROP TABLE dialogue_node_locale_ru;
CREATE TABLE dialogue_node_locale_ru (Id INTEGER PRIMARY KEY AUTOINCREMENT, npc_text TEXT);
INSERT INTO dialogue_node_locale_ru (Id) SELECT Id FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:14:08
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_answers_locale_ru;
DROP TABLE dialogue_answers_locale_ru;
CREATE TABLE dialogue_answers_locale_ru (Id INTEGER PRIMARY KEY AUTOINCREMENT);
INSERT INTO dialogue_answers_locale_ru (Id) SELECT Id FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:14:22
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_answers_locale_ru;
DROP TABLE dialogue_answers_locale_ru;
CREATE TABLE dialogue_answers_locale_ru (Id INTEGER PRIMARY KEY AUTOINCREMENT, answer_text TEXT);
INSERT INTO dialogue_answers_locale_ru (Id) SELECT Id FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:16:13
DROP TABLE main.dialogue_answers_locale_ru;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:17:13
CREATE TABLE dialogue_answers_locale_ru (Id INTEGER PRIMARY KEY AUTOINCREMENT, answer_text TEXT);

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:23:35
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_answers;
DROP TABLE dialogue_answers;
CREATE TABLE dialogue_answers (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Node_ID INTEGER, Answer_text_id INTEGER REFERENCES dialogue_answers_locale_ru (answer_text), To_node INTEGER, End_dialogue BOOLEAN, Npc_id INTEGER);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text_id, To_node, End_dialogue, Npc_id) SELECT Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:26:29
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_node;
DROP TABLE dialogue_node;
CREATE TABLE dialogue_node (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Node_ID INTEGER, Npc_id INTEGER, Npc_text_id INTEGER REFERENCES dialogue_node_locale_ru (npc_text));
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text_id) SELECT Id, Node_ID, Npc_id, Npc_text FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:28:45
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_node;
DROP TABLE dialogue_node;
CREATE TABLE dialogue_node (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Node_ID INTEGER, Npc_id INTEGER, Npc_text_id INTEGER REFERENCES dialogue_node_locale_ru (Id));
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text_id) SELECT Id, Node_ID, Npc_id, Npc_text_id FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:30:55
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_node;
DROP TABLE dialogue_node;
CREATE TABLE dialogue_node (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Node_ID INTEGER, Npc_id INTEGER, Npc_text_id INTEGER);
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text_id) SELECT Id, Node_ID, Npc_id, Npc_text_id FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:32:34
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_answers_locale_ru;
DROP TABLE dialogue_answers_locale_ru;
CREATE TABLE dialogue_answers_locale_ru (Id INTEGER PRIMARY KEY AUTOINCREMENT, answer_text TEXT, answer_id INTEGER REFERENCES dialogue_answers (Id));
INSERT INTO dialogue_answers_locale_ru (Id, answer_text) SELECT Id, answer_text FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:33:28
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_answers;
DROP TABLE dialogue_answers;
CREATE TABLE dialogue_answers (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Node_ID INTEGER, Answer_text_id INTEGER, To_node INTEGER, End_dialogue BOOLEAN, Npc_id INTEGER);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text_id, To_node, End_dialogue, Npc_id) SELECT Id, Node_ID, Answer_text_id, To_node, End_dialogue, Npc_id FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:34:17
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_node_locale_ru;
DROP TABLE dialogue_node_locale_ru;
CREATE TABLE dialogue_node_locale_ru (Id INTEGER PRIMARY KEY AUTOINCREMENT, npc_text TEXT, node_id INTEGER REFERENCES dialogue_node (Id));
INSERT INTO dialogue_node_locale_ru (Id, npc_text) SELECT Id, npc_text FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:35:04
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_answers;
DROP TABLE dialogue_answers;
CREATE TABLE dialogue_answers (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Node_ID INTEGER, To_node INTEGER, End_dialogue BOOLEAN, Npc_id INTEGER);
INSERT INTO dialogue_answers (Id, Node_ID, To_node, End_dialogue, Npc_id) SELECT Id, Node_ID, To_node, End_dialogue, Npc_id FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;

-- Запросы, выполненные к базе данных world (E:/Projects/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Дата и время выполнения: 2019-07-17 01:41:51
PRAGMA foreign_keys = 0;
CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM dialogue_node;
DROP TABLE dialogue_node;
CREATE TABLE dialogue_node (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Node_ID INTEGER, Npc_id INTEGER);
INSERT INTO dialogue_node (Id, Node_ID, Npc_id) SELECT Id, Node_ID, Npc_id FROM sqlitestudio_temp_table;
DROP TABLE sqlitestudio_temp_table;
PRAGMA foreign_keys = 1;