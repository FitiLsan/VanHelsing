--
-- Файл сгенерирован с помощью SQLiteStudio v3.2.1 в Ср июл 24 00:19:15 2019
--
-- Использованная кодировка текста: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;
DROP TABLE dialogue_answers;
-- Таблица: dialogue_answers
CREATE TABLE dialogue_answers (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Node_ID INTEGER, Answer_text TEXT, To_node INTEGER, End_dialogue BOOLEAN, Npc_id INTEGER REFERENCES npc (id));
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (1, 0, 'GG answer text 0:1', 1, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (2, 0, 'GG answer text 0:2', 2, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (3, 0, 'GG answer text 0:3', 3, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (4, 1, 'GG answer text 1:1', 0, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (5, 1, 'GG answer text 1:2', 0, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (6, 1, 'GG answer text 1:3', 0, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (7, 1, 'GG answer text 1:4', 0, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (8, 2, 'GG answer text 2:1', 0, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (9, 2, 'GG answer text 2:2', 0, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (10, 3, 'GG answer text 3:1', 0, 1, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (11, 0, 'Привет, все хорошо.', 1, 0, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (12, 0, 'Нормально, а как твои дела?', 2, 0, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (13, 0, 'Ты кто такой и почему меня об этом спрашиваешь?', 3, 0, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (14, 0, 'Не хочу с тобой разговаривать, пока.', 4, 0, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (15, 1, 'Ага!', 0, 1, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (16, 3, 'Нет, не узнал. Странный ты, я пошел.', 0, 1, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (17, 3, 'Узнал. Кажется что-то такое припоминаю.', 5, 0, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (18, 4, 'Может быть, я подумаю над этим.', 0, 1, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (19, 2, 'И что видно?', 6, 0, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (20, 2, 'Тут пока-что не особо есть что посмотреть, работаем над этим.', 7, 0, 2);

-- Таблица: dialogue_node
DROP TABLE dialogue_node;
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
