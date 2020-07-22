update 'dialogue_node' SET Npc_text ='*Долли бродит по полянке и ищет огнелист*
Что тебе надо?' where Npc_id = 4 and Node_id = 0; 
update 'dialogue_answers' SET Answer_text='Долли! Что ты тут делаешь? Что бы ты ни задумала, брось это, это не стоит того! ', To_node ='1', Quest_ID='1', End_dialogue='0', Start_quest='0', End_quest='0', Task_quest = '0'   where Id = '12'; 
update 'dialogue_node' SET Npc_text ='Ты не понимаешь, я люблю его! Я стану как он и мы вечно будем вместе!' where Npc_id = 4 and Node_id = 1; 
update 'dialogue_answers' SET Answer_text='Чудовищем? Ты хочешь стать монстром, который вечно пытается утопиться? Подумай об Эрике, о его дочке! Ты же хотела помочь им, они ждут тебя, беспокоятся о тебе!', To_node ='2', Quest_ID='1', End_dialogue='0', Start_quest='0', End_quest='0', Task_quest = '0'   where Id = '13'; 
update 'dialogue_node' SET Npc_text ='Его дочь и так поправится, эта горячка и сама пройдет, а кто вылечит Дориана? Кто позаботится о нем? Я должна это сделать, надо всего лишь собрать побольше огнелистов и мы станем навсегда вместе… ' where Npc_id = 4 and Node_id = 2; 
update 'dialogue_answers' SET Answer_text='Это уже не Дориан, он уже не человек и не способен на чувства, он опасен для тебя и для жителей. Вернись домой, пока не поздно, мы что-нибудь придумаем… ', To_node ='3', Quest_ID='1', End_dialogue='0', Start_quest='0', End_quest='0', Task_quest = '0'   where Id = '14'; 
update 'dialogue_node' SET Npc_text ='*Долли отворачивается и уходит к топям.*' where Npc_id = 4 and Node_id = 3; 
update 'dialogue_answers' SET Answer_text='Нужно проследить за ней!', To_node ='0', Quest_ID='1', End_dialogue='1', Start_quest='0', End_quest='1', Task_quest = '0'   where Id = '15'; 
