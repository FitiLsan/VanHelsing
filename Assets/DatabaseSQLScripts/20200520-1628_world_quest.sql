update 'quest_locale_ru' SET Title = 'Помощь раненым', Description = 'После крушения воины получили ранения, им нужно помочь восстановить силы.' where QuestId = '5'; 
update 'quest' SET StartDialogId = '32', EndDialogId = '30', StartQuestEventType = '1', EndQuestEventType = '1' where Id = '5'; 
update 'quest_objectives' SET Type='8', TargetId='35', Amount='1',isOptional ='0' where QuestId='5' and Id ='6'; 
update 'quest_objectives_locale_ru' SET Title='Отдать лекарство NPC 1' where ObjectiveId = '6'; 
update 'quest_objectives' SET Type='8', TargetId='37', Amount='1',isOptional ='1' where QuestId='5' and Id ='7'; 
update 'quest_objectives_locale_ru' SET Title='Отдать лекарство NPC 2' where ObjectiveId = '7'; 
update 'quest_objectives' SET Type='8', TargetId='39', Amount='1',isOptional ='1' where QuestId='5' and Id ='8'; 
update 'quest_objectives_locale_ru' SET Title='Отдать лекарство NPC 3' where ObjectiveId = '8'; 
