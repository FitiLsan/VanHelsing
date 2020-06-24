insert into 'quest' (StartDialogId, EndDialogId, StartQuestEventType, EndQuestEventType) values ('0','0','0','0') ; 
insert into 'quest_locale_ru' (QuestId) values ('5'); 
update 'quest_locale_ru' SET Title = 'Охота на кролей', Description = 'Нужно помочь достать 3 пары кроличих ушей, они обитают на поляне неподалеку.
Интересно зачем они ему?' where QuestId = '5'; 
update 'quest' SET StartDialogId = '31', EndDialogId = '32', StartQuestEventType = '1', EndQuestEventType = '1' where Id = '5'; 
insert into 'quest_objectives' (QuestId) values ('5') ; 
insert into 'quest_objectives_locale_ru' (ObjectiveId) values ('6'); 
update 'quest_locale_ru' SET Title = 'Охота на кролей', Description = 'Нужно помочь достать 3 пары кроличих ушей, они обитают на поляне неподалеку.
Интересно зачем они ему?' where QuestId = '5'; 
update 'quest' SET StartDialogId = '31', EndDialogId = '32', StartQuestEventType = '1', EndQuestEventType = '1' where Id = '5'; 
update 'quest_objectives' SET Type='1', TargetId='9', Amount='3',isOptional ='0' where QuestId='5' and Id ='6'; 
update 'quest_objectives_locale_ru' SET Title='Убить кролика' where ObjectiveId = '6'; 
insert into 'quest_objectives' (QuestId) values ('5') ; 
insert into 'quest_objectives_locale_ru' (ObjectiveId) values ('7'); 
update 'quest_locale_ru' SET Title = 'Охота на кролей', Description = 'Нужно помочь достать 3 пары кроличих ушей, они обитают на поляне неподалеку.
Интересно зачем они ему?' where QuestId = '5'; 
update 'quest' SET StartDialogId = '31', EndDialogId = '34', StartQuestEventType = '1', EndQuestEventType = '1' where Id = '5'; 
update 'quest_objectives' SET Type='1', TargetId='9', Amount='3',isOptional ='0' where QuestId='5' and Id ='6'; 
update 'quest_objectives_locale_ru' SET Title='Убить кролика' where ObjectiveId = '6'; 
update 'quest_objectives' SET Type='8', TargetId='32', Amount='1',isOptional ='0' where QuestId='5' and Id ='7'; 
update 'quest_objectives_locale_ru' SET Title='������' where ObjectiveId = '7'; 
update 'quest_locale_ru' SET Title = 'Новости', Description = 'Поговорить с NPC неподалеку и узнать как обстоят дела.' where QuestId = '1'; 
update 'quest' SET StartDialogId = '3', EndDialogId = '9', StartQuestEventType = '1', EndQuestEventType = '1' where Id = '1'; 
update 'quest_objectives' SET Type='8', TargetId='4', Amount='1',isOptional ='0' where QuestId='1' and Id ='1'; 
update 'quest_objectives_locale_ru' SET Title='Разговор с NPC' where ObjectiveId = '1'; 
update 'quest_locale_ru' SET Title = 'Новости ч.2', Description = 'Нужно узнать больше подробностей о случившемся' where QuestId = '2'; 
update 'quest' SET StartDialogId = '11', EndDialogId = '17', StartQuestEventType = '1', EndQuestEventType = '1' where Id = '2'; 
update 'quest_objectives' SET Type='8', TargetId='13', Amount='1',isOptional ='0' where QuestId='2' and Id ='2'; 
update 'quest_objectives_locale_ru' SET Title='Разговор с NPC' where ObjectiveId = '2'; 
update 'quest_locale_ru' SET Title = 'Новости ч.3', Description = 'Остался еще один, может быть он нам сможет рассказать еще что-нибудь? Надо узнать.' where QuestId = '3'; 
update 'quest' SET StartDialogId = '19', EndDialogId = '21', StartQuestEventType = '1', EndQuestEventType = '1' where Id = '3'; 
update 'quest_objectives' SET Type='8', TargetId='20', Amount='3',isOptional ='0' where QuestId='3' and Id ='3'; 
update 'quest_objectives_locale_ru' SET Title='Разговор с NPC' where ObjectiveId = '3'; 
