--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 1570 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'COVID-19 Pandemic IL (DR-4489)'
				,@BeginDate DATE = CONVERT(DATE,'20200326') --INCIDENT start date (not declaration date!)
				,@AddedBy VARCHAR(50) = 'UNH 66757' --change to current NH ticket
				,@DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbfed.Disasters);

		--SET IDENTITY_INSERT [dasforbfed].[Disasters] ON; --OPSDEV ONLY

			INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedBy)
			VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 89, @BeginDate), 1, @AddedBy);--1
			-- Save/Set the row count from the previously executed statement
			SELECT @ROWCOUNT = @@ROWCOUNT;		
		
		--SET IDENTITY_INSERT [dasforbfed].[Disasters] OFF; --OPSDEV ONLY
		
		--Affected zip codes:
		DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	
		DECLARE @Zips1 TABLE (ZipCode VARCHAR(5));
		INSERT INTO @Zips1 (ZipCode) VALUES
('61065'),
('61327'),
('60933'),
('62474'),
('62969'),
('62859'),
('61636'),
('61425'),
('62894'),
('61363'),
('61317'),
('62848'),
('60116'),
('60442'),
('62963'),
('62965'),
('62277'),
('61871'),
('62649'),
('62928'),
('62931'),
('61109'),
('60197'),
('61114'),
('60423'),
('61755'),
('60673'),
('60161'),
('61884'),
('62002'),
('62984'),
('61312'),
('60132'),
('61241'),
('61850'),
('60151'),
('60512'),
('62688'),
('61841'),
('62037'),
('60706'),
('61330'),
('62025'),
('60136'),
('60695'),
('62238'),
('62825'),
('60479'),
('62539'),
('61310'),
('62842'),
('60014'),
('60911'),
('61331'),
('62671'),
('60518'),
('60639'),
('62316'),
('61415'),
('62994'),
('60944'),
('61376'),
('62851'),
('61818'),
('60973'),
('60108'),
('60008'),
('62514'),
('61027'),
('60429'),
('62550'),
('62820'),
('62431'),
('61723'),
('60919'),
('61802'),
('61857'),
('61602'),
('62939'),
('60138'),
('60060'),
('61725'),
('60622'),
('61036'),
('60431'),
('61728'),
('62271'),
('60469'),
('61001'),
('60188'),
('61239'),
('60688'),
('62340'),
('60637'),
('60450'),
('61413'),
('61953'),
('62361'),
('62891'),
('62958'),
('61462'),
('61604'),
('61420'),
('62313'),
('60045'),
('60155'),
('62846'),
('60687'),
('61251'),
('62985'),
('61080'),
('61615'),
('60079'),
('60417'),
('60180'),
('62890'),
('60074'),
('60523'),
('62661'),
('60443'),
('61564'),
('62320'),
('62951'),
('60948'),
('62856'),
('61733'),
('60531'),
('60436'),
('62009'),
('61739'),
('60549'),
('62344'),
('62841'),
('60555'),
('60585'),
('60803'),
('62876'),
('61629'),
('61470'),
('60603'),
('60543'),
('62221'),
('62993'),
('61231'),
('60473'),
('60921'),
('60646'),
('61866'),
('60190'),
('60527'),
('61524'),
('61318'),
('61790'),
('62214'),
('60002'),
('60408'),
('61761'),
('60696'),
('60520'),
('61864'),
('62803'),
('62665'),
('62549'),
('62231'),
('62840'),
('60481'),
('60940'),
('62534'),
('61754'),
('62075'),
('60098'),
('62808'),
('60458'),
('60686'),
('62541'),
('62656'),
('60051'),
('62374'),
('60171'),
('61530'),
('62716'),
('60522'),
('61050'),
('60457'),
('62921'),
('61053'),
('62634'),
('60099'),
('60620'),
('60455'),
('62367'),
('62425'),
('62553'),
('62345'),
('62265'),
('61282'),
('62501'),
('60410'),
('62703'),
('60048'),
('60506'),
('62446'),
('61847'),
('60685'),
('62347'),
('60656'),
('61442'),
('62955'),
('62832'),
('61010'),
('61105'),
('60203'),
('62085'),
('61569'),
('61356'),
('60154'),
('61204'),
('61770'),
('61348'),
('62935'),
('61417'),
('61375'),
('60101'),
('61605'),
('60078'),
('62216'),
('62471'),
('62208'),
('60926'),
('60536'),
('62375'),
('60070'),
('62379'),
('62017'),
('61734'),
('60515'),
('62095'),
('61370'),
('62776'),
('61484'),
('61037'),
('62268'),
('61777'),
('60044'),
('60559'),
('62537'),
('61107'),
('62444'),
('62635'),
('61438'),
('61801'),
('60113'),
('61402'),
('62045'),
('61265'),
('61855'),
('60062'),
('60968'),
('60598'),
('60490'),
('61701'),
('62259'),
('61321'),
('62901'),
('61607'),
('60183'),
('61822'),
('62920'),
('60631'),
('60019'),
('61088'),
('60105'),
('61611'),
('61233'),
('62908'),
('61799'),
('62983'),
('60302'),
('62997'),
('60675'),
('62756'),
('60103'),
('62892'),
('61942'),
('60964'),
('61750'),
('62977'),
('60185'),
('61820'),
('60572'),
('62319'),
('61338'),
('60554'),
('60053'),
('60303'),
('61747'),
('62880'),
('61436'),
('62691'),
('62823'),
('61705'),
('62012'),
('62063'),
('62460'),
('60091'),
('61132'),
('60026'),
('60424'),
('62684'),
('60016'),
('61570'),
('61410'),
('60041'),
('61325'),
('60109'),
('62902'),
('62069'),
('60030'),
('60556'),
('61744'),
('61311'),
('61283'),
('62086'),
('60153'),
('61230'),
('61319'),
('61016'),
('62334'),
('60163'),
('60941'),
('60599'),
('60630'),
('62546'),
('61412'),
('60050'),
('61480'),
('61115'),
('62781'),
('61024'),
('61332'),
('62477'),
('61771'),
('62982'),
('61554'),
('60957'),
('62862'),
('62943'),
('62926'),
('62663'),
('60657'),
('62975'),
('60634'),
('62461'),
('61603'),
('60065'),
('62034'),
('61421'),
('61284'),
('61735'),
('60626'),
('61014'),
('62428'),
('61567'),
('61360'),
('60480'),
('61721'),
('61772'),
('62264'),
('62001'),
('62060'),
('60147'),
('62816'),
('60935'),
('61791'),
('60614'),
('60669'),
('62673'),
('62990'),
('61956'),
('60399'),
('60545'),
('60193'),
('60445'),
('62254'),
('60468'),
('60608'),
('62022'),
('61651'),
('60148'),
('61520'),
('60474'),
('62922'),
('60199'),
('62447'),
('60126'),
('61242'),
('61612'),
('61070'),
('62526'),
('61483'),
('60927'),
('62884'),
('62992'),
('61920'),
('62523'),
('62035'),
('60661'),
('61523'),
('62513'),
('62610'),
('62829'),
('62835'),
('62898'),
('62815'),
('60029'),
('62878'),
('62861'),
('61702'),
('60632'),
('60482'),
('62624'),
('61810'),
('61039'),
('60901'),
('60073'),
('61558'),
('61414'),
('60465'),
('60519'),
('62480'),
('61434'),
('62973'),
('60670'),
('62924'),
('60122'),
('62281'),
('60425'),
('62870'),
('61299'),
('60628'),
('60967'),
('60204'),
('62046'),
('60930'),
('61774'),
('61529'),
('62351'),
('62205'),
('61561'),
('62419'),
('61656'),
('62707'),
('62084'),
('62864'),
('60681'),
('61458'),
('62617'),
('62258'),
('62536'),
('60470'),
('60521'),
('62896'),
('62219'),
('60177'),
('60618'),
('62314'),
('61466'),
('62675'),
('62373'),
('61046'),
('62256'),
('60084'),
('62689'),
('61825'),
('62450'),
('60530'),
('61812'),
('60638'),
('60123'),
('61911'),
('60707'),
('62353'),
('62442'),
('60172'),
('62715'),
('61568'),
('60401'),
('62280'),
('62380'),
('61131'),
('62521'),
('60146'),
('60018'),
('60699'),
('60653'),
('60619'),
('62520'),
('60939'),
('62844'),
('61650'),
('61482'),
('61655'),
('61839'),
('62090'),
('61726'),
('61912'),
('62762'),
('60129'),
('62440'),
('62837'),
('60143'),
('60942'),
('60651'),
('62852'),
('61544'),
('62888'),
('61476'),
('62366'),
('61057'),
('61488'),
('61741'),
('61759'),
('61433'),
('62972'),
('62252'),
('62078'),
('62987'),
('61643'),
('62640'),
('60168'),
('62434'),
('62071'),
('61315'),
('62950'),
('62463'),
('62411'),
('60617'),
('60428'),
('62376'),
('60827'),
('61368'),
('60159'),
('62705'),
('62966'),
('61563'),
('61276'),
('60419'),
('61077'),
('60037'),
('62882'),
('61084'),
('62079'),
('62458'),
('60969'),
('61110'),
('62249'),
('61314'),
('62871'),
('60666'),
('61235'),
('61517'),
('60561'),
('61270'),
('62681'),
('62622'),
('60416'),
('60467'),
('62272'),
('61546'),
('61862'),
('61634'),
('61876'),
('60552'),
('62817'),
('62940'),
('62426'),
('60456'),
('62274'),
('61710'),
('60659'),
('61943'),
('61541'),
('62946'),
('61633'),
('61419'),
('61732'),
('62953'),
('60069'),
('61275'),
('60931'),
('61472'),
('60162'),
('61316'),
('62033'),
('61743'),
('62031'),
('61882'),
('60564'),
('62011'),
('60514'),
('62266'),
('61940'),
('60922'),
('60156'),
('61519'),
('61369'),
('62261'),
('61606'),
('61572'),
('62814'),
('60075'),
('61322'),
('60804'),
('61060'),
('60643'),
('62512'),
('61543'),
('61729'),
('61126'),
('62275'),
('62094'),
('62293'),
('62059'),
('62932'),
('60544'),
('62627'),
('61054'),
('61863'),
('60946'),
('61490'),
('60406'),
('61751'),
('60460'),
('61877'),
('60020'),
('60513'),
('61049'),
('62914'),
('60951'),
('60035'),
('62947'),
('62301'),
('61929'),
('62611'),
('60011'),
('60454'),
('61537'),
('60510'),
('62833'),
('61931'),
('61548'),
('60115'),
('61426'),
('61008'),
('62962'),
('62306'),
('61435'),
('61870'),
('62633'),
('61378'),
('61261'),
('61258'),
('61353'),
('61542'),
('62897'),
('62517'),
('60912'),
('61416'),
('61041'),
('61264'),
('61285'),
('60196'),
('60157'),
('61531'),
('62217'),
('61773'),
('60961'),
('61260'),
('61745'),
('60464'),
('60476'),
('62070'),
('62215'),
('60956'),
('60402'),
('62201'),
('60038'),
('61252'),
('62464'),
('60124'),
('60955'),
('62919'),
('60644'),
('62323'),
('62739'),
('61834'),
('61625'),
('60701'),
('61044'),
('61326'),
('62883'),
('61742'),
('61535'),
('61011'),
('62765'),
('60201'),
('60094'),
('60568'),
('60077'),
('61273'),
('61232'),
('60690'),
('60958'),
('62451'),
('61274'),
('62836'),
('60647'),
('62865'),
('60404'),
('62473'),
('61830'),
('60189'),
('60440'),
('60466'),
('61013'),
('61468'),
('61266'),
('61813'),
('60012'),
('61637'),
('62868'),
('60962'),
('60433'),
('61259'),
('62312'),
('62348'),
('62942'),
('60652'),
('61460'),
('62875'),
('61776'),
('60914'),
('60137'),
('61467'),
('62642'),
('61635'),
('61949'),
('62027'),
('62476'),
('61525'),
('62834'),
('62811'),
('61842'),
('60118'),
('60566'),
('62560'),
('60418'),
('62757'),
('61364'),
('62554'),
('62906'),
('60633'),
('62048'),
('60068'),
('62220'),
('62723'),
('60640'),
('61547'),
('61377'),
('62248'),
('62719'),
('60021'),
('61073'),
('62233'),
('62630'),
('60438'),
('62239'),
('60604'),
('61350'),
('60033'),
('60305'),
('60463'),
('61846'),
('62702'),
('62452'),
('62292'),
('61957'),
('60432'),
('60511'),
('60560'),
('62530'),
('61654'),
('62006'),
('62433'),
('62341'),
('61473'),
('62545'),
('60444'),
('61930'),
('62625'),
('62311'),
('62092'),
('61533'),
('62974'),
('60960'),
('62234'),
('62519'),
('61237'),
('61424'),
('62887'),
('60917'),
('62026'),
('62672'),
('62244'),
('60046'),
('62886'),
('61614'),
('62812'),
('61873'),
('61560'),
('60421'),
('61038'),
('60929'),
('62083'),
('61345'),
('60503'),
('60081'),
('62273'),
('61012'),
('61453'),
('61471'),
('61067'),
('61064'),
('61913'),
('62979'),
('62427'),
('60173'),
('60141'),
('62927'),
('60040'),
('61341'),
('60966'),
('62076'),
('60607'),
('60437'),
('61824'),
('60017'),
('61449'),
('61344'),
('62475'),
('60187'),
('62024'),
('60090'),
('62736'),
('61924'),
('61250'),
('61254'),
('61087'),
('62668'),
('62445'),
('61450'),
('62683'),
('62824'),
('61025'),
('61848'),
('62065'),
('61440'),
('60615'),
('60641'),
('62667'),
('61720'),
('60104'),
('60478'),
('61925'),
('62570'),
('62938'),
('61727'),
('62018'),
('62708'),
('62712'),
('62056'),
('60682'),
('60532'),
('61816'),
('62021'),
('62338'),
('62093'),
('61071'),
('61831'),
('62346'),
('60636'),
('61469'),
('60083'),
('61843'),
('61272'),
('61020'),
('62355'),
('60541'),
('61451'),
('62874'),
('61437'),
('62058'),
('61430'),
('62895'),
('62638'),
('62912'),
('61328'),
('62356'),
('61821'),
('62998'),
('61933'),
('62769'),
('62934'),
('62701'),
('61526'),
('62360'),
('62531'),
('62441'),
('61731'),
('62910'),
('61240'),
('61878'),
('62077'),
('61613'),
('62061'),
('61263'),
('61928'),
('61079'),
('60169'),
('62357'),
('62278'),
('62879'),
('62655'),
('61015'),
('62704'),
('61278'),
('61042'),
('60186'),
('60714'),
('60140'),
('60936'),
('62631'),
('61641'),
('60403'),
('61844'),
('61102'),
('61601'),
('62294'),
('62830'),
('61817'),
('62054'),
('60546'),
('60181'),
('62226'),
('62670'),
('62279'),
('62628'),
('60938'),
('61279'),
('60441'),
('62827'),
('61832'),
('61459'),
('60435'),
('60963'),
('60039'),
('60412'),
('60067'),
('61062'),
('60047'),
('61534')
;
		DECLARE @Zips2 TABLE (ZipCode VARCHAR(5));
		INSERT INTO @Zips2 (ZipCode) VALUES
('62469'),
('60301'),
('62232'),
('60007'),
('60471'),
('61313'),
('62915'),
('60420'),
('62052'),
('60602'),
('61856'),
('61342'),
('60624'),
('61101'),
('62522'),
('62295'),
('62796'),
('61075'),
('62543'),
('62417'),
('60712'),
('61031'),
('62988'),
('60674'),
('62613'),
('62763'),
('61760'),
('61740'),
('61236'),
('61932'),
('62088'),
('60959'),
('62918'),
('60920'),
('62563'),
('62329'),
('62019'),
('61775'),
('62013'),
('61048'),
('60134'),
('62923'),
('62954'),
('61361'),
('60645'),
('61320'),
('61323'),
('60462'),
('61074'),
('62885'),
('62612'),
('61852'),
('62651'),
('60120'),
('62410'),
('60434'),
('61432'),
('61455'),
('60539'),
('61068'),
('62532'),
('60150'),
('61475'),
('61354'),
('62449'),
('61951'),
('60691'),
('61334'),
('62693'),
('61914'),
('61030'),
('61565'),
('61552'),
('60913'),
('62695'),
('61704'),
('60649'),
('60096'),
('62961'),
('60697'),
('60950'),
('62917'),
('61340'),
('61803'),
('62028'),
('60176'),
('62807'),
('61277'),
('62202'),
('61125'),
('60642'),
('61379'),
('60567'),
('61880'),
('62726'),
('62956'),
('60013'),
('60629'),
('60472'),
('62916'),
('61301'),
('62555'),
('62424'),
('60110'),
('62518'),
('62941'),
('62850'),
('62462'),
('62439'),
('60918'),
('60551'),
('60184'),
('60082'),
('61474'),
('61111'),
('61724'),
('61257'),
('61811'),
('61559'),
('61019'),
('62692'),
('61262'),
('62601'),
('62573'),
('61337'),
('60439'),
('62321'),
('61418'),
('60087'),
('61028'),
('62535'),
('62010'),
('60621'),
('62241'),
('60095'),
('61063'),
('60910'),
('61106'),
('61749'),
('62903'),
('61256'),
('61372'),
('60111'),
('62359'),
('61858'),
('62242'),
('62297'),
('60932'),
('60550'),
('62465'),
('61447'),
('61955'),
('61528'),
('62363'),
('60128'),
('62677'),
('60534'),
('61439'),
('62049'),
('61448'),
('60953'),
('62098'),
('60304'),
('60086'),
('60165'),
('61333'),
('61373'),
('60623'),
('61764'),
('62413'),
('62257'),
('60121'),
('62091'),
('62246'),
('60517'),
('61919'),
('62722'),
('61061'),
('60131'),
('62285'),
('61875'),
('62843'),
('62030'),
('62326'),
('62854'),
('60178'),
('61555'),
('62260'),
('60525'),
('62074'),
('62222'),
('62849'),
('61478'),
('61089'),
('62418'),
('62324'),
('60076'),
('60970'),
('61130'),
('60061'),
('60557'),
('60022'),
('60504'),
('61550'),
('62481'),
('62032'),
('62643'),
('61485'),
('62863'),
('60093'),
('61571'),
('61736'),
('61371'),
('61653'),
('60142'),
('60409'),
('61539'),
('62282'),
('61201'),
('62867'),
('60678'),
('62907'),
('62253'),
('62690'),
('61730'),
('62236'),
('60538'),
('61748'),
('61465'),
('60507'),
('62525'),
('60461'),
('60668'),
('62354'),
('61047'),
('61638'),
('62081'),
('62960'),
('62538'),
('62818'),
('62644'),
('60064'),
('62225'),
('61085'),
('62454'),
('62767'),
('62819'),
('62571'),
('62067'),
('61833'),
('61018'),
('61709'),
('62343'),
('62459'),
('62639'),
('62557'),
('62339'),
('62040'),
('62250'),
('61422'),
('60055'),
('62828'),
('60451'),
('60130'),
('61243'),
('60605'),
('60088'),
('60452'),
('62685'),
('61104'),
('61441'),
('60194'),
('62240'),
('60502'),
('60006'),
('60133'),
('60097'),
('60491'),
('62330'),
('61006'),
('61769'),
('60164'),
('62524'),
('62358'),
('62089'),
('61865'),
('62540'),
('61051'),
('60526'),
('61335'),
('61937'),
('62533'),
('60974'),
('60447'),
('61358'),
('61427'),
('61562'),
('62243'),
('62626'),
('60540'),
('60542'),
('60952'),
('62822'),
('62957'),
('62288'),
('62948'),
('62467'),
('60805'),
('61815'),
('61849'),
('62286'),
('62223'),
('60548'),
('62801'),
('62666'),
('61758'),
('61324'),
('60625'),
('61826'),
('60949'),
('62080'),
('62414'),
('62761'),
('62858'),
('61532'),
('61491'),
('61616'),
('61103'),
('60042'),
('60004'),
('61778'),
('60174'),
('62565'),
('61362'),
('62839'),
('62561'),
('61454'),
('60407'),
('60606'),
('61281'),
('61059'),
('62422'),
('60202'),
('62567'),
('62262'),
('61489'),
('60119'),
('62853'),
('61752'),
('60505'),
('62515'),
('60117'),
('61753'),
('62629'),
('60005'),
('62298'),
('60448'),
('60010'),
('60015'),
('60089'),
('61078'),
('60475'),
('62766'),
('62047'),
('61486'),
('61359'),
('61452'),
('60043'),
('61859'),
('62352'),
('61851'),
('61072'),
('62794'),
('62551'),
('61652'),
('62325'),
('61477'),
('62660'),
('60537'),
('62365'),
('62674'),
('62860'),
('62995'),
('60449'),
('60915'),
('60160'),
('62255'),
('62650'),
('62615'),
('62959'),
('61501'),
('62362'),
('60677'),
('60613'),
('60106'),
('62062'),
('62949'),
('61738'),
('60085'),
('62082'),
('60693'),
('62438'),
('62711'),
('61238'),
('62682'),
('62263'),
('61043'),
('62050'),
('60152'),
('62572'),
('62015'),
('62821'),
('62466'),
('61411'),
('62869'),
('61936'),
('62872'),
('62909'),
('62568'),
('62016'),
('62905'),
('61007'),
('61610'),
('60499'),
('62421'),
('60145'),
('60954'),
('62806'),
('60694'),
('60945'),
('62866'),
('60563'),
('60484'),
('62694'),
('60558'),
('60664'),
('62349'),
('62659'),
('60009'),
('62087'),
('61910'),
('62370'),
('60071'),
('60934'),
('62284'),
('62618'),
('61443'),
('62289'),
('60446'),
('61349'),
('60601'),
('60565'),
('62967'),
('60175'),
('61032'),
('62556'),
('60191'),
('60459'),
('61401'),
('60144'),
('61374'),
('62448'),
('62786'),
('61874'),
('62207'),
('62547'),
('61423'),
('61367'),
('61540'),
('62706'),
('61737'),
('61854'),
('61021'),
('62378'),
('62930'),
('62544'),
('61553'),
('62432'),
('62036'),
('62203'),
('62401'),
('62510'),
('62893'),
('61234'),
('62420'),
('62970'),
('61941'),
('62964'),
('61516'),
('60684'),
('60430'),
('62952'),
('62999'),
('60654'),
('62478'),
('62230'),
('60689'),
('61336'),
('62831'),
('62548'),
('60025'),
('61917'),
('61112'),
('61091'),
('60616'),
('60422'),
('61428'),
('60516'),
('62976'),
('62423'),
('62764'),
('60569'),
('60056'),
('61872'),
('62777'),
('60195'),
('61329'),
('60487'),
('60477'),
('60453'),
('62838'),
('62206'),
('61431'),
('60411'),
('60415'),
('60553'),
('60586'),
('61944'),
('62023'),
('60611'),
('62468'),
('60612'),
('61479'),
('61244'),
('61108'),
('60135'),
('62443'),
('62336'),
('62053'),
('62558'),
('60107'),
('62662'),
('62996'),
('61756'),
('60072'),
('62810'),
('62889'),
('60609'),
('61545'),
('61845'),
('60610'),
('62791'),
('60034'),
('60192'),
('60102'),
('60179'),
('61536'),
('61639'),
('60112'),
('60660'),
('60208'),
('62809'),
('62204'),
('60928'),
('62044'),
('62097'),
('62014'),
('61853'),
('61346'),
('62881'),
('62664'),
('60426'),
('62237'),
('62933'),
('62305'),
('62479'),
('60680'),
('62899'),
('62877'),
('62245'),
('61630'),
('61052'),
('61883'),
('61722'),
('62051'),
('60031'),
('60655'),
('61814'),
('61938'),
('60139'),
('60924'),
('62436'),
('62218'),
('62621'),
('61840'),
('60501'),
('61081'),
('62269')
;
--		DECLARE @Zips3 TABLE (ZipCode VARCHAR(5));
--		INSERT INTO @Zips3 (ZipCode) VALUES
--;

--;WITH Z AS
--(
--	SELECT * FROM @ZIPS1 
--	UNION ALL
--	SELECT * FROM @ZIPS2 
--	--UNION ALL
--	--SELECT * FROM @ZIPS3
--)
--	select 'all_zips' as category, count(ZipCode) as tally from z
--	union all
--	select 'distinct_zips' as category, count(distinct ZipCode) as tally from z
--;

		IF NOT EXISTS 
		(
			SELECT 
				ZipId 
			FROM 
				[dasforbfed].[Zips] Z1
				INNER JOIN 
				(
					SELECT * FROM @ZIPS1 
					UNION ALL
					SELECT * FROM @ZIPS2 
					--UNION ALL
					--SELECT * FROM @ZIPS3 			
				) Z2
					ON Z1.ZipCode = Z2.ZipCode
			WHERE
				Z1.DisasterId = @DisasterID
		)
		BEGIN
			INSERT INTO 
				[dasforbfed].[Zips]	(ZipCode, DisasterId)
			SELECT DISTINCT
				ZipCode
				,@DisasterID 
			FROM 
				(
					SELECT * FROM @ZIPS1 
					UNION ALL
					SELECT * FROM @ZIPS2 
					--UNION ALL
					--SELECT * FROM @ZIPS3 			
				)z
		END;

		-- Save/Set the row count from the previously executed statement
		SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT;

	IF @ROWCOUNT = @ExpectedRowCount
		BEGIN
			PRINT 'Transaction committed.'
			COMMIT TRANSACTION
			--ROLLBACK TRANSACTION
		END
	ELSE
		BEGIN
			PRINT 'Transaction NOT committed.';
			PRINT 'Expected row count not met. Expecting ' +  CAST(@ExpectedRowCount AS VARCHAR(10)) + ' rows, but returned ' + CAST(@ROWCOUNT AS VARCHAR(10))+ ' rows.';
			ROLLBACK TRANSACTION;
		END
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed. Errors found in SQL statement.';
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;