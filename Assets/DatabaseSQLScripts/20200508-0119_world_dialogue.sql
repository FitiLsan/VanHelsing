insert into 'dialogue_answers' (Npc_id, Node_ID, Answer_text, To_node, End_dialogue) values ('6','0','answer','0','0'); 
update 'dialogue_node' SET Npc_text ='Какие наши дальнейшие действия? Нам нужно восстановить силы, но все припасы пропали.' where Npc_id = 6 and Node_id = 0; 
update 'dialogue_answers' SET Answer_text='Держи крабью клешню и приготовь ка нам из нее похлебки. Нам нужно восстановить силы.', To_node ='0', Quest_ID='4', End_dialogue='1', Start_quest='0', End_quest='1', Task_quest = '0'   where Id = '26'; 
update 'dialogue_answers' SET Answer_text='Нам нужно выбираться отсюда.', To_node ='0', Quest_ID='0', End_dialogue='1', Start_quest='0', End_quest='0', Task_quest = '0'   where Id = '27'; 
update 'dialogue_answers' SET Answer_text='У меня есть идея на счет обеда.', To_node ='0', Quest_ID='4', End_dialogue='1', Start_quest='0', End_quest='0', Task_quest = '1'   where Id = '28'; 
