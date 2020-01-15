--
-- Файл сгенерирован с помощью SQLiteStudio v3.2.1 в Ср июл 10 19:51:45 2019
--
-- Использованная кодировка текста: System
--
PRAGMA foreign_keys = off;

-- Таблица: dialogue_answers
CREATE TABLE dialogue_answers (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Node_ID INTEGER, Answer_text TEXT, To_node INTEGER, End_dialogue BOOLEAN, Npc_id INTEGER);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (1, 1, 'GG answer text 1:1', 2, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (2, 1, 'GG answer text 1:2', 3, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (3, 1, 'GG answer text 1:3', 4, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (4, 2, 'GG answer text 2:1', 1, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (5, 2, 'GG answer text 2:2', 1, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (6, 2, 'GG answer text 2:3', 1, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (7, 2, 'GG answer text 2:4', 1, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (8, 3, 'GG answer text 3:1', 1, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (9, 3, 'GG answer text 3:2', 1, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (10, 4, 'GG answer text 4:1', 1, 1, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (11, 1, 'Привет, все хорошо.', 2, 0, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (12, 1, 'Нормально, а как твои дела?', 3, 0, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (13, 1, 'Ты кто такой и почему меня об этом спрашиваешь?', 4, 0, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (14, 1, 'Не хочу с тобой разговаривать, пока.', 5, 0, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (15, 2, 'Ага!', 0, 1, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (16, 4, 'Нет, не узнал. Странный ты, я пошел.', 0, 1, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (17, 4, 'Узнал. Кажется что-то такое припоминаю.', 6, 0, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (18, 5, 'Может быть, я подумаю над этим.', 0, 1, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (19, 3, 'И что видно?', 7, 0, 2);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id) VALUES (20, 3, 'Тут пока-что не особо есть что посмотреть, работаем над этим.', 8, 0, 2);

PRAGMA foreign_keys = on;
