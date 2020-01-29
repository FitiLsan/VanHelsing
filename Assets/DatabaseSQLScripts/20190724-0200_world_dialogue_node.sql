--
-- Файл сгенерирован с помощью SQLiteStudio v3.2.1 в Ср июл 24 02:00:56 2019
--
-- Использованная кодировка текста: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Таблица: dialogue_node
DROP TABLE IF EXISTS dialogue_node;
CREATE TABLE dialogue_node (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Npc_id INTEGER REFERENCES npc (id), Node_ID INTEGER, Npc_text TEXT);
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (1, 1, 0, 'Npc 1 text 1');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (2, 1, 1, 'Npc 1 text 2');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (3, 1, 2, 'Npc 1 text 3');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (4, 1, 3, 'Npc 1 text 4');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (5, 2, 0, 'Привет, я тестовый NPC 2, как продвигается твоя разработка?');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (6, 2, 1, 'Это замечательно, продолжай в том же духе!');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (7, 2, 2, 'Мои дела...да собственно тоже хорошо...стою тут...наблюдаю.');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (8, 2, 3, 'Я, первый NPC, с которым ты можешь тут поговорить, не узнал?');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (9, 2, 4, 'Очень жаль, может быть позже ты вернешься...');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (10, 2, 5, 'Ну и славно. Позже поговорим.');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (11, 2, 6, 'Ну, горы вон там красивые, домики тут всякие...ничего особенного.');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (12, 2, 7, 'Это верно! Удачи тебе.');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (13, 2, 8, NULL);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
