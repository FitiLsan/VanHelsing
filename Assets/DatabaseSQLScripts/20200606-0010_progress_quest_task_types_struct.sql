--
-- Файл сгенерирован с помощью SQLiteStudio v3.2.1 в Пт июн 5 23:59:56 2020
--
-- Использованная кодировка текста: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Таблица: quest_task_types
DROP TABLE IF EXISTS quest_task_types;
CREATE TABLE quest_task_types (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, type STRING);
INSERT INTO quest_task_types (id, type) VALUES (0, '');
INSERT INTO quest_task_types (id, type) VALUES (1, 'KillNpc');
INSERT INTO quest_task_types (id, type) VALUES (2, 'CollectItem');
INSERT INTO quest_task_types (id, type) VALUES (3, 'TalkWithNpc');
INSERT INTO quest_task_types (id, type) VALUES (4, 'UseObject');
INSERT INTO quest_task_types (id, type) VALUES (5, 'FindLocation');
INSERT INTO quest_task_types (id, type) VALUES (6, 'KillEnemyFamily');
INSERT INTO quest_task_types (id, type) VALUES (7, 'UseItem');
INSERT INTO quest_task_types (id, type) VALUES (8, 'SelectAnswer');

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
