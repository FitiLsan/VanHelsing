--
-- Файл сгенерирован с помощью SQLiteStudio v3.2.1 в Ср июл 24 00:20:00 2019
--
PRAGMA foreign_keys = off;

INSERT INTO quest (MinLevel, QuestLevel, TimeAllowed, ZoneId, RewardExp, RewardMoney, StartDialogId, EndDialogId) VALUES (1, 1, -1, 1, 0, 0, 10, 20);

INSERT INTO quest_locale_ru (QuestId, Title, Description) VALUES (1, 'Тест квест', 'Тестовый квест номер 1.');


INSERT INTO quest_objectives (QuestId, Type, TargetId, Amount) VALUES (1, 3, 1, 1);

INSERT INTO quest_objectives_locale_ru (ObjectiveId, Title) VALUES ( 1, 'Тест 111');

PRAGMA foreign_keys = on;
