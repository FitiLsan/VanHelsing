insert into 'quest' (StartDialogId, EndDialogId, StartQuestEventType, EndQuestEventType) values ('0','0','0','0') ; 
insert into 'quest_locale_ru' (QuestId) values ('4'); 
update 'quest_locale_ru' SET Title = 'Вкусный обед', Description = 'Убей грязевого краба, он мешает нам пройти по ущелью.
Кстати из него можно приготовить вкусный обед.' where QuestId = '4'; 
update 'quest' SET StartDialogId = '24', EndDialogId = '26', StartQuestEventType = '1', EndQuestEventType = '1' where Id = '4'; 
insert into 'quest_objectives' (QuestId) values ('4') ; 
insert into 'quest_objectives_locale_ru' (ObjectiveId) values ('4'); 
update 'quest_locale_ru' SET Title = 'Вкусный обед', Description = 'Убей грязевого краба, он мешает нам пройти по ущелью.
Кстати из него можно приготовить вкусный обед.' where QuestId = '4'; 
update 'quest' SET StartDialogId = '24', EndDialogId = '26', StartQuestEventType = '1', EndQuestEventType = '1' where Id = '4'; 
update 'quest_objectives' SET Type='1', TargetId='7', Amount='1',isOptional ='0' where QuestId='4' and Id ='4'; 
update 'quest_objectives_locale_ru' SET Title='Убить краба' where ObjectiveId = '4'; 
