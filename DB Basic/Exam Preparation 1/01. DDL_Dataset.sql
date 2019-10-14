USE Airport
GO

-- Disable referential integrity
EXEC sp_MSForEachTable 'DISABLE TRIGGER ALL ON ?'
GO

EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
GO

EXEC sp_MSForEachTable 'DELETE FROM ?';
GO

EXEC sp_MSForEachTable
    'IF OBJECT_ID(''?'') IN (SELECT OBJECT_ID FROM SYS.IDENTITY_COLUMNS)
     DBCC CHECKIDENT(''?'', RESEED, 0)';
GO

SET IDENTITY_INSERT Planes OFF;
SET IDENTITY_INSERT Flights OFF;
SET IDENTITY_INSERT LuggageTypes OFF;
SET IDENTITY_INSERT Passengers OFF;
SET IDENTITY_INSERT Luggages OFF;
SET IDENTITY_INSERT Tickets OFF;

-- Enable referential integrity
EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';
GO

EXEC sp_MSForEachTable 'ENABLE TRIGGER ALL ON ?'
GO

SET IDENTITY_INSERT [dbo].Planes ON;

INSERT INTO Planes (Id, [Name], Seats, [Range]) VALUES
(1, 'Browsedrive', 231, 3486),
 (2, 'Bubblemix', 156, 4010),
 (3, 'Divape', 263, 3223),
 (4, 'Skinte', 233, 4226),
 (5, 'Topiczoom', 136, 4088),
 (6, 'Yoveo', 247, 2966),
 (7, 'Buzzbean', 215, 4201),
 (8, 'Katz', 32, 1570),
 (9, 'Eamia', 279, 3562),
 (10, 'Zooveo', 52, 3452),
 (11, 'Zoombox', 52, 3592),
 (12, 'Linkbuzz', 230, 3164),
 (13, 'Devpulse', 323, 4262),
 (14, 'Geba', 117, 4156),
 (15, 'Buzzshare', 223, 3185),
 (16, 'Dabjam', 182, 3240),
 (17, 'Jabberbean', 56, 2134),
 (18, 'Quinu', 198, 2876),
 (19, 'Plambee', 261, 2556),
 (20, 'Tekfly', 276, 1340),
 (21, 'InnoZ', 166, 1900),
 (22, 'Roodel', 130, 3347),
 (23, 'LiveZ', 123, 2663),
 (24, 'Cogibox', 245, 3609),
 (25, 'Topiclounge', 68, 1941),
 (26, 'Lajo', 106, 2126),
 (27, 'Jabbersphere', 322, 3424),
 (28, 'Tagcat', 146, 2136),
 (29, 'Muxo', 223, 1989),
 (30, 'Bluezoom', 301, 3279),
 (31, 'Trunyx', 195, 2653),
 (32, 'Blognation', 42, 3393),
 (33, 'Avaveo', 38, 4108),
 (34, 'Twitternation', 178, 2720),
 (35, 'Skyba', 187, 2894),
 (36, 'Cogidoo', 330, 4275),
 (37, 'Divavu', 288, 2884),
 (38, 'Dynabox', 123, 3210),
 (39, 'Feedspan', 185, 1962),
 (40, 'Zoomcast', 315, 2206),
 (41, 'Bluejam', 62, 4139),
 (42, 'Youbridge', 159, 3508),
 (43, 'Feedbug', 189, 3402),
 (44, 'Skinix', 151, 3551),
 (45, 'Omba', 160, 3379),
 (46, 'Quire', 41, 1356),
 (47, 'Wordpedia', 202, 3945),
 (48, 'Feednation', 159, 2138),
 (49, 'Photofeed', 291, 3516),
 (50, 'Oozz', 278, 1721),
 (51, 'Linkbuzz', 254, 1433),
 (52, 'Mydeo', 313, 3045),
 (53, 'Fanoodle', 73, 2688),
 (54, 'Demizz', 326, 2736),
 (55, 'Agimba', 175, 3597),
 (56, 'Twimbo', 282, 2989),
 (57, 'Buzzdog', 286, 1318),
 (58, 'Ntag', 44, 1847),
 (59, 'Skiba', 38, 1236),
 (60, 'Innojam', 197, 3978),
 (61, 'Demivee', 101, 3882),
 (62, 'Youfeed', 312, 2105),
 (63, 'Mydeo', 99, 3771),
 (64, 'Meembee', 251, 3593),
 (65, 'Wikibox', 304, 3038),
 (66, 'Quimm', 129, 1632),
 (67, 'Brainlounge', 35, 4390),
 (68, 'Feedmix', 293, 2664),
 (69, 'Dabvine', 49, 2869),
 (70, 'Kwinu', 94, 3248),
 (71, 'Oba', 154, 1966),
 (72, 'Gabcube', 87, 4344),
 (73, 'Topicstorm', 64, 4484),
 (74, 'Flipbug', 313, 2439),
 (75, 'Tagpad', 218, 3296),
 (76, 'Thoughtstorm', 148, 2852),
 (77, 'Jabbersphere', 249, 3625),
 (78, 'JumpXS', 69, 2388),
 (79, 'Yabox', 317, 2927),
 (80, 'Youspan', 101, 2302),
 (81, 'Riffpedia', 163, 2202),
 (82, 'Skimia', 203, 1353),
 (83, 'Dabtype', 149, 1477),
 (84, 'Skinix', 147, 3125),
 (85, 'Vidoo', 328, 3838),
 (86, 'Yakitri', 321, 1360),
 (87, 'Trilith', 223, 4375),
 (88, 'Skidoo', 293, 2670),
 (89, 'Buzzshare', 95, 2178),
 (90, 'Quatz', 36, 3089),
 (91, 'Trudoo', 283, 2550),
 (92, 'Browsebug', 319, 3815),
 (93, 'Realpoint', 302, 3074),
 (94, 'Jabberstorm', 271, 3430),
 (95, 'Browsedrive', 99, 3324),
 (96, 'Vidoo', 214, 4162),
 (97, 'Photobug', 201, 2717),
 (98, 'Babbleopia', 207, 4143),
 (99, 'Yotz', 304, 1594),
 (100, 'Wordpedia', 120, 3974)

 SET IDENTITY_INSERT [dbo].Planes OFF;

 SET IDENTITY_INSERT [dbo].Flights ON;

 INSERT INTO Flights (Id, DepartureTime, ArrivalTime, Origin, Destination, PlaneId) VALUES 
(1, '2019-04-27 18:20:45', '2019-08-17 09:21:49', 'Kolyshley', 'Rancabolang', 13),
(2, '2019-07-12 14:25:16', '2019-09-02 06:54:12', 'Urug', 'Velykyy Burluk', 55),
(3, '2019-05-10 11:48:25', '2019-04-25 08:24:34', 'San Jose de Colinas', 'Qalaikhumb', 83),
(4, '2019-07-10 14:17:14', '2019-04-14 23:11:22', 'Ilheus', 'San Lorenzo', 12),
(5, '2019-08-28 15:05:02', '2019-08-19 17:53:06', 'Qaryatad Dais', 'Bebedahan', 71),
(6, '2019-05-19 08:55:41', '2019-05-20 15:23:41', 'Riangblolong', 'Lobez', 53),
(7, '2019-06-02 05:03:20', '2019-05-04 08:00:21', 'Gayamdesa', 'Langsa', 75),
(8, '2019-08-27 12:12:50', '2019-07-24 23:07:32', 'Kyustendil', 'Bela Vista do Paraiso', 31),
(9, '2019-07-27 10:22:46', '2019-06-24 12:14:49', 'Donja Brela', 'Xuancheng', 22),
(10, '2019-04-13 15:01:46', '2019-06-11 18:12:59', 'Qiancang', 'Catole do Rocha', 4),
(11, '2019-07-24 14:36:20', '2019-07-08 03:00:55', 'Aubagne', 'Kitahama', 17),
(12, '2019-06-12 00:38:57', '2019-09-11 09:01:38', 'Makrychori', 'Quitilipi', 83),
(13, '2019-06-30 08:57:15', '2019-07-27 04:29:17', 'Waimangura', 'Siukh', 70),
(14, '2019-04-27 08:45:59', '2019-08-27 16:42:35', 'Hinvi', 'Kipini', 7),
(15, '2019-09-21 08:11:00', '2019-08-25 16:33:59', 'Dalovice', 'Longxian Chengguanzhen', 88),
(16, '2019-04-04 00:19:10', '2019-08-12 00:35:42', 'Porto Uniao', 'Xinli', 76),
(17, '2019-09-04 17:02:55', '2019-05-08 05:12:57', 'Montauban', 'Aleksotas', 42),
(18, '2019-08-22 06:31:47', '2019-09-11 04:32:11', 'Xunzhong', 'Qaxbas', 4),
(19, '2019-05-03 21:05:54', '2019-04-08 05:18:12', 'Bandarban', 'Sondo', 29),
(20, '2019-06-12 06:17:06', '2019-05-18 11:10:58', 'Hronov', 'Melrose', 80),
(21, '2019-06-23 03:40:23', '2019-09-04 01:25:21', 'Yamazakicho-nakabirose', 'Merke', 6),
(22, '2019-05-06 13:02:34', '2019-05-27 04:22:37', 'Veracruz', 'Pulorejo', 42),
(23, '2019-06-10 16:57:52', '2019-08-10 02:58:44', 'Dingqing', 'Selenice', 49),
(24, '2019-04-22 20:49:26', '2019-07-07 23:32:57', 'Hailin', 'Kobenhavn', 63),
(25, '2019-07-21 16:22:28', '2019-05-12 19:54:08', 'Sevlievo', 'Nuga', 23),
(26, '2019-05-13 18:58:53', '2019-08-25 11:19:26', 'Zevenaar', 'Nimes', 37),
(27, '2019-05-07 23:00:26', '2019-04-11 02:04:48', 'Meghrashen', 'Daultala', 35),
(28, '2019-08-30 04:51:56', '2019-09-16 13:13:30', 'Qingxi', 'Cukanguncal', 32),
(29, '2019-05-23 07:40:10', '2019-04-08 17:33:42', 'Launceston', 'Eg-Uur', 86),
(30, '2019-09-29 21:54:19', '2019-07-05 19:56:25', 'Potulando', 'Ayn Halagim', 28),
(31, '2019-08-04 07:59:13', '2019-09-01 11:52:33', 'Malgobek', 'Klaeng', 11),
(32, '2019-08-06 09:04:28', '2019-09-09 20:06:52', 'Kyurdarmir', 'Laylay', 42),
(33, '2019-08-15 07:44:43', '2019-04-28 23:20:48', 'San Martin', 'Quinta do Anjo', 6),
(34, '2019-08-28 08:17:39', '2019-08-13 08:52:04', 'Lakshmipur', 'Daoxian', 10),
(35, '2019-07-28 09:07:52', '2019-08-14 19:56:47', 'Malasiqui', 'Bazar-Korgon', 1),
(36, '2019-08-11 05:24:28', '2019-08-14 05:02:00', 'Tajrish', 'Kynopiastes', 30),
(37, '2019-09-13 05:11:04', '2019-05-24 05:14:56', 'Opocno', 'Tyoply Stan', 8),
(38, '2019-07-02 21:38:36', '2019-09-02 16:02:10', 'Upton', 'Tonghong', 16),
(39, '2019-07-23 03:27:48', '2019-06-27 20:08:36', 'Pomerode', 'Enschede', 86),
(40, '2019-06-16 04:25:57', '2019-06-15 00:35:35', 'Xinhuang', 'Inashiki', 14),
(41, '2019-08-31 08:42:22', '2019-08-13 15:27:31', 'Samothraki', 'Carlsbad', 30),
(42, '2019-09-04 08:23:54', '2019-06-28 22:44:04', 'Lawa-an', 'Hulei', 39),
(43, '2019-05-18 21:19:47', '2019-07-05 00:08:27', 'Xiniqi', 'Sumberkemuning', 17),
(44, '2019-05-12 01:42:54', '2019-08-02 23:11:47', 'Dondon', 'Qatanah', 21),
(45, '2019-05-11 19:44:48', '2019-06-13 11:45:03', 'Ceibas', 'Shilipu', 6),
(46, '2019-08-06 22:28:16', '2019-05-27 04:28:28', 'Wolczyn', 'Sneek', 41),
(47, '2019-09-17 22:28:52', '2019-04-10 19:02:22', 'Ronov nad Doubravou', 'Daejeon', 10),
(48, '2019-06-07 10:44:20', '2019-08-15 21:44:32', 'Wulan Haye', 'Dzhalilabad', 22),
(49, '2019-09-19 01:51:44', '2019-04-17 20:34:25', 'Cabriz', 'Sukolilo', 51),
(50, '2019-05-14 14:50:20', '2019-04-24 18:14:43', 'Qiping', 'Simuay', 6),
(51, '2019-05-21 16:18:30', '2019-09-11 01:21:15', 'Alfena', 'Makariv', 47),
(52, '2019-09-02 16:41:34', '2019-04-02 01:32:38', 'Gressier', 'Daniwato', 94),
(53, '2019-06-20 20:08:40', '2019-04-17 04:09:38', 'Willemstad', 'Trollhattan', 38),
(54, '2019-09-12 18:17:05', '2019-08-20 15:55:27', 'Bosanski Novi', 'Conde', 21),
(55, '2019-05-20 10:23:53', '2019-06-23 16:34:16', 'Yonaan', 'Taganak', 10),
(56, '2019-06-12 13:20:49', '2019-05-31 14:36:43', 'Swindon', 'Gajah', 25),
(57, '2019-09-05 17:04:23', '2019-08-21 00:00:49', 'Yakutsk', 'Neresnytsya', 20),
(58, '2019-07-25 03:09:56', '2019-05-31 07:50:15', 'Dolni Libina', 'Monaghan', 68),
(59, '2019-08-17 23:26:25', '2019-06-15 17:58:38', 'Mitu', 'Vilar do Monte', 21),
(60, '2019-04-04 04:31:43', '2019-06-13 21:01:43', 'Ebene', 'Calinaoan Malasin', 78),
(61, '2019-06-09 12:59:37', '2019-06-10 05:04:52', 'Daqiao', 'Cangyou', 21),
(62, '2019-05-30 09:02:19', '2019-08-30 05:33:23', 'Ganyesa', 'Zhuyang', 6),
(63, '2019-09-23 23:57:29', '2019-08-28 10:31:48', 'Komsomolsk-on-Amur', 'Jequitinhonha', 41),
(64, '2019-05-03 08:31:27', '2019-09-25 19:38:59', 'Gitega', 'Kladanj', 72),
(65, '2019-05-24 11:58:16', '2019-07-10 16:51:29', 'Schiedam postbusnummers', 'Alaminos', 91),
(66, '2019-08-27 22:58:37', '2019-06-04 05:28:46', 'Adirejo', 'Koblain', 88),
(67, '2019-04-25 21:20:56', '2019-05-31 02:15:42', 'Codrington', 'Kasiyan', 76),
(68, '2019-09-07 11:32:57', '2019-05-02 13:29:21', 'Novocherkassk', 'Mamedkala', 67),
(69, '2019-09-03 12:54:27', '2019-05-25 15:31:58', 'Kadoma', 'Jianqiao', 57),
(70, '2019-08-01 12:38:03', '2019-06-01 17:17:51', 'Tlumacov', 'Perivolia', 1),
(71, '2019-06-19 10:06:48', '2019-04-14 00:26:42', 'Santa Barbara d''Oeste', 'Vingaker', 16),
(72, '2019-06-29 06:49:56', '2019-04-23 04:50:14', 'Ryglice', 'Rathdrum', 86),
(73, '2019-05-23 08:20:00', '2019-06-07 13:17:58', 'Narsaq', 'Carcarana', 23),
(74, '2019-06-09 17:51:40', '2019-04-27 22:13:09', 'Piripiri', 'Yashan', 54),
(75, '2019-04-16 09:16:47', '2019-06-18 07:35:18', 'Kudang', 'Vand?uvre-les-Nancy', 65),
(76, '2019-04-26 20:49:48', '2019-09-23 04:43:07', 'Zalec', 'Okawa', 35),
(77, '2019-07-06 15:29:10', '2019-05-03 12:38:57', 'Shibli', 'Al Qutayfah', 92),
(78, '2019-05-30 23:59:37', '2019-04-23 21:20:01', 'Vitebsk', 'Infantas', 60),
(79, '2019-09-28 22:12:01', '2019-08-14 05:40:26', 'Smarde', 'Talun', 17),
(80, '2019-04-30 19:46:48', '2019-09-17 15:48:40', 'Puerto Aisan', 'Jiaqiao', 49),
(81, '2019-05-03 00:44:08', '2019-09-14 15:03:48', 'Lapaz', 'Tessaoua', 69),
(82, '2019-04-19 04:26:09', '2019-08-02 20:27:55', 'Xiangyangpu', 'Tlumacov', 92),
(83, '2019-09-17 11:45:46', '2019-09-17 13:07:08', 'Madrid', 'Kiten', 38),
(84, '2019-06-23 02:31:56', '2019-09-03 19:57:00', 'Kauswagan', 'Shaogongzhuang', 22),
(85, '2019-04-02 02:46:19', '2019-05-06 11:19:23', 'Cileuya', 'Shenshu', 9),
(86, '2019-09-03 05:00:43', '2019-04-20 18:01:45', 'Neo Psychiko', 'Biratnagar', 62),
(87, '2019-05-10 09:12:14', '2019-08-04 02:35:27', 'Remolino', 'Xiaolongmen', 40),
(88, '2019-04-03 17:40:02', '2019-06-23 17:05:56', 'Yashiga', 'Gulao', 51),
(89, '2019-05-07 03:04:07', '2019-06-09 15:15:57', 'Shangcai Zhongxing', 'Maredakalada', 73),
(90, '2019-08-18 10:32:31', '2019-06-16 03:51:43', 'Nizhniy Novgorod', 'Portarlington', 46),
(91, '2019-08-22 00:47:08', '2019-04-30 20:26:20', 'Awilega', 'Baruunsuu', 61),
(92, '2019-09-03 08:02:44', '2019-05-17 19:56:13', 'Boto', 'Pantubig', 52),
(93, '2019-09-01 15:04:50', '2019-05-25 03:45:07', 'Nishinoomote', 'Haicheng', 52),
(94, '2019-04-22 16:34:25', '2019-06-05 02:23:42', 'Fencheng', 'Gournes', 84),
(95, '2019-04-29 16:21:32', '2019-09-15 17:24:56', 'Usagara', 'Ikhtiman', 98),
(96, '2019-05-28 00:42:20', '2019-06-16 18:42:58', 'Abelheira', 'Sabanitas', 41),
(97, '2019-04-24 22:00:57', '2019-04-23 14:29:33', 'Mendes', 'Butwal', 99),
(98, '2019-06-27 12:46:05', '2019-05-26 22:27:19', 'Le Mans', 'Grazhdanka', 48),
(99, '2019-05-06 03:25:17', '2019-08-25 08:14:52', 'Dawang', 'Uchkulan', 17),
(100, '2019-07-25 19:34:29', '2019-08-27 00:39:09', 'Independencia', 'Luanda', 61)

 SET IDENTITY_INSERT [dbo].Flights OFF;

 SET IDENTITY_INSERT [dbo].Passengers ON;

INSERT INTO Passengers (Id, FirstName, LastName, Age, Address, PassportId) VALUES 
(1, 'Marylinda', 'Landrieu', 69, '3 Bobwhite Junction', '847-03-2587'),
(2, 'Wain', 'Fryett', 90, '1884 Cascade Center', '530-04-5642'),
(3, 'Bren', 'Voase', 37, '7446 Bonner Plaza', '675-99-0906'),
(4, 'Averil', 'Primrose', 82, '6 Bartelt Place', '609-23-0744'),
(5, 'Ashlee', 'Heinl', 90, '233 Algoma Avenue', '538-39-4454'),
(6, 'Siffre', 'Kleinbaum', 25, '0 Tomscot Alley', '184-69-4160'),
(7, 'Andee', 'Ferebee', 41, '8481 Darwin Lane', '710-72-6633'),
(8, 'Layla', 'Rosekilly', 57, '647 Summit Terrace', '776-86-2758'),
(9, 'Ruprecht', 'Storre', 48, '3077 Katie Street', '857-53-8901'),
(10, 'Jude', 'Braunstein', 43, '23224 3rd Plaza', '681-42-1415'),
(11, 'Adoree', 'MacTague', 38, '97 Rusk Lane', '720-74-1315'),
(12, 'Jillana', 'Potkins', 62, '68161 Ilene Alley', '235-94-0730'),
(13, 'Garrek', 'Jacobsz', 41, '1 Parkside Park', '661-07-7522'),
(14, 'Rania', 'Sneesby', 36, '06 Heffernan Court', '277-50-5749'),
(15, 'Wally', 'Kneller', 42, '65 Kropf Parkway', '105-23-1592'),
(16, 'Adina', 'Uvedale', 77, '033 Londonderry Lane', '292-15-5340'),
(17, 'Roana', 'Gosselin', 78, '0731 Swallow Way', '409-30-9967'),
(18, 'Ashley', 'Peterkin', 43, '3713 2nd Crossing', '301-79-8886'),
(19, 'Doralin', 'Brabin', 42, '162 Lukken Junction', '237-77-3955'),
(20, 'Kennedy', 'Babar', 34, '5 5th Alley', '284-76-2618'),
(21, 'Gaven', 'Hansmann', 82, '76664 Glacier Hill Street','555-555-555'), 
(22, 'Marshall', 'Rontree', 24, '4 Londonderry Plaza', '756-95-2093'),
(23, 'Sybille', 'Dagon', 65, '0 Cambridge Point', '577-19-0195'),
(24, 'Sidonnie', 'Manifold', 16, '98 Holy Cross Alley', '146-24-0913'),
(25, 'Giacobo', 'Pucknell', 16, '403 Di Loreto Alley', '371-68-7682'),
(26, 'Adolphe', 'Juste', 81, '4419 Lawn Circle', '410-76-3620'),
(27, 'Conrad', 'Tire', 33, '07931 Prairieview Street', '708-99-9676'),
(28, 'Fransisco', 'Milward', 19, '3123 Lyons Pass', '207-50-4375'),
(29, 'Joan', 'Ballach', 50, '00594 Center Plaza', '173-64-9232'),
(30, 'Olly', 'Wastie', 79, '7635 Green Ridge Lane', '670-66-1300'),
(31, 'Darrelle', 'Ogles', 75, '22683 Sullivan Pass', '852-58-4767'),
(32, 'Hanny', 'Bruineman', 84, '15451 Havey Parkway', '199-90-6837'),
(33, 'Serene', 'Andriuzzi', 58, '4 Northridge Point', '515-84-0143'),
(34, 'Felipa', 'Wabe', 89, '096 Canary Lane', '561-21-4214'),
(35, 'Cort', 'Petzolt', 84, '5897 Sachtjen Way', '520-06-3576'),
(36, 'Bekki', 'Ethridge', 26, '09683 Memorial Street', '273-63-6838'),
(37, 'Justinian', 'O''Rowane', 66, '00 Mockingbird Drive','123-44-888'), 
(38, 'Dorita', 'Malek', 47, '4 Corry Drive', '545-87-7011'),
(39, 'Rudyard', 'Kaveney', 47, '66260 Amoth Point', '639-39-2544'),
(40, 'Brantley', 'Axby', 32, '6516 Chinook Terrace', '409-77-0820'),
(41, 'Thaxter', 'Gyngyll', 53, '3548 Pleasure Avenue', '257-10-8498'),
(42, 'Quent', 'Tumilty', 53, '28 Corscot Crossing', '851-56-0968'),
(43, 'Elmira', 'Esom', 82, '0186 Westerfield Way', '737-12-9195'),
(44, 'Glyn', 'Holworth', 37, '8 Judy Center', '744-15-2566'),
(45, 'Cassandry', 'Zink', 25, '87 Hayes Way', '323-67-3235'),
(46, 'Maddy', 'Bisgrove', 60, '53 Bluestem Point', '104-22-1592'),
(47, 'Hank', 'Nannizzi', 69, '0 Tennessee Place', '807-96-9213'),
(48, 'Andris', 'Woolliams', 18, '338 Badeau Lane', '712-45-1778'),
(49, 'Nikola', 'Guiet', 29, '23234 Comanche Hill', '534-49-8241'),
(50, 'Cassandry', 'Ollander', 19, '04293 Messerschmidt Way','123-44-555'), 
(51, 'Humberto', 'Croley', 47, '263 Washington Parkway', '537-40-9304'),
(52, 'Hermina', 'Edkins', 17, '5 Eagle Crest Center', '249-20-7940'),
(53, 'Basil', 'Pannaman', 68, '71 Artisan Hill', '263-95-5882'),
(54, 'Kizzie', 'Learman', 16, '5 American Ash Terrace', '890-62-6415'),
(55, 'Eleen', 'Ummfrey', 86, '01 Sheridan Crossing', '260-49-9211'),
(56, 'Gabie', 'Aujean', 86, '858 Sullivan Lane', '449-66-6433'),
(57, 'Christopher', 'Pimer', 71, '52 Manufacturers Circle','331-44-555'), 
(58, 'Tarra', 'Francom', 76, '5889 Maywood Circle', '677-47-2642'),
(59, 'Chan', 'Klambt', 18, '68 Vera Park', '253-90-8517'),
(60, 'Harman', 'Camellini', 73, '992 Fairfield Way', '398-48-4209'),
(61, 'Allis', 'Borley', 57, '2640 Vidon Alley', '479-46-7096'),
(62, 'Osbert', 'Veregan', 28, '2764 Ramsey Street', '434-66-9113'),
(63, 'Michele', 'Collingdon', 41, '43804 Merrick Park', '683-90-9154'),
(64, 'Cassie', 'Spurier', 42, '02 Lyons Street', '838-60-9743'),
(65, 'Raffarty', 'McCurley', 77, '0026 Beilfuss Drive', '276-92-8509'),
(66, 'Rowney', 'Bonnett', 51, '2056 Kedzie Pass', '165-12-7011'),
(67, 'Idaline', 'Waterdrinker', 65, '72 Sachs Crossing', '690-90-3106'),
(68, 'Hayyim', 'Wardingley', 44, '7587 Vermont Drive', '585-45-5365'),
(69, 'Carolan', 'Leyson', 74, '2895 Reindahl Way', '826-95-4023'),
(70, 'Sterne', 'Readmire', 58, '0 Paget Pass', '720-07-4280'),
(71, 'Celestina', 'Maha', 60, '25574 Redwing Drive', '290-30-0692'),
(72, 'Darius', 'Ellissen', 87, '21377 Kennedy Alley', '862-60-7020'),
(73, 'Helli', 'Abrahmovici', 55, '985 Springs Plaza', '448-50-1261'),
(74, 'Taite', 'Stithe', 36, '24 Melrose Pass', '112-43-7659'),
(75, 'Aurea', 'Muffitt', 85, '50690 6th Crossing', '716-45-4056'),
(76, 'Olympie', 'Hrinishin', 31, '3 Esch Circle', '584-57-0505'),
(77, 'Erick', 'Halleday', 84, '6995 Bunting Pass', '466-10-9360'),
(78, 'Angelika', 'Blasetti', 81, '57033 Gale Terrace', '879-87-0484'),
(79, 'Kissie', 'Le Barr', 66, '93540 Hoffman Park', '444-72-2538'),
(80, 'Chicky', 'Clotworthy', 43, '3 Moland Park', '406-47-9922'),
(81, 'Neddie', 'Hugill', 68, '195 Hermina Alley', '834-96-5672'),
(82, 'Nathanil', 'Gemmell', 81, '457 New Castle Terrace', '365-54-0528'),
(83, 'Orazio', 'McFfaden', 38, '435 Marquette Terrace', '135-11-2922'),
(84, 'Kerrill', 'O'' Molan', 29, '304 Sundown Avenue', '637-06-1117'),
(85, 'Birk', 'Dumphries', 42, '140 Meadow Valley Park', '527-51-8034'),
(86, 'Lucais', 'Cambling', 20, '61607 Corben Trail', '203-36-6181'),
(87, 'Brittne', 'Leggin', 61, '58 Continental Pass', '534-15-9803'),
(88, 'Gaylord', 'Stanistreet', 72, '1 Manley Center', '602-68-0145'),
(89, 'Ailee', 'Demetr', 56, '410 Vidon Way', '201-73-5753'),
(90, 'Davey', 'Peel', 25, '5283 Armistice Road', '225-58-3874'),
(91, 'Fara', 'Aloigi', 66, '96 Vermont Street', '728-39-3361'),
(92, 'Veda', 'Lettuce', 64, '22 Debra Circle', '848-17-9951'),
(93, 'Elmore', 'Jonathon', 73, '90 Bonner Pass', '810-85-7144'),
(94, 'Elmo', 'Romei', 61, '29181 Luster Center', '820-91-0497'),
(95, 'Aldon', 'Matkovic', 78, '127 Center Place', '548-40-7691'),
(96, 'Trueman', 'McGrah', 24, '1368 Pawling Crossing', '377-63-8613'),
(97, 'Aubrey', 'Garner', 40, '04 Prairie Rose Park', '310-40-5017'),
(98, 'Adriano', 'Pickrill', 50, '809 Montana Road', '353-50-6617'),
(99, 'Misty', 'Ollive', 17, '52 Hanson Lane', '846-99-0753'),
(100, 'Enrichetta', 'Anyon', 48, '4 Haas Park', '105-40-7273')

 SET IDENTITY_INSERT [dbo].Passengers OFF;

 SET IDENTITY_INSERT [dbo].LuggageTypes ON;

INSERT INTO LuggageTypes (Id,Type) VALUES
(1, 'Carry-On'),
(2, 'Upright Luggage'),
(3, 'Wheeled Business Case'),
(4, 'Duffel Bag'),
(5, 'Wheeled Duffel Bag'),
(6, 'Weekender Bag'),
(7, 'Garment Bag')

 SET IDENTITY_INSERT [dbo].LuggageTypes OFF;

 SET IDENTITY_INSERT [dbo].Luggages ON;

INSERT INTO Luggages (Id, LuggageTypeId, PassengerId) VALUES 
(1, 4, 89),
(2, 7, 78),
(3, 1, 35),
(4, 5, 51),
(5, 7, 79),
(6, 1, 27),
(7, 7, 68),
(8, 2, 61),
(9, 3, 56),
(10, 3, 97),
(11, 7, 9),
(12, 5, 33),
(13, 7, 10),
(14, 6, 77),
(15, 1, 75),
(16, 3, 58),
(17, 2, 89),
(18, 3, 40),
(19, 3, 24),
(20, 4, 46),
(21, 4, 42),
(22, 6, 21),
(23, 5, 16),
(24, 3, 71),
(25, 3, 38),
(26, 2, 31),
(27, 1, 52),
(28, 5, 15),
(29, 5, 33),
(30, 3, 87),
(31, 7, 5),
(32, 2, 64),
(33, 3, 3),
(34, 6, 96),
(35, 6, 74),
(36, 4, 48),
(37, 6, 37),
(38, 2, 48),
(39, 2, 37),
(40, 4, 92),
(41, 2, 41),
(42, 4, 54),
(43, 6, 77),
(44, 3, 15),
(45, 4, 23),
(46, 6, 78),
(47, 4, 46),
(48, 7, 99),
(49, 7, 20),
(50, 3, 55),
(51, 4, 65),
(52, 5, 60),
(53, 4, 25),
(54, 1, 79),
(55, 3, 43),
(56, 5, 50),
(57, 5, 13),
(58, 3, 59),
(59, 6, 20),
(60, 2, 64),
(61, 3, 78),
(62, 4, 98),
(63, 7, 56),
(64, 5, 53),
(65, 3, 1),
(66, 3, 60),
(67, 6, 52),
(68, 1, 77),
(69, 6, 4),
(70, 2, 27),
(71, 7, 97),
(72, 4, 43),
(73, 1, 34),
(74, 7, 48),
(75, 7, 1),
(76, 1, 61),
(77, 3, 4),
(78, 7, 95),
(79, 4, 80),
(80, 7, 4),
(81, 7, 73),
(82, 6, 39),
(83, 4, 30),
(84, 4, 55),
(85, 7, 7),
(86, 3, 79),
(87, 5, 11),
(88, 6, 93),
(89, 5, 8),
(90, 5, 60),
(91, 1, 1),
(92, 3, 27),
(93, 7, 74),
(94, 5, 21),
(95, 2, 51),
(96, 1, 29),
(97, 4, 76),
(98, 6, 54),
(99, 7, 71),
(100, 7, 78)

 SET IDENTITY_INSERT [dbo].Luggages OFF;

 SET IDENTITY_INSERT [dbo].Tickets ON;

INSERT INTO Tickets(Id, PassengerId, FlightId, LuggageId, Price) VALUES 
(1, 69, 17, 3, 247.14),
(2, 87, 12, 47, 447.82),
(3, 91, 48, 59, 404.87),
(4, 18, 76, 69, 276.14),
(5, 33, 39, 82, 274.46),
(6, 76, 41, 81, 346.68),
(7, 23, 13, 73, 72.44),
(8, 26, 92, 23, 440.12),
(9, 30, 8, 56, 163.28),
(10, 92, 10, 80, 273.69),
(11, 27, 16, 35, 382.32),
(12, 81, 16, 93, 51.06),
(13, 98, 28, 34, 354.18),
(14, 87, 50, 77, 261.92),
(15, 80, 69, 74, 432.77),
(16, 54, 31, 81, 104.53),
(17, 23, 33, 93, 339.94),
(18, 19, 23, 98, 294.36),
(19, 39, 37, 49, 69.16),
(20, 59, 58, 98, 393.38),
(21, 57, 90, 90, 183.44),
(22, 14, 38, 4, 131.65),
(23, 69, 25, 73, 238.58),
(24, 30, 69, 76, 330.17),
(25, 81, 80, 89, 103.63),
(26, 92, 59, 70, 176.07),
(27, 70, 95, 77, 66.22),
(28, 44, 82, 8, 138.16),
(29, 26, 98, 36, 328.16),
(30, 91, 70, 16, 33.01),
(31, 81, 43, 92, 348.48),
(32, 83, 5, 26, 184.31),
(33, 8, 23, 74, 28.35),
(34, 78, 91, 9, 66.8),
(35, 23, 22, 44, 136.07),
(36, 45, 61, 82, 187.4),
(37, 93, 73, 40, 279.23),
(38, 20, 78, 43, 56.52),
(39, 9, 60, 59, 386.11),
(40, 76, 79, 47, 327.68),
(41, 90, 98, 57, 240.43),
(42, 31, 76, 100, 287.47),
(43, 26, 9, 66, 351.47),
(44, 66, 24, 65, 312.38),
(45, 3, 48, 23, 377.3),
(46, 71, 18, 6, 155.41),
(47, 26, 95, 41, 355.62),
(48, 25, 27, 25, 86.25),
(49, 39, 36, 73, 439.96),
(50, 35, 24, 81, 101.12),
(51, 16, 42, 86, 331.08),
(52, 95, 7, 70, 178.44),
(53, 56, 72, 80, 262.41),
(54, 42, 20, 6, 136.87),
(55, 78, 1, 6, 73.33),
(56, 35, 17, 50, 40.11),
(57, 20, 66, 74, 189.73),
(58, 52, 52, 31, 214.43),
(59, 18, 43, 86, 159.59),
(60, 85, 51, 5, 115.18),
(61, 12, 30, 74, 303.25),
(62, 84, 11, 80, 258.24),
(63, 13, 25, 33, 407.24),
(64, 70, 97, 100, 228.1),
(65, 19, 24, 32, 93.41),
(66, 71, 18, 17, 49.64),
(67, 77, 22, 60, 368.01),
(68, 51, 32, 35, 244.04),
(69, 18, 52, 42, 217.74),
(70, 76, 43, 15, 311.77),
(71, 37, 77, 54, 129.67),
(72, 65, 26, 14, 365.38),
(73, 26, 67, 64, 427.0),
(74, 52, 4, 24, 407.3),
(75, 45, 78, 5, 126.4),
(76, 83, 42, 51, 270.62),
(77, 40, 3, 1, 43.54),
(78, 36, 4, 44, 95.58),
(79, 30, 87, 76, 332.45),
(80, 20, 33, 93, 342.65),
(81, 63, 77, 22, 117.5),
(82, 75, 34, 22, 393.41),
(83, 51, 12, 54, 138.26),
(84, 39, 52, 55, 382.25),
(85, 42, 4, 77, 190.12),
(86, 58, 20, 95, 293.76),
(87, 13, 58, 52, 435.05),
(88, 83, 55, 73, 33.11),
(89, 82, 96, 81, 200.98),
(90, 81, 86, 53, 75.17),
(91, 67, 45, 30, 203.3),
(92, 85, 13, 5, 438.77),
(93, 27, 84, 97, 344.77),
(94, 69, 90, 36, 109.16),
(95, 17, 81, 59, 151.97),
(96, 36, 75, 77, 303.47),
(97, 41, 30, 84, 373.9),
(98, 5, 79, 38, 42.62),
(99, 2, 83, 16, 76.72),
(100, 90, 62, 40, 141.96)

 SET IDENTITY_INSERT [dbo].Tickets OFF;

 GO
 USE Airport

INSERT INTO Planes([Name], Seats, [Range]) VALUES
('Airbus 336', 112, 5132),
('Airbus 330', 432, 5325),
('Boeing 369', 231, 2355),
('Stelt 297', 254, 2143),
('Boeing 338', 165, 5111),
('Airbus 558', 387, 1342),
('Boeing 128', 345, 5541)

INSERT INTO LuggageTypes(Type) VALUES
('Crossbody Bag'),
('School Backpack'),
('Shoulder Bag')

UPDATE Tickets
SET Price *= 1.13
WHERE FlightId IN (SELECT Id
					FROM Flights
					WHERE Destination = 'Carlsbad')

DELETE 
FROM Tickets
WHERE FlightId IN (SELECT Id
					FROM Flights
					WHERE Destination = 'Ayn Halagim')

DELETE 
FROM Flights
WHERE Destination = 'Ayn Halagim'


SELECT *
FROM Planes p
WHERE p.[Name] LIKE '%tr%'
ORDER BY p.Id, p.[Name], p.Seats, p.[Range]


SELECT FlightId, SUM(Price)
FROM Tickets
GROUP BY FlightId
ORDER BY SUM(Price) DESC, FlightId

SELECT p.FirstName + ' ' + p.LastName AS [Full Name], f.Origin, f.Destination
FROM Passengers p
JOIN Tickets t
ON t.PassengerId = p.Id
JOIN Flights f
ON f.Id = t.FlightId
ORDER BY [Full Name], f.Origin, f.Destination


SELECT p.FirstName, p.LastName, p.AGE
FROM Passengers p
FULL JOIN Tickets t
ON t.PassengerId = p.Id
WHERE t.PassengerId IS NULL
ORDER BY p.AGE DESC, p.FirstName, p.LastName

SELECT p.FirstName + ' ' + p.LastName AS [Full Name],
	   pl.[Name] AS [Plane Name],
	   f.Origin + ' - ' + f.Destination AS [Trip],
	   lt.[Type] AS [Luggage Type]
FROM Passengers p
JOIN Tickets t
ON t.PassengerId = p.Id
JOIN Flights f
ON f.Id = t.FlightId
JOIN Planes pl
ON pl.Id = f.PlaneId
JOIN Luggages l
ON l.Id = t.LuggageId
JOIN LuggageTypes lt
ON lt.Id = l.LuggageTypeId
ORDER BY [Full Name], [Plane Name], f.Origin, f.Destination, lt.[Type]

SELECT p.[Name], p.Seats, COUNT(t.PassengerId)  AS [Passengers Count]
FROM Planes P
LEFT OUTER JOIN Flights f
ON f.PlaneId = p.Id
LEFT JOIN Tickets t
ON t.FlightId = f.Id
GROUP BY p.[Name], p.Seats
ORDER BY COUNT(t.PassengerId) DESC, p.[Name], p.Seats

GO

CREATE OR ALTER FUNCTION udf_CalculateTickets(@origin NVARCHAR(50), @destination NVARCHAR(50), @peopleCount INT)
RETURNS NVARCHAR(50)
AS
BEGIN
	IF (@peopleCount <= 0)
	BEGIN
		RETURN 'Invalid people count!'
	END

	DECLARE @flightId INT = (SELECT TOP(1) Id
							 FROM Flights
							 WHERE Origin = @origin AND Destination = @destination)

	IF (@flightId IS NULL)
	BEGIN
		RETURN 'Invalid flight!'
	END

	DECLARE @price DECIMAL(15, 2) = (SELECT Price
									 FROM Tickets t
									 JOIN Flights f
									 ON t.FlightId = f.Id
									 WHERE f.Destination = @destination AND f.Origin = @origin)

	DECLARE @totalPrice DECIMAL(15, 2) = @price * @peopleCount
	RETURN 'Total price ' + CAST(@totalPrice AS NVARCHAR(50))
END

GO

CREATE OR ALTER PROCEDURE usp_CancelFlights
AS
	UPDATE Flights
	SET ArrivalTime = NULL, DepartureTime = NULL
	WHERE ArrivalTime > DepartureTime