--
-- Файл сгенерирован с помощью SQLiteStudio v3.2.1 в Ср июл 10 19:52:01 2019
--
-- Использованная кодировка текста: System
--
PRAGMA foreign_keys = off;

-- Таблица: dialogue_node
CREATE TABLE dialogue_node (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Node_ID INTEGER, Npc_id INTEGER, Npc_text TEXT);
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text) VALUES (1, 1, 1, 'Npc 1 text 1');
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text) VALUES (2, 2, 1, 'Npc 1 text 2');
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text) VALUES (3, 3, 1, 'Npc 1 text 3');
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text) VALUES (4, 4, 1, 'Npc 1 text 4');
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text) VALUES (5, 2, 2, 'Привет, я тестовый NPC 2, как продвигается твоя разработка?');
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text) VALUES (6, 3, 2, 'Это замечательно, продолжай в том же духе!');
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text) VALUES (7, 4, 2, 'Мои дела...да собственно тоже хорошо...стою тут...наблюдаю.');
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text) VALUES (8, 5, 2, 'Я, первый NPC, с которым ты можешь тут поговорить, не узнал?');
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text) VALUES (9, 6, 2, 'Очень жаль, может быть позже ты вернешься...');
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text) VALUES (10, 7, 2, 'Ну и славно. Позже поговорим.');
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text) VALUES (11, 8, 2, 'Ну, горы вон там красивые, домики тут всякие...ничего особенного.');
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text) VALUES (12, 9, 2, 'Это верно! Удачи тебе.');
INSERT INTO dialogue_node (Id, Node_ID, Npc_id, Npc_text) VALUES (13, 10, 2, NULL);

PRAGMA foreign_keys = on;
