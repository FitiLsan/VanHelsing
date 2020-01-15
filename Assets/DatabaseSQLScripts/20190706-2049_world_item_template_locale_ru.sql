-- Queries executed on database world (D:/Dev/RPG_Van_Helsing/Assets/StreamingAssets/world.bytes)
-- Date and time of execution: 2019-07-06 20:49:27
CREATE TABLE item_template_locale_ru (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ItemId REFERENCES item_template (Id) ON DELETE CASCADE NOT NULL, Title TEXT NOT NULL DEFAULT item, FlavorText TEXT);