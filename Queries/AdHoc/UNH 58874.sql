DECLARE @TempData TABLE(BF_SSN VARCHAR(9), LN_SEQ INT)
INSERT INTO @TempData
VALUES
('001683340',1),('001683340',2),('001825202',3),('001845143',1),('001845143',2),('002708166',2),('002708166',3),('002708166',4),('002708166',5),('002708166',6),('002708166',7),('002708166',8),('002781913',1),('002781913',2),('002826740',1),('004745256',1),('004745256',2),('004829902',1),('004829902',2),('010727854',1),('014742747',1),('014742747',2),('016704422',1),('016704422',2),('016860124',1),('016860124',2),('016860124',3),('016860124',4),('017729916',1),('017729916',2),('017743559',1),('017743559',2),('017743559',3),('017743559',4),('018502969',1),('018502969',2),('019529386',1),('019529386',2),('019529386',3),('019860787',1),('020849593',1),('020849593',2),('022684597',2),('022684597',3),('022684597',4),('022860801',1),('023725332',1),('024582421',1),('024582421',2),('025648547',1),('025648547',2),('025648547',3),('025724943',1),('025724943',2),('025724943',3),('028682349',1),('028682349',2),('028682349',3),('028682349',4),('028682349',5),('028682349',6),('028727551',1),('029661787',3),('029661787',4),('029661787',5),('033628106',1),('033628106',2),('033661113',1),('033661113',2),('033661113',3),('033661113',4),('033661113',5),('037583828',1),('038622639',1),('038622639',2),('039564804',1),('039686477',1),('039686477',2),('039686477',4),('039686477',6),('039686477',7),('039686477',8),('041822104',1),('041885302',1),('041885302',2),('042089016',1),('042089016',2),('042828590',1),('043843322',1),('043843322',2),('043843322',3),('043882617',2),('043882617',3),('043882617',4),('043882617',6),('043882617',7),('043900753',1),('044841505',1),('046847433',3),('046847433',4),('046847433',5),('046905160',1),('047780534',1),('047780534',2),('047780534',3),('047780534',4),('047843513',1),('047843513',2),('047843513',3),('047843513',4),('047849195',1),('047849195',2),('048746046',1),('048746046',2),('050724701',1),('050724701',3),('050724701',5),('051683005',1),('051683005',2),('051728777',1),('051728777',2),('051728777',3),('052744906',1),('052744906',2),('052760901',1),('052760901',2),('052760901',3),('052760901',4),('052760901',5),('052760901',6),('052760901',7),('052760901',8),('052768608',2),('053787514',1),('053787514',2),('054664467',1),('054664467',2),('054664467',3),('054664467',4),('054682666',1),('054682666',2),('054682666',3),('054682666',4),('054702240',1),('054702240',2),('054702240',3),('054944241',1),('055623246',1),('056745647',1),('056745647',2),('056745647',3),('058821820',1),('058821820',2),('059666564',1),('059801127',1),('059801127',2),('063708637',1),('063708637',2),('063708637',3),('063708637',4),('064623073',1),('064623073',2),('064623073',3),('064623073',4),('064623073',5),('064623073',6),('064623073',7),('064623073',8),('064623073',9),('064745074',1),('064763609',1),('064763609',2),('064847255',1),('064847255',2),('064847255',3),('064847255',4),('064847255',5),('064847255',6),('065725509',4),('065725509',5),('065805894',1),('065805894',2),('067668418',1),('067668418',2),('067685704',1),('067685704',2),('067685704',3),('067685704',4),('067705356',1),('067705356',2),('067705356',3),('067705356',4),('067705356',5),('067705356',6),('067840545',1),('068747221',1),('068747221',2),('068747221',3),('068747221',4),('069706925',1),('069706925',2),('069706925',3),('069706925',4),('070761991',1),('071687401',1),('071687401',2),('071687401',3),('071687401',4),('071745598',3),('071745598',4),('072723922',1),('072723922',2),('072826980',1),('073504685',1),('073504685',2),('073504685',3),('073504685',4),('074582978',1),('074582978',2),('074703825',1),('075688921',1),('075763200',1),('077704260',1),('077704260',2),('077707583',1),('077707583',2),('077707583',3),('077707583',4),('079743565',2),('079743565',3),('079744697',1),('080728566',1),('080728566',2),('080786255',1),('080786255',2),('080786255',3),('080786255',4),('083763498',1),('083763498',2),('083763498',3),('083908740',1),('083908740',2),('087609359',1),('088686800',1),('088686800',2),('088686800',3),('088686800',4),('088740279',1),('088740279',2),('088740279',3),('088740279',4),('088740279',6),('088740279',7),('088740279',9),('088740279',10),('089666572',1),('089666572',2),('089666572',3),('089666572',4),('089666572',5),('089666572',6),('089666572',7),('090769949',1),('090769949',2),('090902452',1),('090902452',2),('092627251',1),('092627251',2),('092627251',3),('092627251',4),('092729898',1),('092729898',2),('092729898',3),('092729898',4),('092766651',2),('092766651',3),('092766651',4),('093740431',1),('093740431',2),('094646918',1),('094646918',2),('094646918',3),('094646918',4),('094646918',5),('094781133',1),('094844272',1),('094844272',2),('094844272',3),('094844272',4),('095541477',1),('095541477',6),('095720352',1),('095720352',2),('095720352',3),('095769562',1),('095769562',2),('096747080',1),('096747080',2),('097585836',1),('097585836',2),('097585836',3),('097585836',4),('097604596',1),('097604596',3),('097604596',4),('097668146',1),('097668146',3),('097841026',1),('098701256',1),('098701256',2),('098701256',3),('098701256',4),('098705992',1),('098705992',2),('098705992',3),('100749487',1),('100749487',2),('100749487',3),('101709109',1),('101709109',2),('101709109',3),('101709109',4),('101709109',6),('101709109',7),('101709109',8),('101709109',9),('101760450',2),('101760450',3),('101760450',4),('102749983',1),('102765026',1),('103903729',1),('104704769',1),('104704769',2),('104704769',3),('104704769',4),('105647092',1),('105647092',3),('105647092',6),('105647092',7),('105647092',8),('105666954',1),('105666954',2),('105666954',3),('105666954',4),('106729467',1),('106729467',2),('106729467',3),('106729467',4),('107743120',1),('107743120',2),('107743120',3),('107743120',4),('107743120',5),('108728843',2),('108746999',1),('108746999',2),('108765820',1),('109402000',1),('109402000',2),('109402000',3),('110741671',1),('110741671',2),('110741671',3),('110741671',4),('112643367',1),('112643367',2),('112643367',3),('112643367',4),('112643367',5),('112725216',7),('112725216',8),('112725216',9),('112725216',10),('113700705',1),('113700705',2),('113700705',3),('113700705',4),('113706339',1),('113706339',2),('114706760',2),('114764749',1),('114840517',1),('115725369',1),('115725369',2),('115725369',3),('115769551',1),('115769551',2),('115769551',3),('115864096',3),('115902388',1),('115902388',2),('115902388',3),('115902388',4),('115902388',5),('115902388',6),('115902388',7),('115902388',8),('116546065',1),('116546065',2),('116546065',4),('116546065',5),('116546065',6),('116546065',7),('116546065',8),('116546065',9),('116546065',10),('116722471',1),('116722471',2),('118703336',1),('118703336',2),('118748775',1),('118748775',2),('118748775',3),('119705291',1),('119705291',2),('119784630',1),('119784630',2),('119784630',3),('119784630',4),('120705989',1),('120705989',3),('120705989',4),('120705989',5),('120705989',6),('120705989',7),('120749723',1),('120749723',2),('121421541',1),('121421541',2),('121506373',1),('121769262',1),('121769262',2),('121769262',3),('121769262',4),('122701328',1),('122701328',2),('122701328',3),('122701328',4),('122701328',5),('122701328',6),('122701336',1),('122701336',2),('122787767',1),('123785102',1),('124542817',1),('124542817',2),('124542817',4),('124565132',1),('126683038',1),('126846665',4),('126846665',5),('126861637',1),('126861637',2),('127564129',1),('127783066',1),('127783066',2),('127783066',3),('127783066',4),('128705002',1),('128705002',3),('128740454',1),('128740454',2),('128740454',3),('128740454',5),('128740454',6),('128741595',1),('130725367',1),('131681129',1),('131681129',2),('131681129',3),('131681129',4),('132567375',1),('132721350',1),('133723152',1),('133949370',1),('133949370',2),('133949370',3),('136067324',1),('136067324',5),('137171421',1),('140785547',1),('140785547',2),('140907057',2),('140907057',3),('144768284',1),('144768284',2),('144906093',1),('145884236',1),('145884236',2),('145884236',3),('145884236',4),('145884236',5),('145884236',6),('145884236',7),('145884236',8),('145884236',9),('145884236',10),('145884236',11),('145884236',12),('145884236',13),('146829361',1),('146829361',2),('146829361',3),('146829361',4),('150804544',1),('150804544',2),('150804544',3),('150804544',4),('150804544',5),('150804544',6),('150804544',7),('150866583',1),('154741848',1),('154741848',2),('154827182',1),('154827182',2),('160602458',2),('163667806',3),('163667806',4),('167700685',1),('168705636',1),('168705636',2),('168705636',4),('183669177',1),('183669177',2),('198563937',3),('198563937',4),('201706635',1),('201706635',2),('202663998',1),('203681782',1),('203681782',2),('203681782',3),('203681782',4),('203681782',5),('211484043',1),('211484043',2),('212132533',1),('212132533',2),('212132533',3),('212132533',4),('212132533',5),('212132533',6),('212132533',7),('212132533',8),('212154444',1),('212154444',2),('212154444',3),('212154444',4),('213153227',3),('213153227',4),('215174749',1),('215174749',2),('215398608',1),('215398608',2),('215435553',1),('215435553',2),('216175946',1),('216175946',2),('216175946',3),('216821892',1),('217210377',1),('217672635',1),('217672635',2),('217672635',3),('217672635',4),('218174633',1),('218174633',2),('219274094',1),('219274094',2),('220046768',1),('220061894',1),('220251375',1),('223277571',1),('223456625',6),('223456625',7),('223658460',1),('224828888',1),('226556036',1),('226556036',2),('227371813',1),('227371813',2),('227511489',1),('227511489',2),('227511489',3),('228473518',1),('228473518',2),('228473518',3),('228473518',4),('229437414',1),('229437414',4),('230534772',1),('230534772',2),('230534772',3),('231397319',1),('231397319',3),('231414127',1),('231414127',2),('231414127',3),('231414127',4),('231477738',1),('234251159',1),('234251159',2),('237658043',1),('239556139',1),('239556139',2),('239556139',3),('239556139',4),('239556139',5),('239615157',1),('239615157',2),('240631675',1),('241614743',1),('241614743',2),('242595074',1),('242595074',2),('242595074',3),('243516684',1),('245410338',1),('245410338',2),('245410338',3),('245410338',4),('245410338',5),('245410338',6),('245614266',1),('245699171',1),('245699171',2),('245699171',3),('245699171',4),('245699171',5),('245699171',6),('245699171',7),('248575239',1),('248575239',2),('248575239',3),('248575239',4),('250793428',1),('251873704',1),('251873704',2),('253595487',1),('253595487',2),('254737065',1),('255536550',1),('255536550',2),('255536550',3),('255592143',1),('255592143',2),('255592143',3),('255592143',4),('257612941',1),('285768354',1),('285768354',2),('285768354',3),('285768354',4),('285904881',1),('285904881',2),('285904881',3),('285904881',4),('285904881',5),('285904881',6),('286681353',3),('286681353',4),('286681353',5),('286681353',6),('286681353',7),('286681353',8),('286681353',9),('304980537',1),('304980537',2),('306115441',1),('317945092',1),('317945092',2),('317945092',3),('317945092',4),('317945092',5),('319920201',1),('319920201',2),('319920201',4),('319920201',5),('319920201',6),('321469844',1),('321469844',2),('323825434',1),('325882707',2),('328765653',1),('328765653',2),('328765653',3),('328765653',4),('334843679',1),('339742412',1),('339742412',2),('341761992',1),('341761992',2),('341761992',3),('341761992',4),('341761992',5),('341761992',6),('341761992',7),('342723098',1),('342723098',2),('342723098',3),('342723098',4),('342723098',5),('342723098',6),('346667111',1),('346667111',2),('352565521',1),('363025983',1),('363025983',2),('370900872',1),('370900872',3),('374239211',1),('374239211',2),('374239211',3),('385046465',3),('385046465',4),('388989074',3),('388989074',4),('389088903',1),('394884609',1),('394884609',3),('398903882',1),('398903882',2),('398903882',4),('399981681',1),('399981681',2),('401433338',2),('409690089',2),('409690089',3),('409690089',4),('410571208',1),('411655509',1),('413378132',1),('413378132',2),('414539788',1),('414539788',2),('414539788',3),('414539788',4),('418747623',1),('419252468',1),('419252468',2),('419252468',4),('419252468',5),('425133623',1),('426378077',3),('426378077',4),('427597923',1),('427597923',2),('427656611',1),('428671816',1),('428671816',2),('428671816',3),('428671816',4),('428671816',5),('428671816',6),('428671816',7),('428671816',8),('428673094',1),('428673094',2),('431576490',1),('431576490',2),('432635847',1),('432635847',2),('434495640',1),('434495640',2),('435570733',1),('435630780',1),('438631536',1),('438631536',2),('438631536',3),('439651395',3),('439651395',4),('439651395',5),('441821092',4),('441821092',5),('441821092',6),('441821092',7),('441821092',8),('441942263',1),('441942263',2),('441942263',3),('443784314',1),('449815583',1),('449853704',4),('449853704',5),('449853704',6),('449853704',7),('449939118',5),('449939118',6),('449996777',5),('449996777',7),('449996777',8),('449996777',11),('449996777',12),('450758314',7),('450758314',8),('450758314',9),('450758314',13),('450758314',14),('450790503',1),('450790503',2),('450790503',3),('450790503',4),('450790503',5),('450790503',6),('450790503',7),('450790503',8),('450836544',1),('450933987',1),('450933987',2),('451699056',1),('451699056',2),('451699056',3),('453257435',1),('453270533',1),('453590486',1),('453590486',2),('453590486',3),('453590486',4),('453590486',5),('453590486',6),('453978917',1),('453978917',2),('453978917',3),('454452317',2),('454452317',3),('454452317',4),('454452317',5),('454452317',6),('454671143',1),('454671143',2),('454671143',3),('454671143',4),('454959843',1),('454999127',1),('455973172',1),('455973172',2),('456514964',3),('456514964',4),('456658394',1),('456658394',2),('456658394',3),('456658394',4),('456663955',2),('456663955',3),('456715571',1),('456715571',2),('456715571',3),('456715571',4),('456890221',1),('456890221',2),('457596880',1),('457596880',3),('457617210',1),('457617210',2),('457617210',3),('457770883',1),('457770883',2),('457950909',2),('458790032',1),('458790032',2),('458790032',3),('458859633',1),('458911792',1),('458911792',2),('458911792',3),('458911792',4),('458911792',5),('458911792',6)

INSERT INTO @TempData
VALUES
('458935286',1),('458935286',2),('459997525',1),('460514658',1),('460514658',2),('460514658',3),('460514658',4),('460514658',5),('460514658',6),('460514658',7),('460514658',8),('460514658',9),('460514658',10),('460514658',11),('460514658',12),('460612765',1),('460612765',2),('460619205',1),('460910833',2),('461716379',4),('461716379',5),('461716379',6),('461716379',7),('461716379',8),('461913125',2),('461913125',3),('463654130',1),('463654130',2),('463654130',3),('463712386',1),('463712386',2),('463737172',1),('463737172',2),('463737172',3),('463737172',4),('463772020',1),('463772020',2),('463772020',3),('463772020',4),('463772020',5),('463772020',6),('464491653',1),('464491653',2),('464491653',3),('464491653',4),('464491653',5),('464491653',6),('464491653',7),('464692002',1),('464692002',2),('464694659',1),('465839825',1),('465839825',2),('465839825',5),('465857749',1),('465857749',2),('465990637',1),('466669658',1),('466669658',2),('466771889',1),('466771889',2),('467834435',1),('467834435',3),('472153682',1),('473172769',1),('473172769',2),('474020664',1),('477803631',1),('477803631',2),('477803631',3),('477803631',4),('477803631',5),('477803631',6),('477803631',7),('477803631',8),('485909405',2),('485909405',3),('486920207',1),('486920207',2),('488946841',1),('488946841',2),('488946841',3),('488946841',4),('488946841',5),('488946841',6),('489866305',1),('489866305',2),('490868551',1),('490868551',2),('490868551',3),('490868551',5),('490868551',6),('490868551',7),('490966510',1),('493927399',1),('493927399',2),('493927399',3),('494726174',1),('494726174',2),('495045050',1),('504983681',3),('504983681',4),('504983681',5),('504983681',6),('504983681',7),('504983681',8),('506987742',1),('506987742',2),('506987742',3),('506987742',4),('511132648',1),('511967781',1),('511967781',2),('512641347',1),('512641347',2),('516045327',1),('516061153',9),('516061153',10),('517981134',1),('517981134',2),('518331629',1),('518331629',2),('518331629',3),('518376944',1),('518982321',1),('519135302',1),('519135302',2),('519135302',3),('519155488',2),('519155488',3),('519155488',4),('519155488',5),('519707213',1),('519707213',2),('519707213',3),('519707213',4),('519707213',5),('519707213',6),('519707213',7),('519707213',8),('519707213',9),('519707213',10),('520294692',1),('521579285',1),('521654491',1),('521654491',2),('524152509',1),('524152509',2),('524152509',3),('524152509',4),('524531308',1),('524531308',2),('526837281',1),('526976073',1),('526976073',2),('526976073',3),('526976073',4),('527972293',1),('527972293',2),('527972293',3),('528773480',1),('528773480',2),('528773480',3),('528773480',4),('528815496',2),('528815496',3),('529551889',1),('529551889',2),('529599385',3),('529599385',4),('530252181',3),('530252181',4),('530252181',5),('530252181',6),('530252181',7),('530252181',8),('530394831',3),('530394831',4),('530591529',1),('531111993',1),('531111993',2),('531111993',3),('531134935',1),('531134935',2),('531134935',3),('531134935',4),('531137305',2),('531138563',1),('531138563',2),('531138563',3),('531193735',1),('531193735',2),('531296029',1),('531829962',1),('531829962',2),('531829962',3),('531829962',4),('531829962',5),('531829962',6),('531941454',1),('531941454',2),('531941454',3),('531941454',4),('531967570',1),('531967570',2),('531967570',3),('531967570',4),('531967570',5),('531967570',6),('531967570',7),('532028039',1),('532028039',2),('532028039',4),('532028039',5),('532028039',6),('532028039',8),('532060828',1),('532115534',1),('532115534',2),('532178995',1),('532178995',2),('532196204',1),('532196204',2),('532198940',1),('532198940',2),('532370874',1),('532497210',1),('532497210',2),('532497210',3),('532964437',1),('532964437',2),('532984117',2),('533087683',1),('533087683',2),('533171994',1),('533171994',2),('533171994',4),('533171994',7),('533218302',1),('533218302',2),('533924244',1),('533924244',2),('533924244',3),('533924244',4),('533924244',5),('533924244',6),('534064346',1),('534064346',2),('534064346',3),('534115029',1),('534115029',2),('534115029',3),('534115029',4),('534115029',5),('534130502',1),('534191070',4),('534193707',1),('534193707',2),('534193707',3),('534193707',4),('534193707',5),('534193707',6),('534726888',2),('534989422',1),('534989422',2),('534989422',3),('534989422',4),('534989422',5),('535024119',1),('535024119',2),('535024119',3),('535026268',1),('535047141',1),('535047141',2),('535066038',1),('535066038',2),('535066038',3),('535066038',4),('535066038',5),('535177727',1),('535312118',1),('535312118',2),('535312118',3),('535312118',4),('535312118',5),('535312118',6),('535846026',1),('535846026',2),('535846026',3),('535846026',4),('535846026',7),('535846026',8),('535846026',9),('535846026',10),('535846026',11),('535907865',1),('535907865',2),('535907865',3),('535907865',4),('535907865',5),('535907865',6),('536065624',2),('536069663',1),('536069663',2),('536069663',3),('536069663',4),('536069663',5),('536086551',1),('536086551',2),('536154128',1),('536176646',2),('536176646',3),('536216481',1),('536216481',2),('536216481',3),('536398950',1),('536437174',1),('536437174',2),('536494580',1),('536494580',2),('536494580',3),('536494580',4),('536494580',5),('536494580',6),('537027973',2),('537027973',3),('537065256',1),('537065256',3),('537065256',4),('537065379',1),('537136626',1),('537136626',2),('537136626',3),('537194607',1),('537194607',2),('537212647',1),('537212647',2),('537212647',3),('537212647',4),('537212647',5),('537218937',1),('537218937',2),('537740909',1),('537740909',2),('537740909',3),('537740909',4),('537942899',1),('537942899',2),('537964912',1),('537964912',2),('537964912',4),('537964912',5),('538158088',2),('538158088',3),('538158088',4),('538174052',1),('538174052',2),('538236024',1),('538849866',1),('538849866',2),('538849866',3),('538849866',4),('538921067',1),('538986057',1),('539085127',1),('539085127',2),('539112994',1),('539112994',2),('539112994',3),('539115954',1),('539115954',2),('539177390',1),('539177390',2),('539177390',3),('539193742',1),('539193742',2),('539193810',1),('539238767',1),('539238767',2),('539238767',3),('540086693',1),('540086693',2),('540086693',3),('540086693',4),('540086693',5),('540330926',1),('540330926',2),('540330926',3),('540330926',5),('540702609',1),('540702609',2),('541258333',1),('541258333',2),('541258333',3),('541258333',4),('541313822',1),('541803339',2),('541803339',3),('542214394',3),('542297687',1),('542373988',1),('543197796',1),('543197796',2),('543291970',1),('544041211',1),('544041211',2),('544041211',3),('544045230',1),('544045230',2),('544045230',3),('544045230',4),('544045230',5),('544045230',6),('544378226',1),('544378226',2),('544849233',1),('544849233',2),('546131315',1),('546131315',2),('546131315',3),('546350531',1),('546710010',1),('546710010',2),('546710010',3),('546776924',1),('546776924',2),('546776924',3),('546776924',4),('546931716',1),('546931716',2),('547639329',1),('547639329',2),('547639329',3),('547639329',4),('547639329',5),('547639329',6),('547917226',1),('547917226',2),('547917226',3),('547917226',4),('547917226',5),('547973923',1),('547973923',2),('547973923',3),('547973923',4),('547973923',5),('547973923',6),('548699585',1),('548699585',2),('548699585',3),('549712135',1),('549788039',1),('549796227',1),('549796227',2),('549796227',3),('549796227',4),('549796227',5),('549796227',6),('549978723',5),('549978723',6),('551599135',1),('551599135',2),('551599135',3),('551599135',4),('551599135',5),('551975546',1),('551975546',2),('551975546',4),('551975546',5),('552736219',1),('552736219',2),('552736219',3),('552892968',3),('552910394',1),('552910394',2),('552910394',3),('552910394',4),('553953373',1),('553953373',2),('554658342',1),('554658342',2),('554810866',1),('554810866',2),('554873063',1),('554873063',2),('554873063',5),('554873063',6),('554873063',7),('555690748',2),('555771475',1),('555771475',2),('556730488',1),('556730488',2),('556730488',3),('557570960',1),('557570960',2),('557570960',3),('557570960',4),('557835846',5),('557875325',1),('557875325',2),('557875325',3),('557875325',4),('557875325',5),('557993121',1),('558061290',1),('558897092',1),('558897092',2),('558953175',1),('558953175',2),('559317253',1),('559317253',2),('559532589',1),('559532589',2),('559830015',1),('559830015',2),('560081997',1),('560652807',1),('560652807',2),('560833813',1),('560833813',2),('560932020',1),('560932020',2),('560932020',3),('560932020',4),('561951344',1),('561951344',2),('561951344',3),('561971248',1),('561971248',2),('562576306',1),('562577880',1),('562577880',2),('562577880',3),('562806764',1),('562806764',2),('562994474',1),('562994474',2),('563879008',1),('564653627',1),('564653627',2),('564655021',1),('564796845',1),('564796845',2),('564796845',3),('564796845',4),('564830026',1),('564830026',2),('565154571',1),('565154571',2),('565532775',3),('565810009',1),('565810009',2),('565810009',3),('566772982',1),('566772982',2),('566772982',3),('566772982',4),('566772982',5),('566836762',1),('566836762',2),('566836762',3),('566836762',4),('566836762',5),('566847303',1),('567914200',1),('567914200',2),('567914200',3),('567914200',4),('568731568',1),('568731568',2),('568731568',3),('568753515',2),('568753515',3),('568754803',1),('568934590',1),('568934590',2),('569155220',1),('569155220',2),('569155220',3),('569155220',4),('569155220',5),('569155220',6),('569155220',7),('569155220',8),('569155220',9),('569155220',10),('569474480',1),('569474480',2),('569872140',1),('569872140',2),('569872140',3),('569872140',4),('569998201',1),('569998201',2),('569998201',3),('570510190',1),('570510190',2),('570510190',3),('570510190',4),('570510190',5),('570510190',6),('570610453',1),('570772702',1),('570772702',2),('570826013',1),('570871588',1),('570871588',2),('570871588',3),('570891371',1),('570891371',2),('570891371',5),('570891371',6),('570936533',1),('570936533',5),('570936533',7),('571719813',1),('571912644',1),('571993976',1),('572434014',1),('572434014',2),('572434014',3),('572434014',4),('572434014',5),('572652763',1),('572652763',2),('572736846',1),('573652222',1),('573652222',2),('573652222',3),('573652222',4),('573652222',5),('573652222',6),('573652222',7),('573652222',8),('573658565',1),('573793951',1),('573793951',2),('573793951',3),('573793951',4),('573793951',5),('573853687',1),('573853687',2),('573874190',1),('573874190',2),('573874190',3),('573899165',1),('573899165',2),('574506664',1),('574742804',1),('574742804',2),('574742804',3),('574742804',4),('574902154',1),('574902154',2),('574981078',4),('574981078',5),('575270829',1),('575310387',1),('575357373',1),('575357373',2),('575357373',3),('575357373',4),('575357373',5),('575398561',1),('575398561',2),('575412677',1),('576395797',1),('576395797',2),('577134173',1),('577156467',1),('577156467',2),('577156467',3),('577156467',4),('579046801',3),('579046801',4),('579196766',1),('579196766',2),('579196766',3),('579415505',1),('583872066',7),('583872066',8),('585773700',1),('589160830',1),('589160830',2),('589589399',1),('589978281',1),('590908271',1),('590908271',2),('590908271',3),('590908271',4),('590908271',5),('590908271',6),('591083679',1),('591083679',2),('592210664',1),('592210664',2),('592210664',3),('592210664',4),('592210664',5),('592210664',6),('593177568',1),('593177568',2),('593620100',1),('593620100',2),('593620100',3),('593620100',4),('593620100',5),('593620100',7),('593620100',8),('594905840',2),('594905840',3),('594905840',4),('594905840',5),('594905840',6),('594905840',7),('595121860',1),('595528520',1),('595682447',1),('595682447',2),('595682447',3),('595682447',4),('595682447',6),('595682447',7),('595682447',8),('595682447',9),('597124308',1),('597124308',2),('597124308',3),('598037450',1),('598037450',2),('598037450',3),('598037450',4),('598037450',5),('598037450',6),('598226855',1),('598226855',2),('599098744',1),('599098744',2),('599098744',3),('599098744',4),('599098744',5),('599098744',6),('599098744',7),('599098744',8),('599098744',9),('599098744',10),('600281112',1),('600281112',2),('600281112',3),('600289427',1),('600289427',2),('600446309',2),('600446309',3),('600446309',4),('600487948',1),('600575562',1),('600575562',2),('600725481',1),('600725481',2),('600768678',1),('600800752',1),('600800752',2),('600808192',1),('600808192',2),('600808192',3),('601155668',2),('601155668',3),('601155668',4),('601155668',5),('601881425',1),('601881425',2),('601905210',2),('601905210',3),('601905210',4),('602091504',1),('602091504',2),('602305558',1),('602341404',1),('602341404',2),('602462287',1),('602469007',1),('602469007',2),('602665410',1),('602665410',2),('603216098',1),('603342326',1),('603342326',3),('603342326',4),('603807059',1),('603807059',2),('604056325',1),('604056325',2),('604206450',1),('604206450',2),('604206450',3),('604206450',4),('604529706',1),('604529706',2),('604529706',3),('604529706',4),('604962876',1),('604962876',2),('604962876',3),('605108714',1),('605108714',2),('605157882',1),('605157882',2),('605200055',1),('605245255',1),('605245255',2),('605245255',3),('605245255',4),('605245255',5),('605316291',3),('605316291',4),('605603818',1),('605603818',2),('605603818',3),('606023042',1),('606023042',2),('606032698',3),('606032698',4),('606032698',5),('606032698',6),('606032698',7),('606032698',8),('606032698',9),('606032698',10),('606091187',3),('606091187',4),('606091187',5),('606091187',6),('606169722',1),('606381449',1),('606387051',1),('606387051',2),('606427633',1),('606427633',2),('607381077',1),('607409637',1),('607602124',1),('607602124',2),('607809627',1),('607809627',2),('607809627',3),('607809627',4),('608071617',1),('608071617',2),('608071617',3),('608229230',1),('608229230',2),('608229230',3),('608229238',1),('608340248',3),('608344609',1),('608344609',2),('608344609',3),('608344609',4),('608944915',1),('608944915',2),('608944915',3),('608944915',4),('609013998',1),('609013998',2),('609013998',3),('609013998',4),('609013998',5),('609186724',1),('609285460',1)
INSERT INTO @TempData
VALUES
('609285460',2),('609285460',3),('609307203',1),('609343034',1),('610181385',1),('610367956',2),('610367956',3),('610367956',4),('610367956',5),('610686929',1),('610686929',2),('611208117',1),('611563979',1),('611563979',2),('611563979',3),('611829747',1),('611829747',2),('611829747',3),('611829747',5),('611829747',6),('612102157',1),('612102157',2),('612102157',3),('612142195',4),('612142195',5),('612300676',1),('612300676',2),('612300676',3),('612300676',4),('612300676',5),('612300676',7),('612522984',2),('612709825',1),('612709825',2),('612726827',1),('613050221',3),('613050221',4),('613266299',1),('613266299',2),('613266299',3),('613266299',4),('613322595',1),('613322595',2),('613322595',3),('613347014',1),('613347014',2),('613347014',3),('613424078',1),('613424078',2),('613424078',3),('613424078',4),('613424078',5),('613948845',1),('613948845',2),('614034940',1),('614039854',1),('614286742',1),('614286742',2),('614303357',1),('614306444',1),('614306444',2),('614323721',3),('614323721',4),('614323721',5),('614323721',6),('614323721',7),('614323721',8),('614349144',1),('614449261',1),('614449261',2),('614449261',3),('614449261',4),('614449261',5),('614449261',6),('614449261',7),('614449261',8),('614449261',9),('614460005',1),('614460005',2),('614460005',3),('614460005',4),('614469140',1),('614469140',2),('614560854',1),('614560854',2),('614560854',3),('614647582',1),('614865691',1),('614865691',2),('614921802',1),('614921802',2),('614921802',3),('615057853',1),('615057853',2),('615057853',3),('615057853',4),('615057853',5),('615057853',6),('615380482',1),('615380482',2),('615380535',1),('615380535',2),('615447235',3),('615844994',1),('616075438',1),('616075438',2),('616075438',3),('616180783',6),('616180783',7),('616180783',8),('616247419',1),('616247419',2),('616247419',3),('616300529',1),('616321776',1),('616342242',1),('616342242',2),('616342242',4),('616385112',1),('616385112',2),('617031689',1),('617074443',1),('617074443',2),('617078511',3),('617078511',4),('617241599',1),('617241599',2),('617310088',1),('617310088',2),('617346401',2),('618182463',1),('618303982',1),('618303982',3),('618341629',1),('618341629',2),('618341629',3),('618341629',4),('618421690',1),('618421690',2),('618621524',1),('618621524',2),('618621524',3),('618621524',4),('618621524',5),('618621524',6),('618621524',7),('618621524',8),('618647886',1),('620052163',1),('620052163',2),('620052163',3),('620263458',1),('620263458',2),('620263458',5),('620304892',1),('620304892',2),('620304892',3),('620307830',1),('620307830',2),('620342955',1),('620531660',1),('621097998',1),('622303548',1),('622305436',1),('622305436',2),('622347314',1),('622347314',2),('622347314',3),('622347314',4),('622423446',1),('622423446',2),('622438640',1),('622863275',1),('622863275',2),('622863275',3),('622863275',4),('623318753',1),('623348324',1),('623348324',2),('623348324',3),('623348324',4),('624078536',1),('624078536',2),('624346937',1),('624346937',2),('624744170',1),('624744170',3),('625101801',1),('625101801',2),('625148124',1),('625148124',2),('625305835',1),('625404320',1),('625425824',1),('625425824',2),('625425824',3),('625425824',4),('625425824',5),('625425824',6),('625425824',7),('625425824',8),('626052658',1),('626052658',2),('626052658',6),('626052658',7),('626093296',1),('626093296',2),('626681336',1),('626681336',2),('627144924',1),('627161144',1),('627161144',2),('627163280',1),('627163280',2),('627163280',3),('627163280',4),('627163280',5),('627163280',6),('627181114',1),('627181114',2),('627181114',3),('628105054',1),('628105054',2),('628168791',1),('628168791',2),('628168791',3),('628221097',1),('628221097',2),('628221097',3),('628221097',4),('628463175',5),('629020751',1),('629034741',1),('629034741',2),('629127218',1),('629127218',2),('629205617',1),('629205617',2),('629248936',1),('629248936',2),('629248936',3),('630140268',1),('630860179',1),('631071710',1),('631123499',2),('631123499',3),('631123499',4),('631141596',1),('631141596',2),('631184820',1),('631245535',1),('631245535',2),('631245535',3),('632032334',3),('632032334',4),('632032334',5),('632032334',6),('632147648',1),('632183894',1),('633123752',1),('633125191',1),('633125191',2),('633222190',1),('633222190',4),('633228688',1),('634095417',1),('634105775',1),('634105775',2),('634420245',1),('634682073',1),('635070758',1),('635070758',2),('635070758',3),('635102673',1),('635102673',2),('635124991',2),('635124991',3),('635124991',4),('636010152',1),('636010152',2),('636010152',3),('636145931',1),('636145931',2),('636145931',3),('636245439',1),('636280100',1),('636280100',2),('637104603',1),('638286660',1),('638286660',2),('638286660',3),('638288565',1),('639056801',2),('639056801',3),('639056801',5),('639056801',6),('639056801',7),('640032046',3),('640032046',4),('640032046',5),('640032046',6),('640077400',1),('640077400',2),('640077400',3),('640077400',4),('640077400',5),('640101841',2),('640101841',3),('640101841',4),('640101841',5),('640248438',1),('641105763',3),('641105763',4),('641105763',5),('641105763',6),('641105763',7),('641105763',8),('641987540',1),('642201235',1),('642201235',2),('642201235',3),('642425154',3),('642425154',4),('643011163',1),('643011163',2),('643011163',3),('643265390',1),('643265390',2),('643265390',3),('643265390',4),('643265390',5),('644164783',1),('644164783',2),('644164783',3),('644164783',4),('644841574',1),('644841574',2),('644841574',3),('644841574',4),('645106704',1),('645126495',1),('645126495',2),('645126495',3),('645263491',1),('680745243',1),('680745243',2),('732055072',1),('732055072',2),('732055072',3),('733030503',2),('733032752',1),('733032752',2),('770010449',1),('770228204',1),('770228204',2),('770228204',3),('770228204',4),('770228204',5),('770228204',6),('770228204',7)

SELECT 
	T.*, 
	LN10.LD_LON_EFF_ADD 
FROM 
	@TempData T 
	INNER JOIN UDW..LN10_LON LN10 
		ON LN10.BF_SSN = T.BF_SSN 
		AND LN10.LN_SEQ = T.LN_SEQ