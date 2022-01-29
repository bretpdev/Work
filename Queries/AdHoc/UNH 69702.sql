declare @data table(Account_Number varchar(10), Col2 varchar(10), loan_seq int, Balance varchar(100), Col5 varchar(10), Last_name varchar(100))
insert into @data values
('0545745763','1054',9,'$2,081.02','8/18/2020','ANDERSON'),
('0545745763','1054',10,'$2,900.00','8/18/2020','ANDERSON'),
('0901702206','1054',4,'$2,863.00','8/18/2020','HULL'),
('0901702206','1054',5,'$2,863.00','8/18/2020','HULL'),
('1068251985','1054',2,'$2,950.00','8/18/2020','WILDING'),
('1068251985','1054',5,'$2,950.00','8/18/2020','WILDING'),
('1178896484','1054',2,'$2,826.00','8/18/2020','VERNON'),
('1178896484','1054',4,'$2,826.00','8/18/2020','VERNON'),
('1459345555','1054',15,'$3,060.10','8/18/2020','PAYSTRUP'),
('2903000101','1054',14,'$1,179.33','8/18/2020','OLSEN'),
('2903000101','1054',15,'$2,059.69','8/18/2020','OLSEN'),
('3012146711','1054',2,'$2,900.00','8/18/2020','LAFAELE'),
('3236447703','1054',4,'$2,436.46','8/18/2020','DIPPRE'),
('3236447703','1054',5,'$2,403.06','8/18/2020','DIPPRE'),
('3574545539','1054',2,'$2,540.00','8/18/2020','JOHNSTONE'),
('3574545539','1054',5,'$2,540.00','8/18/2020','JOHNSTONE'),
('3574545539','1054',6,'$2,626.50','8/18/2020','JOHNSTONE'),
('3574545539','1054',7,'$2,626.50','8/18/2020','JOHNSTONE'),
('3745317586','1054',6,'$1,544.00','8/18/2020','BUCHANAN'),
('3745317586','1054',7,'$2,826.00','8/18/2020','BUCHANAN'),
('4111667014','1054',7,'$2,826.00','8/18/2020','MARTINA'),
('4111667014','1054',8,'$2,863.00','8/18/2020','MARTINA'),
('4178855097','1054',2,'$2,280.00','8/18/2020','WANKIER'),
('4178855097','1054',4,'$47.10','8/18/2020','WANKIER'),
('4182496897','1054',1,'$3,344.61','8/18/2020','FELLION'),
('4182496897','1054',2,'$3,344.61','8/18/2020','FELLION'),
('4187054426','1054',1,'$2,650.00','8/18/2020','KJAR'),
('4187054426','1054',2,'$2,650.00','8/18/2020','KJAR'),
('4187054426','1054',3,'$2,730.00','8/18/2020','KJAR'),
('4187054426','1054',4,'$2,730.00','8/18/2020','KJAR'),
('4187054426','1054',5,'$449.60','8/18/2020','KJAR'),
('4198967991','1054',1,'$2,810.00','8/18/2020','EMBLEY'),
('4206904578','1054',1,'$1,544.00','8/18/2020','BURKE'),
('4206904578','1054',2,'$1,544.00','8/18/2020','BURKE'),
('4206904578','1054',3,'$1,764.00','8/18/2020','BURKE'),
('4206904578','1054',4,'$1,764.00','8/18/2020','BURKE'),
('4206904578','1054',6,'$200.26','8/18/2020','BURKE'),
('4237158102','1054',1,'$2,810.00','8/18/2020','RUSSELL'),
('4264988861','1054',3,'$2,650.00','8/18/2020','HUNSAKER'),
('4264988861','1054',4,'$2,730.00','8/18/2020','HUNSAKER'),
('4276117253','1054',1,'$2,118.00','8/18/2020','PATTON'),
('4276117253','1054',3,'$2,855.00','8/18/2020','PATTON'),
('4286206904','1054',16,'$1,593.65','8/18/2020','MONSEN'),
('4295095856','1054',1,'$2,184.58','8/18/2020','GUNDERSON'),
('4295095856','1054',2,'$2,186.09','8/18/2020','GUNDERSON'),
('4295095856','1054',3,'$1,190.52','8/18/2020','GUNDERSON'),
('4309580134','1054',3,'$2,826.00','8/18/2020','PETERSON'),
('4314117172','1054',6,'$1,519.50','8/18/2020','HILL'),
('4314117172','1054',9,'$1,937.89','8/18/2020','HILL'),
('4314117172','1054',10,'$301.28','8/18/2020','HILL'),
('4337829001','1054',3,'$2,730.00','8/18/2020','MIZELL'),
('4351102885','1054',7,'$85.82','8/18/2020','BURGESS'),
('4351102885','1054',8,'$2,232.99','8/18/2020','BURGESS'),
('4351102885','1054',10,'$2,146.34','8/18/2020','BURGESS'),
('4358682842','1054',3,'$2,190.36','8/18/2020','MOTT'),
('4359672021','1054',7,'$3,031.91','8/18/2020','CLAWSON'),
('4362439305','1054',5,'$2,730.83','8/18/2020','LANE'),
('4362439305','1054',6,'$2,730.83','8/18/2020','LANE'),
('4362439305','1054',7,'$61.50','8/18/2020','LANE'),
('4363612605','1054',1,'$2,575.00','8/18/2020','ARNITA'),
('4363612605','1054',2,'$2,575.00','8/18/2020','ARNITA'),
('4369025587','1054',5,'$2,419.76','8/18/2020','ROBERTSON'),
('4369025587','1054',6,'$2,419.76','8/18/2020','ROBERTSON'),
('4370505030','1054',3,'$2,859.48','8/18/2020','READING'),
('4370505030','1054',4,'$3,042.00','8/18/2020','READING'),
('4370505030','1054',5,'$434.52','8/18/2020','READING'),
('4371598964','1054',3,'$3,331.77','8/18/2020','MERRILL'),
('4371598964','1054',4,'$3,331.77','8/18/2020','MERRILL'),
('4371672650','1054',1,'$1,804.10','8/18/2020','DRYER'),
('4371672650','1054',2,'$1,858.55','8/18/2020','DRYER'),
('4371672650','1054',3,'$1,717.35','8/18/2020','DRYER'),
('4388338990','1054',2,'$2,752.16','8/18/2020','LEWIS'),
('4388338990','1054',3,'$3,677.79','8/18/2020','LEWIS'),
('4388338990','1054',4,'$3,807.79','8/18/2020','LEWIS'),
('4388338990','1054',6,'$3,997.40','8/18/2020','LEWIS'),
('4388338990','1054',7,'$340.16','8/18/2020','LEWIS'),
('4388819168','1054',5,'$2,800.00','8/18/2020','MOORE'),
('4388819168','1054',6,'$2,800.00','8/18/2020','MOORE'),
('4388819168','1054',7,'$2,800.00','8/18/2020','MOORE'),
('4388819168','1054',8,'$2,800.00','8/18/2020','MOORE'),
('4411313356','1054',5,'$3,517.79','8/18/2020','WEAVER'),
('4411313356','1054',7,'$3,997.40','8/18/2020','WEAVER'),
('4411313356','1054',8,'$3,971.77','8/18/2020','WEAVER'),
('4412465993','1054',3,'$2,730.00','8/18/2020','MOHLER'),
('4413629663','1054',1,'$2,761.58','8/18/2020','BOWMAN'),
('4413629663','1054',2,'$2,761.58','8/18/2020','BOWMAN'),
('4428575353','1054',1,'$2,556.94','8/18/2020','ALLEN'),
('4428575353','1054',2,'$2,634.12','8/18/2020','ALLEN'),
('4428575353','1054',3,'$380.04','8/18/2020','ALLEN'),
('4429537973','1054',1,'$2,810.00','8/18/2020','HANSEN'),
('4429537973','1054',2,'$2,810.00','8/18/2020','HANSEN'),
('4439811808','1054',2,'$2,061.49','8/18/2020','SCHOFIELD'),
('4439811808','1054',3,'$1,842.41','8/18/2020','SCHOFIELD'),
('4440927264','1054',5,'$2,826.00','8/18/2020','SHELLEY'),
('4446376427','1054',5,'$2,393.00','8/18/2020','SALUONE'),
('4446376427','1054',6,'$2,393.00','8/18/2020','SALUONE'),
('4453157623','1054',6,'$2,693.00','8/18/2020','ROBINSON'),
('4453157623','1054',7,'$2,765.00','8/18/2020','ROBINSON'),
('4453157623','1054',8,'$2,765.00','8/18/2020','ROBINSON'),
('4453157623','1054',10,'$2,826.00','8/18/2020','ROBINSON'),
('4472313535','1054',7,'$2,669.45','8/18/2020','FREDERIKSEN'),
('4472313535','1054',8,'$2,669.45','8/18/2020','FREDERIKSEN'),
('4472419592','1054',8,'$2,591.67','8/18/2020','ESPLIN'),
('4472419592','1054',9,'$2,669.45','8/18/2020','ESPLIN'),
('4473410321','1054',5,'$2,483.33','8/18/2020','ROLF'),
('4473410321','1054',6,'$2,900.00','8/18/2020','ROLF'),
('4473410321','1054',7,'$3,000.00','8/18/2020','ROLF'),
('4473410321','1054',8,'$3,000.00','8/18/2020','ROLF'),
('4484204541','1054',1,'$2,863.00','8/18/2020','THOMAS'),
('4484204541','1054',2,'$2,863.00','8/18/2020','THOMAS'),
('4487010637','1054',1,'$2,863.00','8/18/2020','FENN'),
('4487470257','1054',1,'$2,898.02','8/18/2020','HIGGINS'),
('4487470257','1054',2,'$3,092.32','8/18/2020','HIGGINS'),
('4487470257','1054',3,'$3,447.79','8/18/2020','HIGGINS'),
('4487470257','1054',4,'$3,782.79','8/18/2020','HIGGINS'),
('4487470257','1054',6,'$621.83','8/18/2020','HIGGINS'),
('4501500507','1054',5,'$222.52','8/18/2020','VANBREE'),
('4506832701','1054',7,'$2,765.00','8/18/2020','OSTERGAARD'),
('4506832701','1054',8,'$2,765.00','8/18/2020','OSTERGAARD'),
('4507209189','1054',1,'$2,810.00','8/18/2020','WILKINSON'),
('4507209189','1054',2,'$2,810.00','8/18/2020','WILKINSON'),
('4507591997','1054',2,'$2,929.71','8/18/2020','IRWIN'),
('4507591997','1054',3,'$2,929.71','8/18/2020','IRWIN'),
('4522436959','1054',2,'$2,950.00','8/18/2020','GOBLE'),
('4540765756','1054',5,'$1,448.71','8/18/2020','PARK'),
('4540765756','1054',6,'$2,761.58','8/18/2020','PARK'),
('4540765756','1054',7,'$362.14','8/18/2020','PARK'),
('4548214131','1054',1,'$1,632.22','8/18/2020','SCHOLZ'),
('4555408472','1054',1,'$2,863.00','8/18/2020','BROCKMAN'),
('4555408472','1054',2,'$2,863.00','8/18/2020','BROCKMAN'),
('4558279748','1054',4,'$2,761.58','8/18/2020','JUDD'),
('4562999080','1054',1,'$2,887.00','8/18/2020','CRESS'),
('4562999080','1054',2,'$467.75','8/18/2020','CRESS'),
('4562999080','1054',3,'$30.16','8/18/2020','CRESS'),
('4567490314','1054',1,'$1,661.34','8/18/2020','SKIPPS'),
('4567490314','1054',2,'$1,555.94','8/18/2020','SKIPPS'),
('4567490314','1054',3,'$126.44','8/18/2020','SKIPPS'),
('4583008968','1054',2,'$1,082.31','8/18/2020','BERRY'),
('4593311399','1054',6,'$2,800.00','8/18/2020','FORSYTH'),
('4598849451','1054',3,'$3,249.20','8/18/2020','JOHNS'),
('4598849451','1054',4,'$2,854.12','8/18/2020','JOHNS'),
('4601567453','1054',1,'$2,863.00','8/18/2020','GIFFORD'),
('4618277476','1054',1,'$1,985.47','8/18/2020','HYER'),
('4633333324','1054',6,'$3,997.40','8/18/2020','SHELTON'),
('4633333324','1054',7,'$3,971.77','8/18/2020','SHELTON'),
('4638602229','1054',1,'$2,540.00','8/18/2020','BEECHER'),
('4638602229','1054',3,'$2,540.00','8/18/2020','BEECHER'),
('4653855384','1054',5,'$1,708.00','8/18/2020','BOUDREUAX'),
('4653855384','1054',6,'$1,450.00','8/18/2020','BOUDREUAX'),
('4655146770','1054',3,'$2,650.00','8/18/2020','REYNOLDS'),
('4655146770','1054',4,'$2,650.00','8/18/2020','REYNOLDS'),
('4655511492','1054',2,'$2,810.00','8/18/2020','WORKMAN'),
('4659037453','1054',5,'$3,000.00','8/18/2020','JOHNSON'),
('4659037453','1054',6,'$2,800.00','8/18/2020','JOHNSON'),
('4659037453','1054',7,'$2,800.00','8/18/2020','JOHNSON'),
('4659037453','1054',9,'$2,800.00','8/18/2020','JOHNSON'),
('4662232762','1054',4,'$2,855.98','8/18/2020','LEAVITT'),
('4662232762','1054',5,'$2,855.98','8/18/2020','LEAVITT'),
('4679439062','1054',2,'$1,950.71','8/18/2020','SAUNDERS'),
('4679439062','1054',3,'$1,929.71','8/18/2020','SAUNDERS'),
('4698185861','1054',6,'$2,950.00','8/18/2020','SHIRLEY'),
('4698185861','1054',7,'$2,950.00','8/18/2020','SHIRLEY'),
('4700816595','1054',3,'$2,826.00','8/18/2020','MORGAN'),
('4700816595','1054',5,'$2,826.00','8/18/2020','MORGAN'),
('4716885542','1054',4,'$2,880.64','8/18/2020','HOGAN'),
('4716885542','1054',5,'$2,957.71','8/18/2020','HOGAN'),
('4716885542','1054',6,'$2,800.00','8/18/2020','HOGAN'),
('4716885542','1054',8,'$2,800.00','8/18/2020','HOGAN'),
('4719008753','1054',4,'$3,337.75','8/18/2020','SWALBERG'),
('4719008753','1054',5,'$3,337.75','8/18/2020','SWALBERG'),
('4726968505','1054',6,'$2,998.60','8/18/2020','MESSICK'),
('4726968505','1054',7,'$1,976.13','8/18/2020','MESSICK'),
('4726968505','1054',8,'$333.17','8/18/2020','MESSICK'),
('4732921714','1054',1,'$2,950.00','8/18/2020','HUNT'),
('4732921714','1054',2,'$796.50','8/18/2020','HUNT'),
('4734673685','1054',3,'$2,656.44','8/18/2020','ERICKSON'),
('4734673685','1054',4,'$2,826.00','8/18/2020','ERICKSON'),
('4736530996','1054',1,'$2,800.00','8/18/2020','GONZALEZROCHA'),
('4736530996','1054',2,'$2,800.00','8/18/2020','GONZALEZROCHA'),
('4742583696','1054',6,'$2,855.98','8/18/2020','SPRAGUE'),
('4743190190','1054',3,'$961.33','8/18/2020','BRANNON'),
('4743190190','1054',4,'$2,591.67','8/18/2020','BRANNON'),
('4743190190','1054',5,'$1,630.34','8/18/2020','BRANNON'),
('4743356243','1054',3,'$1,997.82','8/18/2020','NORTH'),
('4743356243','1054',4,'$2,863.00','8/18/2020','NORTH'),
('4744454885','1054',3,'$2,540.00','8/18/2020','NILSSEN'),
('4748787312','1054',5,'$2,950.00','8/18/2020','PEDERSEN'),
('4748787312','1054',6,'$2,950.00','8/18/2020','PEDERSEN'),
('4748787312','1054',7,'$2,950.00','8/18/2020','PEDERSEN'),
('4748787312','1054',8,'$2,950.00','8/18/2020','PEDERSEN'),
('4755110743','1054',3,'$1,966.14','8/18/2020','HILL'),
('4755110743','1054',4,'$2,110.49','8/18/2020','HILL'),
('4773079647','1054',5,'$2,761.58','8/18/2020','MASON'),
('4783823379','1054',3,'$2,650.00','8/18/2020','HANSEN'),
('4783823379','1054',4,'$2,730.00','8/18/2020','HANSEN'),
('4794933934','1054',1,'$161.00','8/18/2020','HANSEN'),
('4794933934','1054',2,'$532.25','8/18/2020','HANSEN'),
('4799454139','1054',2,'$480.71','8/18/2020','OLSEN'),
('4799454139','1054',3,'$1,742.72','8/18/2020','OLSEN'),
('4799454139','1054',4,'$2,454.84','8/18/2020','OLSEN'),
('4799454139','1054',5,'$2,553.68','8/18/2020','OLSEN'),
('4799454139','1054',6,'$1,529.72','8/18/2020','OLSEN'),
('4812122007','1054',2,'$3,111.15','8/18/2020','PALMER'),
('4812122007','1054',3,'$3,504.42','8/18/2020','PALMER'),
('4812122007','1054',4,'$388.85','8/18/2020','PALMER'),
('4819336317','1054',1,'$2,439.09','8/18/2020','MCLEAN'),
('4819336317','1054',2,'$1,732.40','8/18/2020','MCLEAN'),
('4819336317','1054',3,'$767.94','8/18/2020','MCLEAN'),
('4828122539','1054',1,'$3,194.07','8/18/2020','RISHI'),
('4828122539','1054',2,'$3,194.07','8/18/2020','RISHI'),
('4828122539','1054',3,'$2,312.81','8/18/2020','RISHI'),
('4828122539','1054',4,'$3,273.12','8/18/2020','RISHI'),
('4834297858','1054',1,'$2,950.00','8/18/2020','DAVIS'),
('4834297858','1054',2,'$2,950.00','8/18/2020','DAVIS'),
('4840565271','1054',2,'$2,196.77','8/18/2020','SHAW'),
('4840565271','1054',3,'$3,304.22','8/18/2020','SHAW'),
('4840565271','1054',4,'$3,500.00','8/18/2020','SHAW'),
('4840565271','1054',5,'$3,822.79','8/18/2020','SHAW'),
('4844913469','1054',1,'$2,863.00','8/18/2020','FIFE'),
('4844913469','1054',2,'$2,863.00','8/18/2020','FIFE'),
('4846237052','1054',11,'$2,761.58','8/18/2020','PERRY'),
('4846237052','1054',13,'$2,855.98','8/18/2020','PERRY'),
('4856365439','1054',9,'$2,252.24','8/18/2020','HIGLEY'),
('4856365439','1054',11,'$2,252.24','8/18/2020','HIGLEY'),
('4856365439','1054',12,'$1,695.51','8/18/2020','HIGLEY'),
('4856365439','1054',13,'$1,839.22','8/18/2020','HIGLEY'),
('4863578474','1054',1,'$2,863.00','8/18/2020','HARDMAN'),
('4878707644','1054',5,'$1,363.71','8/18/2020','SMITH'),
('4878707644','1054',6,'$3,331.77','8/18/2020','SMITH'),
('4894074058','1054',1,'$2,650.00','8/18/2020','ARAGON'),
('4900992720','1054',5,'$3,832.79','8/18/2020','LATHEM'),
('4900992720','1054',6,'$3,782.79','8/18/2020','LATHEM'),
('4921638744','1054',10,'$2,669.15','8/18/2020','BAILEY'),
('4928591940','1054',2,'$2,950.00','8/18/2020','WEIGHT'),
('4928591940','1054',3,'$2,950.00','8/18/2020','WEIGHT'),
('4929653676','1054',3,'$2,761.58','8/18/2020','MELVILLE'),
('4929653676','1054',4,'$2,761.58','8/18/2020','MELVILLE'),
('4932414019','1054',2,'$2,800.00','8/18/2020','WALLACE'),
('4932414019','1054',4,'$2,800.00','8/18/2020','WALLACE'),
('4937756327','1054',2,'$2,540.42','8/18/2020','SLACK'),
('4937756327','1054',3,'$2,810.00','8/18/2020','SLACK'),
('4941249832','1054',4,'$554.35','8/18/2020','SCHWENDIMAN'),
('4941249832','1054',5,'$3,399.50','8/18/2020','SCHWENDIMAN'),
('4941249832','1054',6,'$2,800.50','8/18/2020','SCHWENDIMAN'),
('4943490665','1054',2,'$2,810.00','8/18/2020','ROBERTS'),
('4946283507','1054',3,'$2,800.00','8/18/2020','JEPPSON'),
('4946283507','1054',4,'$2,900.00','8/18/2020','JEPPSON'),
('4946283507','1054',5,'$210.00','8/18/2020','JEPPSON'),
('5008853060','1054',4,'$3,337.75','8/18/2020','WINN'),
('5011498537','1054',9,'$1,561.26','8/18/2020','KIMBALL'),
('5011498537','1054',10,'$2,750.00','8/18/2020','KIMBALL'),
('5013446910','1054',2,'$2,929.71','8/18/2020','HAWKINS'),
('5013446910','1054',3,'$2,929.71','8/18/2020','HAWKINS'),
('5041468157','1054',1,'$2,761.58','8/18/2020','PORTER'),
('5041468157','1054',2,'$2,761.58','8/18/2020','PORTER'),
('5045934154','1054',6,'$1,685.25','8/18/2020','WEBB'),
('5045934154','1054',7,'$1,450.00','8/18/2020','WEBB'),
('5049071774','1054',1,'$2,950.00','8/18/2020','JACOBS'),
('5049071774','1054',3,'$2,950.00','8/18/2020','JACOBS'),
('5055449160','1054',3,'$3,191.67','8/18/2020','KELSEY'),
('5055449160','1054',4,'$3,154.76','8/18/2020','KELSEY'),
('5055449160','1054',5,'$199.91','8/18/2020','KELSEY'),
('5057377395','1054',3,'$143.59','8/18/2020','JONES'),
('5057377395','1054',4,'$1,899.75','8/18/2020','JONES'),
('5057377395','1054',5,'$1,756.16','8/18/2020','JONES'),
('5062122044','1054',5,'$2,819.44','8/18/2020','MOSS'),
('5062122044','1054',6,'$3,500.00','8/18/2020','MOSS'),
('5062122044','1054',7,'$248.56','8/18/2020','MOSS'),
('5069550932','1054',5,'$3,320.75','8/18/2020','JOHNSON'),
('5069550932','1054',6,'$3,572.79','8/18/2020','JOHNSON'),
('5069550932','1054',7,'$306.62','8/18/2020','JOHNSON'),
('5070941750','1054',12,'$2,800.00','8/18/2020','FULLMER'),
('5074867253','1054',5,'$3,264.00','8/18/2020','STEVENS'),
('5074867253','1054',7,'$3,337.75','8/18/2020','STEVENS'),
('5076675358','1054',1,'$2,626.50','8/18/2020','PETERSON'),
('5076675358','1054',2,'$2,626.50','8/18/2020','PETERSON'),
('5082230657','1054',1,'$2,303.91','8/18/2020','HALL'),
('5082230657','1054',2,'$2,375.23','8/18/2020','HALL'),
('5082230657','1054',3,'$245.86','8/18/2020','HALL'),
('5090937996','1054',1,'$2,950.00','8/18/2020','STUART'),
('5090937996','1054',2,'$2,950.00','8/18/2020','STUART'),
('5109656564','1054',1,'$3,331.77','8/18/2020','OLSEN'),
('5109656564','1054',2,'$3,331.77','8/18/2020','OLSEN'),
('5110287587','1054',8,'$583.89','8/18/2020','ANDERSON'),
('5119132834','1054',4,'$2,519.67','8/18/2020','BAUM'),
('5119132834','1054',5,'$2,669.45','8/18/2020','BAUM'),
('5119132834','1054',6,'$285.56','8/18/2020','BAUM'),
('5122770675','1054',5,'$2,575.00','8/18/2020','WRIGHT'),
('5122770675','1054',6,'$2,575.00','8/18/2020','WRIGHT'),
('5134953385','1054',3,'$2,730.00','8/18/2020','FARRER'),
('5135445805','1054',5,'$3,832.79','8/18/2020','STANSFIELD'),
('5135445805','1054',6,'$3,920.00','8/18/2020','STANSFIELD'),
('5138854867','1054',1,'$2,950.00','8/18/2020','BOREN'),
('5138854867','1054',2,'$2,891.00','8/18/2020','BOREN'),
('5153573517','1054',1,'$2,810.00','8/18/2020','GRIFFITHS'),
('5153573517','1054',2,'$2,810.00','8/18/2020','GRIFFITHS'),
('5163629155','1054',5,'$2,965.95','8/18/2020','JOHNSON'),
('5163629155','1054',6,'$3,253.87','8/18/2020','JOHNSON'),
('5163629155','1054',7,'$443.72','8/18/2020','JOHNSON'),
('5177079338','1054',1,'$2,591.00','8/18/2020','HAMILTON'),
('5177079338','1054',2,'$2,810.00','8/18/2020','HAMILTON'),
('5179367013','1054',9,'$1,593.06','8/18/2020','DAVIES'),
('5203257103','1054',3,'$2,742.22','8/18/2020','LIU'),
('5203257103','1054',4,'$2,800.00','8/18/2020','LIU'),
('5203257103','1054',5,'$453.78','8/18/2020','LIU'),
('5210018420','1054',7,'$2,499.25','8/18/2020','GRANT'),
('5210018420','1054',8,'$2,575.00','8/18/2020','GRANT'),
('5211479055','1054',3,'$2,504.79','8/18/2020','PILCHER'),
('5211479055','1054',4,'$2,504.79','8/18/2020','PILCHER'),
('5211479055','1054',5,'$170.67','8/18/2020','PILCHER'),
('5229602734','1054',5,'$3,191.67','8/18/2020','HAMBLIN'),
('5229602734','1054',6,'$3,191.67','8/18/2020','HAMBLIN'),
('5241538943','1054',3,'$1,633.52','8/18/2020','HANDLEY'),
('5241538943','1054',4,'$2,779.94','8/18/2020','HANDLEY'),
('5258553858','1054',3,'$3,191.67','8/18/2020','JENSEN'),
('5258553858','1054',4,'$3,191.67','8/18/2020','JENSEN'),
('5265336964','1054',1,'$2,810.00','8/18/2020','FENN'),
('5268885311','1054',2,'$1,499.00','8/18/2020','CHRISTENSEN'),
('5268885311','1054',3,'$1,499.00','8/18/2020','CHRISTENSEN'),
('5268885311','1054',4,'$215.44','8/18/2020','CHRISTENSEN'),
('5273956173','1054',3,'$2,540.00','8/18/2020','GOWANS'),
('5273956173','1054',5,'$2,540.00','8/18/2020','GOWANS'),
('5274274595','1054',2,'$2,730.00','8/18/2020','JACKSON'),
('5274274595','1054',3,'$2,730.00','8/18/2020','JACKSON'),
('5275651602','1054',2,'$3,150.00','8/18/2020','PAGE'),
('5275651602','1054',4,'$3,150.00','8/18/2020','PAGE'),
('5280169803','1054',3,'$889.75','8/18/2020','BROTHERSON'),
('5280169803','1054',4,'$2,575.13','8/18/2020','BROTHERSON'),
('5280169803','1054',5,'$854.85','8/18/2020','BROTHERSON'),
('5282634352','1054',1,'$2,863.00','8/18/2020','SCHMIDT'),
('5284190531','1054',2,'$2,800.00','8/18/2020','WILLIAMS'),
('5284190531','1054',4,'$2,800.00','8/18/2020','WILLIAMS'),
('5284190531','1054',5,'$2,800.00','8/18/2020','WILLIAMS'),
('5284190531','1054',6,'$3,350.00','8/18/2020','WILLIAMS'),
('5284190531','1054',7,'$402.00','8/18/2020','WILLIAMS'),
('5287267738','1054',5,'$226.40','8/18/2020','EVANS'),
('5287267738','1054',6,'$3,180.57','8/18/2020','EVANS'),
('5287267738','1054',7,'$3,229.61','8/18/2020','EVANS'),
('5294394892','1054',1,'$2,650.00','8/18/2020','DEEM'),
('5294394892','1054',2,'$2,650.00','8/18/2020','DEEM'),
('5318226107','1054',1,'$2,670.47','8/18/2020','WARD'),
('5318226107','1054',2,'$2,670.47','8/18/2020','WARD'),
('5318226107','1054',3,'$2,556.56','8/18/2020','WARD'),
('5318226107','1054',4,'$2,758.56','8/18/2020','WARD'),
('5328265304','1054',1,'$3,150.00','8/18/2020','HEATH'),
('5328265304','1054',2,'$1,575.00','8/18/2020','HEATH'),
('5343971442','1054',1,'$2,575.00','8/18/2020','PEERY'),
('5343971442','1054',2,'$2,575.00','8/18/2020','PEERY'),
('5349691144','1054',2,'$1,900.60','8/18/2020','SADLER'),
('5349691144','1054',3,'$2,280.00','8/18/2020','SADLER'),
('5349691144','1054',4,'$309.40','8/18/2020','SADLER'),
('5356428648','1054',1,'$3,355.00','8/18/2020','SCHAUB'),
('5356428648','1054',2,'$3,355.00','8/18/2020','SCHAUB'),
('5357770481','1054',1,'$2,425.00','8/18/2020','VALLEJOZAMORA'),
('5357770481','1054',2,'$2,425.00','8/18/2020','VALLEJOZAMORA'),
('5357770481','1054',3,'$212.00','8/18/2020','VALLEJOZAMORA'),
('5373538463','1054',8,'$1,546.65','8/18/2020','BURRELL'),
('5373538463','1054',9,'$1,541.35','8/18/2020','BURRELL'),
('5380695234','1054',10,'$2,310.00','8/18/2020','MONROE'),
('5384998560','1054',1,'$3,165.18','8/18/2020','LEE'),
('5392358996','1054',1,'$2,539.31','8/18/2020','BOWMAN'),
('5392358996','1054',2,'$2,615.49','8/18/2020','BOWMAN'),
('5392358996','1054',3,'$2,615.49','8/18/2020','BOWMAN'),
('5392358996','1054',4,'$2,691.67','8/18/2020','BOWMAN'),
('5392358996','1054',5,'$262.04','8/18/2020','BOWMAN'),
('5393064436','1054',1,'$2,826.00','8/18/2020','TOMLINSON'),
('5393064436','1054',3,'$2,826.00','8/18/2020','TOMLINSON'),
('5396976141','1054',5,'$2,761.58','8/18/2020','CLARK'),
('5398919575','1054',1,'$2,950.00','8/18/2020','INMAN'),
('5398919575','1054',2,'$2,624.75','8/18/2020','INMAN'),
('5402121249','1054',1,'$2,730.00','8/18/2020','GREENWELL'),
('5402121249','1054',2,'$2,730.00','8/18/2020','GREENWELL'),
('5411399780','1054',5,'$2,765.00','8/18/2020','STEWART'),
('5414632978','1054',5,'$3,358.60','8/18/2020','HIGGS'),
('5414632978','1054',6,'$3,358.60','8/18/2020','HIGGS'),
('5414632978','1054',7,'$3,863.66','8/18/2020','HIGGS'),
('5414632978','1054',8,'$3,723.26','8/18/2020','HIGGS'),
('5427284524','1054',1,'$2,266.64','8/18/2020','REDD'),
('5427284524','1054',2,'$2,266.64','8/18/2020','REDD'),
('5427284524','1054',3,'$2,472.68','8/18/2020','REDD'),
('5427284524','1054',4,'$2,307.85','8/18/2020','REDD'),
('5427284524','1054',5,'$1,986.19','8/18/2020','REDD'),
('5428820355','1054',2,'$2,929.71','8/18/2020','WILLIAMS'),
('5428820355','1054',3,'$2,929.71','8/18/2020','WILLIAMS'),
('5442972897','1054',3,'$2,826.00','8/18/2020','STRATE'),
('5442972897','1054',5,'$2,826.00','8/18/2020','STRATE'),
('5443520240','1054',1,'$3,355.00','8/18/2020','LOTER'),
('5443520240','1054',2,'$3,355.00','8/18/2020','LOTER'),
('5445397352','1054',1,'$2,758.47','8/18/2020','CALDWELL'),
('5446162148','1054',3,'$1,774.24','8/18/2020','HONSVICK'),
('5454078804','1054',3,'$2,730.00','8/18/2020','SHEEDER'),
('5476804906','1054',1,'$2,863.00','8/18/2020','LAUDIE'),
('5477262071','1054',1,'$3,191.67','8/18/2020','BURTIS'),
('5477262071','1054',2,'$3,191.67','8/18/2020','BURTIS'),
('5480211718','1054',1,'$1,038.87','8/18/2020','RODRIGUEZMALO'),
('5480211718','1054',2,'$1,038.87','8/18/2020','RODRIGUEZMALO'),
('5480211718','1054',3,'$2,251.44','8/18/2020','RODRIGUEZMALO'),
('5480546876','1054',5,'$2,575.00','8/18/2020','NIELSEN'),
('5480546876','1054',6,'$2,650.00','8/18/2020','NIELSEN'),
('5484989998','1054',5,'$3,150.00','8/18/2020','MARICHE'),
('5490430491','1054',1,'$2,650.00','8/18/2020','NIELSON'),
('5490430491','1054',2,'$2,730.00','8/18/2020','NIELSON'),
('5494513796','1054',1,'$3,042.00','8/18/2020','TURNER'),
('5494513796','1054',2,'$3,042.00','8/18/2020','TURNER'),
('5494513796','1054',3,'$3,150.00','8/18/2020','TURNER'),
('5494513796','1054',4,'$3,150.00','8/18/2020','TURNER'),
('5495169460','1054',3,'$2,701.24','8/18/2020','WARD'),
('5497357789','1054',3,'$3,320.75','8/18/2020','JENKINS'),
('5497357789','1054',4,'$3,331.77','8/18/2020','JENKINS'),
('5500281086','1054',3,'$2,826.00','8/18/2020','MENIOVE'),
('5500281086','1054',4,'$2,826.00','8/18/2020','MENIOVE'),
('5544081923','1054',2,'$2,929.71','8/18/2020','FLINT'),
('5544081923','1054',3,'$2,929.71','8/18/2020','FLINT'),
('5557882472','1054',1,'$2,863.00','8/18/2020','ROCKWOOD'),
('5560052924','1054',7,'$3,331.77','8/18/2020','THUNELL'),
('5564048594','1054',3,'$3,304.22','8/18/2020','BUTTERS'),
('5564048594','1054',4,'$3,320.75','8/18/2020','BUTTERS'),
('5575227361','1054',5,'$3,000.00','8/18/2020','QUINONEZ'),
('5575227361','1054',6,'$3,000.00','8/18/2020','QUINONEZ'),
('5575227361','1054',7,'$2,800.00','8/18/2020','QUINONEZ'),
('5575227361','1054',8,'$2,800.00','8/18/2020','QUINONEZ'),
('5585776007','1054',4,'$874.46','8/18/2020','PLAYER'),
('5585776007','1054',5,'$3,331.77','8/18/2020','PLAYER'),
('5585776007','1054',6,'$3,807.79','8/18/2020','PLAYER'),
('5585776007','1054',7,'$3,832.79','8/18/2020','PLAYER'),
('5585776007','1054',9,'$2,457.31','8/18/2020','PLAYER'),
('5590029538','1054',1,'$2,826.00','8/18/2020','WILLIAMS'),
('5590029538','1054',4,'$2,826.00','8/18/2020','WILLIAMS'),
('5598563864','1054',1,'$3,331.18','8/18/2020','BROWN'),
('5598563864','1054',2,'$3,315.24','8/18/2020','BROWN'),
('5598563864','1054',3,'$2,099.08','8/18/2020','BROWN'),
('5598563864','1054',4,'$3,756.49','8/18/2020','BROWN'),
('5601563081','1054',1,'$2,626.50','8/18/2020','CHILD'),
('5601563081','1054',2,'$2,626.50','8/18/2020','CHILD'),
('5609653334','1054',3,'$2,534.25','8/18/2020','SMITH'),
('5609653334','1054',4,'$2,723.97','8/18/2020','SMITH'),
('5609653334','1054',5,'$121.78','8/18/2020','SMITH'),
('5610899176','1054',2,'$2,929.71','8/18/2020','PHELPS'),
('5610899176','1054',3,'$2,929.71','8/18/2020','PHELPS'),
('5622204956','1054',2,'$2,855.98','8/18/2020','RICHINS'),
('5622204956','1054',3,'$971.03','8/18/2020','RICHINS'),
('5622355365','1054',2,'$240.87','8/18/2020','SCHENCK'),
('5624175127','1054',5,'$3,000.00','8/18/2020','TOBIN'),
('5624175127','1054',6,'$2,800.00','8/18/2020','TOBIN'),
('5624175127','1054',7,'$2,800.00','8/18/2020','TOBIN'),
('5642167833','1054',1,'$2,810.00','8/18/2020','LEE'),
('5654630819','1054',1,'$2,950.00','8/18/2020','MINSON'),
('5654630819','1054',2,'$2,950.00','8/18/2020','MINSON'),
('5662470469','1054',1,'$1,800.78','8/18/2020','HALVERSEN'),
('5662470469','1054',2,'$2,556.22','8/18/2020','HALVERSEN'),
('5665428875','1054',1,'$930.98','8/18/2020','HOLLEY'),
('5665428875','1054',2,'$2,855.98','8/18/2020','HOLLEY'),
('5674098475','1054',1,'$2,810.00','8/18/2020','KIMBALL'),
('5674098475','1054',2,'$2,810.00','8/18/2020','KIMBALL'),
('5679514693','1054',6,'$2,500.00','8/18/2020','FORBUSH'),
('5679514693','1054',7,'$2,575.00','8/18/2020','FORBUSH'),
('5679514693','1054',8,'$206.00','8/18/2020','FORBUSH'),
('5680958514','1054',1,'$2,863.00','8/18/2020','MARTINEZ'),
('5680958514','1054',2,'$2,863.00','8/18/2020','MARTINEZ'),
('5704992896','1054',1,'$2,855.98','8/18/2020','HOWES'),
('5704992896','1054',2,'$2,855.95','8/18/2020','HOWES'),
('5704992896','1054',3,'$2,929.71','8/18/2020','HOWES'),
('5704992896','1054',5,'$3,330.85','8/18/2020','HOWES'),
('5713671920','1054',3,'$2,943.67','8/18/2020','BECK'),
('5713671920','1054',4,'$3,762.79','8/18/2020','BECK'),
('5713671920','1054',5,'$3,618.10','8/18/2020','BECK'),
('5713671920','1054',7,'$3,997.40','8/18/2020','BECK'),
('5713671920','1054',8,'$855.27','8/18/2020','BECK'),
('5720188039','1054',3,'$3,331.77','8/18/2020','SYDDALL'),
('5720188039','1054',4,'$3,326.26','8/18/2020','SYDDALL'),
('5743916245','1054',1,'$2,863.00','8/18/2020','NORTHCOTT'),
('5743916245','1054',2,'$2,863.00','8/18/2020','NORTHCOTT'),
('5751501098','1054',2,'$3,337.75','8/18/2020','JONES'),
('5751501098','1054',3,'$3,337.75','8/18/2020','JONES'),
('5751501098','1054',4,'$270.78','8/18/2020','JONES'),
('5756660888','1054',3,'$2,826.00','8/18/2020','PRICE'),
('5756660888','1054',5,'$2,826.00','8/18/2020','PRICE'),
('5757191278','1054',1,'$2,810.00','8/18/2020','DUNN'),
('5757191278','1054',2,'$2,810.00','8/18/2020','DUNN'),
('5761227018','1054',3,'$2,965.58','8/18/2020','CROSSLEY'),
('5761227018','1054',4,'$2,965.58','8/18/2020','CROSSLEY'),
('5762004658','1054',4,'$2,855.98','8/18/2020','GANUS'),
('5762004658','1054',5,'$2,855.98','8/18/2020','GANUS'),
('5765327385','1054',1,'$2,950.00','8/18/2020','MENDOZA'),
('5765327385','1054',2,'$2,950.00','8/18/2020','MENDOZA'),
('5783136281','1054',7,'$3,331.77','8/18/2020','DAVIS'),
('5783136281','1054',8,'$66.64','8/18/2020','DAVIS'),
('5798148294','1054',4,'$2,693.00','8/18/2020','OLSON'),
('5798148294','1054',5,'$2,693.00','8/18/2020','OLSON'),
('5807573858','1054',6,'$2,855.98','8/18/2020','DIXON'),
('5830278885','1054',1,'$2,810.00','8/18/2020','ANGLE'),
('5830278885','1054',2,'$2,810.00','8/18/2020','ANGLE'),
('6175797494','1054',2,'$2,600.00','8/18/2020','BARBER'),
('6175797494','1054',3,'$3,118.83','8/18/2020','BARBER'),
('6175797494','1054',4,'$2,750.00','8/18/2020','BARBER'),
('6175797494','1054',5,'$2,750.00','8/18/2020','BARBER'),
('6740706155','1054',7,'$2,438.48','8/18/2020','SHOCK'),
('6740706155','1054',9,'$2,329.64','8/18/2020','SHOCK'),
('7379559701','1054',5,'$3,000.00','8/18/2020','COPLAN'),
('7379559701','1054',6,'$3,000.00','8/18/2020','COPLAN'),
('7887215790','1054',5,'$866.67','8/18/2020','MERRILL'),
('7887215790','1054',6,'$1,590.00','8/18/2020','MERRILL'),
('7887215790','1054',7,'$2,900.00','8/18/2020','MERRILL'),
('7887215790','1054',8,'$3,172.88','8/18/2020','MERRILL'),
('7887215790','1054',10,'$758.33','8/18/2020','MERRILL'),
('8456603542','1054',11,'$1,622.11','8/18/2020','CURRAN'),
('8456603542','1054',12,'$272.42','8/18/2020','CURRAN'),
('8633185711','1054',14,'$958.75','8/18/2020','BOSTWICK'),
('9003829898','1054',4,'$1,552.00','8/18/2020','WARD'),
('9003829898','1054',6,'$2,635.00','8/18/2020','WARD'),
('9852488774','1054',2,'$2,950.00','8/18/2020','PINKHAM')

DECLARE @CAC TABLE (C_ID INT, FIRST_NAME VARCHAR(100), LAST_NAME VARCHAR(100))
INSERT INTO @CAC VALUES

(452711,'JENNIFER','QUINTANA'),
(540193,'IDAMAE','WALLACE'),
(544766,'MARINDA','MCDONALD'),
(551030,'CHRISTINA','OLSEN'),
(559326,'AUTUMN','DAVIES'),
(566768,'MADISON','BELKNAP'),
(569756,'CAROLYN','CURRAN'),
(570013,'JARED','EDWARDS'),
(570165,'MEGHAN','HIGLEY'),
(571917,'REBECKA','NIELSEN'),
(572035,'WHITNEY','BOWMAN'),
(575464,'VALERIE','BARBER'),
(575929,'TALER','GUNDERSON'),
(578132,'TRAVIS','MOSS'),
(578811,'SAVANNAH','KIMBALL'),
(578814,'RACHEL','FREDERIKSEN'),
(579648,'AMANDA','FAWCETT'),
(579703,'JEFFREY','HONSVICK'),
(582067,'KIERSTYN','SHOCK'),
(582456,'RYLEE','SCHENCK'),
(582699,'AMBER','OVERSON'),
(582912,'JESSIE','HIGGS'),
(583013,'DAVID','PAYSTRUP'),
(583309,'ROGER','QUINONEZ'),
(584240,'BRIANNA','ROLF'),
(584740,'COURTNEY','HICKEN'),
(585115,'LANE','MESSICK'),
(585148,'ERYN','THUNELL'),
(585259,'HEIDI','CLAWSON'),
(628658,'CAMI','PLAYER'),
(658271,'SAIGE','PALMER'),
(663594,'MEGAN','WARD'),
(664041,'MERCEDES','MORETTI-ERICKSON'),
(668327,'JANALEE','MERRILL'),
(669009,'ANNA','WEBBERLEY'),
(670324,'LOREN','MOTT'),
(672884,'REGAN','COPLAN'),
(678908,'JESSICA','TOBIN'),
(678960,'CLAYTON','HALL'),
(678998,'JESSICA','EVANS'),
(679388,'SARAH','SWALBERG'),
(679494,'DALLIN','CLOVE'),
(679603,'ERIK','LINDQUIST'),
(679667,'KAYLA','ROBERTS'),
(679842,'CAMILLE','WARD'),
(679880,'MALORIE','WRIGHT'),
(679947,'AMY','GRANT'),
(680209,'LEXUS','MONROE'),
(680491,'TARA','DIPPRE'),
(680536,'KATELYN','ESPLIN'),
(680615,'ZARA','BECK'),
(680627,'EMILY','WEBB'),
(680668,'RIELEY','BROTHERSON'),
(681106,'ANDRIA','HILL'),
(681117,'ANGELA','SCHWENDIMAN'),
(681388,'LAURA','VANBREE'),
(681449,'MADALYN','JOLLEY'),
(681465,'AMANDA','STANSFIELD'),
(681599,'ELISE','WEAVER'),
(681920,'MEGAN','HANSEN'),
(681956,'MADISON','LEWIS'),
(681989,'KATLEYN','ANDERSON'),
(682213,'ELISSA','SCHOFIELD'),
(682505,'CASSIDY','SMITH'),
(682681,'CHRISTAL','LIU'),
(683339,'AURA','SHIRLEY'),
(683642,'AMANDA','JENSEN'),
(683767,'KYLIE','BURGESS'),
(683867,'ANNA','PILCHER'),
(683890,'CATHY','PARK'),
(683983,'MELLISA','MELVILLE'),
(684023,'ALYSSA','JOHNSON'),
(684088,'BRITTANY','MONSEN'),
(684232,'VERONICA','HALVERSEN'),
(684307,'MICHAELA','HAMBLIN'),
(684321,'LAUREN','SKIPPS'),
(684419,'JESSICA','CARVER'),
(684492,'HEATHER','GANUS'),
(684763,'EMILY','PETERSON'),
(684877,'LISA','GIFFORD'),
(684934,'ASHLEY','STOTT'),
(684946,'ROBIN','PEDERSEN'),
(685501,'ANDRA','DAVIS'),
(685612,'CYNTHIA','MINSON'),
(685847,'ADRIAN','RAMJOUE'),
(685871,'NELLIE','ANDERSON'),
(685909,'JACOB','DALTON'),
(686037,'TAYLOR','STEWART'),
(686056,'CASSANDRA','MIZELL'),
(686096,'ALISSA','JACKSON'),
(686149,'SHELIAH','SALUONE'),
(686306,'COURTNEY','BRANNON'),
(686388,'EMILY','DIXON'),
(686483,'RILEY','DAVIS'),
(686654,'CAITLIN','MASON'),
(686687,'ANNA','BURNS'),
(686722,'SAMANTHA','STEVENS'),
(686751,'KORYSSA','FARRER'),
(686842,'DENAE','BOUDREUAX'),
(686974,'LILIBETH','MARICHE'),
(687063,'ADAM','NILSSEN'),
(687163,'JESSICA','HUNSAKER'),
(687225,'JESSICA','REYNOLDS'),
(687507,'JORDAN','FORSYTH'),
(687648,'AMY','FORBUSH'),
(687675,'MAKYNNA','BLACK'),
(687823,'KALEB','KJAR'),
(687858,'JILL','STRATE'),
(687889,'ERIN','WANKIER'),
(687946,'MEGAN','RUSSELL'),
(687959,'MICHAELLA','SCHOLZ'),
(688078,'KAITLYN','READING'),
(688289,'TAYLOR','WINN'),
(688368,'BRITNI','JOHNSON'),
(688663,'JESSICA','WILLENZIEN'),
(688763,'KARLY','SPRAGUE'),
(689180,'REBECA','GONZALEZROCHA'),
(689606,'SIERRA','SYDDALL'),
(689615,'LISA','SCHNEIDER'),
(690001,'TUCKER','SMITH'),
(690041,'KIRSTEN','OSTERGAARD'),
(690052,'PATRICIA','SADLER'),
(690073,'SHYLI','BUTTERS'),
(690099,'COURTNEE','HOGAN'),
(690099,'COURTNEE','HOGAN'),
(690219,'SAMANTHA','HANSEN'),
(690251,'ASHLEY','BAILEY'),
(690419,'BAILIE','CLARK'),
(690449,'NIKKELLE','JUDD'),
(690509,'ALEXIS','MCLEAN'),
(690542,'KENNETH','DEEM'),
(690550,'CARLIE','MOORE'),
(690600,'ALEESA','ARNITA'),
(690624,'ELIZABETH','PERRY'),
(690684,'MADELYN','HANDLEY'),
(690688,'EMMALEE','OLSEN'),
(690779,'ABIGAIL','LATHEM'),
(690792,'BRANDON','SHEEDER'),
(690809,'JESSIKA','JEPPSON'),
(690942,'EMILY','PEERY'),
(690945,'MELODY','WOOD'),
(691314,'ETHAN','DUNN'),
(691332,'KYLEE','SHAW'),
(691384,'MICHA','HILL'),
(691391,'LINDSEY','SHELLEY'),
(691411,'TOSHA','KELSEY'),
(691561,'HEATHER','OSWALD'),
(691595,'HAROLD','CAMPBELL'),
(691621,'ABIGAIL','NIELSON'),
(691714,'CADE','ROBERTSON'),
(691760,'ASHLEIGH','GENTRY'),
(691838,'ELIZABETH','SLACK'),
(691843,'ELIZABETH','BUCHANAN'),
(691870,'BAILEY','DRYER'),
(691993,'BRIANNA','JOHNSON'),
(691998,'SHONACEE','SCHMIDT'),
(692162,'NATHAN','GOWANS'),
(692297,'AIMIE','JOHNS'),
(692298,'VELCI','SAGASTUME-DE-SAV'),
(692310,'STEPHANIE','JOHNSTONE'),
(692311,'KATHLEEN','YOUNGBERG'),
(692331,'BAILEY','IRWIN'),
(692485,'HANNAH','FLINT'),
(692626,'SARAH','TURNER'),
(692662,'KAITLYN','STEPHEN'),
(692701,'CARLY','PAGE'),
(692748,'KATIE','JONES'),
(692768,'ELIZABETH','FARLEY'),
(692782,'MACIAH','JENKINS'),
(692833,'STACY','ARAGON'),
(693052,'ERIC','LAFAELE'),
(693371,'KAYLEE','MENIOVE'),
(693373,'CANDACE','MORGENSEN'),
(693408,'TRAVIS','THOMAS'),
(693419,'CAROLINE','ERICKSON'),
(693450,'AMBER','ROBINSON'),
(693457,'MEGAN','OLSON'),
(693480,'KENDALL','LEE'),
(693510,'ABIGAIL','WORKMAN'),
(693608,'ALEXIS','OLSEN'),
(693928,'REBECCA','FENN'),
(693946,'TRISH','FULLMER'),
(694076,'SYDNEY','KIMBALL'),
(694266,'KELLY','PRICE'),
(694280,'CONNER','MERRILL'),
(694387,'SAVANNAH','ANGLE'),
(694559,'CAUDICE','MORGAN'),
(694567,'ABBEY','SHELTON'),
(694733,'JASMIHN','WILLIAMS'),
(694835,'TAELOR','BURKE'),
(695951,'KAYLEE','HIGGINS'),
(695996,'JENIFER','JOHNSON'),
(696018,'SARAH','SCOTT'),
(696026,'ASHLEY','BAUM'),
(696244,'JANICE','JONES'),
(696287,'MADISON','HAMILTON'),
(696375,'RANDY','BOSTWICK'),
(696460,'LYNSIE','ALLEN'),
(696540,'AMBER','NORTH'),
(696588,'KIMBERLY','CHILD'),
(696855,'KRISTEN','LEE'),
(697027,'SARA','VALLEJOZAMORA'),
(697051,'MORGAN','TOMLINSON'),
(697122,'MICHAELA','GRIFFITHS'),
(697155,'MEGAN','SQUIRES'),
(697284,'MIKAYLA','CHRISTENSEN'),
(697362,'MADELINE','INMAN'),
(697387,'CRYSTAL','MARTINA'),
(697389,'JESSICA','GIFFORD'),
(697429,'STEPHANIE','WILKINSON'),
(697445,'MARY','GOBLE'),
(697454,'PARKER','GREENWELL'),
(697601,'SAMANTHA','CRESS'),
(707843,'SHARAE','DAVIS'),
(708051,'STEPHANIE','HANSEN'),
(708113,'BRENDEN','REDD'),
(708144,'KYLIE','SAUNDERS'),
(708154,'NATALIE','PORTER'),
(708221,'NICOLE','HOWES'),
(708240,'CADEN','BURRELL'),
(708383,'EMMA','LEE'),
(708562,'SHELBY','HOEFER'),
(708957,'BRIANNA','BERRY'),
(709192,'JOSEPH','RUNSTHROUGH'),
(709197,'DARLENE','MATHEWS'),
(709202,'LISA','LYMAN'),
(709215,'ASHLEE','BROCKMAN'),
(709231,'JENNIFER','WARD'),
(709248,'NORA','LAUDIE'),
(709264,'NICOLE','ROCKWOOD'),
(709278,'MICHAEL','VERNON'),
(709284,'TAMARA','FIFE'),
(709355,'HAILEY','HARDMAN'),
(709376,'JULIA','HULL'),
(709419,'TAYLOR','MARTINEZ'),
(709424,'BAYLEE','SMITH'),
(709433,'JUSTIN','WILLIAMS'),
(709438,'SETH','PRICE'),
(709442,'ALLISON','NORTHCOTT'),
(709507,'ABIGAIL','CLAYTON'),
(709671,'KANDAS','FENN'),
(710112,'ZOELINN','HOLLEY'),
(710117,'LISA','MENDOZA'),
(711192,'ABIGAIL','LANE'),
(711254,'SHALI','FLINT'),
(711275,'KRISTINE','BOWMAN'),
(711358,'JENNIFER','MOHLER'),
(711402,'CASSI','JACOBS'),
(711403,'NATALIE','WILLIAMS'),
(711610,'KARLIE','CROSSLEY'),
(711707,'MADALYN','PHELPS'),
(711793,'ANYA','BURTIS'),
(712161,'MIKKI','STUART'),
(712169,'KIMBERLY','FELLION'),
(712839,'EMILY','CALDWELL'),
(713321,'BROOKE','EMBLEY'),
(713510,'ANNA','HEATH'),
(713848,'JESSICA','PATTON'),
(714171,'ELIZABETH','HAWKINS'),
(714350,'STACIE','LEAVITT'),
(714552,'SARAH','BEECHER'),
(714553,'SARAI','PETERSON'),
(715013,'DONI','GALLEGOS'),
(715473,'LISA','PINKHAM'),
(715491,'JESSICA','WEIGHT'),
(716094,'JESSICA','BOREN'),
(716397,'ELIZABETH','RODRIGUEZMALO'),
(717584,'ANGELA','WILDING'),
(718595,'JOANNA','LOTER'),
(718956,'MARY','SCHAUB'),
(718983,'JACK','HYER'),
(719408,'NATHAN','RICHINS'),
(719693,'BOSTON','WORKMAN'),
(720906,'KELLY','HUNT'),
(721782,'JESSICA','RISHI'),
(715124,'VICTORIA','CADDY'),
(690513,'NICOLE','BRONSON'),
(692850,'MADISON','BROWN')


SELECT DISTINCT 
	D.*,
	RTRIM(PD10.DM_PRS_1) AS FIRST_NAME,
	C.C_ID AS CACTUS_ID,
	LN10.LD_LON_1_DSB AS LOAN_DISBURSEMENT_DATE
	--C.LX_ATY 
FROM 
	@DATA D
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_SPE_ACC_ID = D.Account_Number
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
		AND LN10.LN_SEQ = D.loan_seq
	INNER JOIN @CAC C
		ON C.LAST_NAME = D.Last_name
		AND C.FIRST_NAME = RTRIM(PD10.DM_PRS_1)
	