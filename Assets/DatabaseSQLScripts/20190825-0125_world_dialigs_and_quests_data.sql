--
-- Файл сгенерирован с помощью SQLiteStudio v3.2.1 в Вс авг 25 01:27:47 2019
--
-- Использованная кодировка текста: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Таблица: dialogue_answers
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (1, 0, 'Я рыцарь! Бойся меня.', 1, 0, 1, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (2, 0, 'Да так никто, мимо проходил, а что?', 2, 0, 1, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (3, 0, 'Это не имеет значения, какое тебе дело до меня?', 3, 0, 1, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (4, 1, 'Вполне серьезно, не видно что-ли?', 0, 1, 1, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (5, 1, 'Успакойся, это шутка. Ты что-то хотел?', 2, 0, 1, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (6, 4, 'Хорошо, я обязательно передам ему привет.', 0, 1, 1, 1, 0, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (7, 4, 'Знаешь, я передумал. Пока.', 0, 1, 1, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (8, 2, 'Нет, не хочу.', 0, 1, 1, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (9, 2, 'Да, я помогу тебе, какое дело?', 4, 0, 1, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (10, 3, 'Ну тогда пока.', 0, 1, 1, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (11, 0, 'Привет, все хорошо.', 1, 0, 2, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (12, 0, 'Нормально, а как твои дела?', 2, 0, 2, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (13, 0, 'Ты кто такой и почему меня об этом спрашиваешь?', 3, 0, 2, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (14, 0, 'Не хочу с тобой разговаривать, пока.', 4, 0, 2, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (15, 1, 'Ага!', 0, 1, 2, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (16, 3, 'Нет, не узнал. Странный ты, я пошел.', 0, 1, 2, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (17, 3, 'Узнал. Кажется что-то такое припоминаю.', 5, 0, 2, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (18, 4, 'Может быть, я подумаю над этим.', 0, 1, 2, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (19, 2, 'И что видно?', 6, 0, 2, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (20, 2, 'Тут пока-что не особо есть что посмотреть, работаем над этим.', 7, 0, 2, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (21, 0, 'Привет, да вот мимо проходил.', 1, 0, 3, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (22, 0, 'Ой, забыл...пока.', 0, 1, 3, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (23, 1, 'Меня отправил к тебе твой братец! Он просит передать тебе привет.', 2, 0, 3, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (24, 2, 'Хорошо, обязательно передам.', 0, 1, 3, 0, 1, 1);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (25, 0, 'О, привет тебе!', 1, 0, 4, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (26, 0, 'Зачем ты наблюдаешь?', 2, 0, 4, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (27, 1, 'Нет? Почему нет?', 4, 0, 4, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (28, 4, 'Странный ты.', 0, 1, 4, 0, 0, 0);
INSERT INTO dialogue_answers (Id, Node_ID, Answer_text, To_node, End_dialogue, Npc_id, Start_quest, End_quest, Quest_ID) VALUES (29, 2, 'Но почему?', 4, 0, 4, 0, 0, 0);

-- Таблица: dialogue_node
DELETE FROM dialogue_node;
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (1, 1, 0, 'Привет, я тестовый NPC 1, а кто ты?');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (2, 1, 1, 'Уууу....как страшно...ты серьезно?');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (3, 1, 2, 'У меня есть дело к тебе, поможешь?');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (4, 1, 3, 'Да собственно никакого...');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (5, 2, 0, 'Привет, я тестовый NPC 2, как продвигается твоя разработка?');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (6, 2, 1, 'Это замечательно, продолжай в том же духе!');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (7, 2, 2, 'Мои дела...да собственно тоже хорошо...стою тут...наблюдаю.');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (8, 2, 3, 'Я, первый NPC, с которым ты можешь тут поговорить, не узнал?');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (9, 2, 4, 'Очень жаль, может быть позже ты вернешься...');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (10, 2, 5, 'Ну и славно. Позже поговорим.');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (11, 2, 6, 'Ну, горы вон там красивые, домики тут всякие...ничего особенного.');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (12, 2, 7, 'Это верно! Удачи тебе.');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (13, 2, 8, NULL);
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (14, 1, 4, 'У дома напротив стоит мой брат близнец. Можешь сходить к нему и передать привет от меня? Я бы и сам сходил, но к сожалению функционал для моего передвижения пока-что отсуствувет. Заодно загляни к моему другу, который стоит у колодца и узнай как у него дела! Хорошо?');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (15, 3, 0, 'Здрасьте. Какими судьбами?');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (16, 3, 1, 'Где был? Что видел?');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (17, 3, 2, 'Ооо... круто! Спасибо. Увидешь его еще раз, передавай и мой привет. Удачи.');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (18, 4, 0, 'Я тестовый NPC 4, давно наблюдаю за тобой...');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (19, 4, 1, 'Привет? Ну привет...хотя пожалуй нет.');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (20, 4, 2, 'Я отказываюсь отвечать на данный вопрос.');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (21, 4, 3, 'Пока? Пока.');
INSERT INTO dialogue_node (Id, Npc_id, Node_ID, Npc_text) VALUES (22, 4, 4, '...');

-- Таблица: npc
INSERT INTO npc (id, npc_name) VALUES (1, 'Test NPC 1');
INSERT INTO npc (id, npc_name) VALUES (2, 'Test NPC 2');
INSERT INTO npc (id, npc_name) VALUES (3, 'Test NPC 3');
INSERT INTO npc (id, npc_name) VALUES (4, 'Test NPC 4');

-- Таблица: quest
DELETE FROM quest;
INSERT INTO quest (Id, MinLevel, QuestLevel, TimeAllowed, ZoneId, RewardExp, RewardMoney, StartDialogId, EndDialogId) VALUES (1, 1, 1, -1, 1, 0, 0, 6, 22);
INSERT INTO quest (Id, MinLevel, QuestLevel, TimeAllowed, ZoneId, RewardExp, RewardMoney, StartDialogId, EndDialogId) VALUES (2, 1, 1, -1, 1, 0, 0, 10, 20);

-- Таблица: quest_objectives
DELETE FROM quest_objectives;
INSERT INTO quest_objectives (Id, QuestId, Type, TargetId, Amount, IsOptional) VALUES (1, 1, 8, 1, 1, 0);
INSERT INTO quest_objectives (Id, QuestId, Type, TargetId, Amount, IsOptional) VALUES (2, 1, 8, 12, 1, 0);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
