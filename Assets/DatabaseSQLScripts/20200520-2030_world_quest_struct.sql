--
-- Файл сгенерирован с помощью SQLiteStudio v3.2.1 в Ср май 20 20:39:41 2020
--
-- Использованная кодировка текста: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Таблица: quest
DROP TABLE IF EXISTS quest;
CREATE TABLE quest (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, MinLevel INTEGER DEFAULT (1) NOT NULL, QuestLevel INTEGER DEFAULT (1) NOT NULL, TimeAllowed INTEGER DEFAULT (- 1) NOT NULL, ZoneId INTEGER DEFAULT (1) NOT NULL, RewardExp INTEGER NOT NULL DEFAULT (0), RewardMoney INTEGER NOT NULL DEFAULT (0), StartDialogId INTEGER NOT NULL DEFAULT (0), EndDialogId INTEGER DEFAULT (0) NOT NULL, StartQuestEventType INTEGER NOT NULL DEFAULT (0), EndQuestEventType INTEGER NOT NULL DEFAULT (0), Repeatable INTEGER DEFAULT (0) NOT NULL);
INSERT INTO quest (Id, MinLevel, QuestLevel, TimeAllowed, ZoneId, RewardExp, RewardMoney, StartDialogId, EndDialogId, StartQuestEventType, EndQuestEventType, Repeatable) VALUES (1, 1, 1, -1, 1, 0, 0, 3, 9, 1, 1, 0);
INSERT INTO quest (Id, MinLevel, QuestLevel, TimeAllowed, ZoneId, RewardExp, RewardMoney, StartDialogId, EndDialogId, StartQuestEventType, EndQuestEventType, Repeatable) VALUES (2, 1, 1, -1, 1, 0, 0, 11, 17, 1, 1, 0);
INSERT INTO quest (Id, MinLevel, QuestLevel, TimeAllowed, ZoneId, RewardExp, RewardMoney, StartDialogId, EndDialogId, StartQuestEventType, EndQuestEventType, Repeatable) VALUES (3, 1, 1, -1, 1, 0, 0, 19, 21, 1, 1, 0);
INSERT INTO quest (Id, MinLevel, QuestLevel, TimeAllowed, ZoneId, RewardExp, RewardMoney, StartDialogId, EndDialogId, StartQuestEventType, EndQuestEventType, Repeatable) VALUES (4, 1, 1, -1, 1, 0, 0, 24, 26, 1, 1, 0);
INSERT INTO quest (Id, MinLevel, QuestLevel, TimeAllowed, ZoneId, RewardExp, RewardMoney, StartDialogId, EndDialogId, StartQuestEventType, EndQuestEventType, Repeatable) VALUES (6, 1, 1, -1, 1, 0, 0, 32, 30, 1, 1, 0);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
