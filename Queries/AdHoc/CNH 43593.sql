declare @data table (SSN char(X),  LAST_NAME VARCHAR(XXX))
INSERT INTO @DATA VALUES
('XXXXXXXXX','ADAYA-ORTIZ            '),
('XXXXXXXXX','GILLETT                '),
('XXXXXXXXX','DAO                    '),
('XXXXXXXXX','HOLLOWAY               '),
('XXXXXXXXX','HODGES                 '),
('XXXXXXXXX','UDALL                  '),
('XXXXXXXXX','CHAPMAN                '),
('XXXXXXXXX','AGUIRRE                '),
('XXXXXXXXX','GARCIA GOMEZ           '),
('XXXXXXXXX','DELGADO GUILLEN        '),
('XXXXXXXXX','LEON                   '),
('XXXXXXXXX','RAMOS CISNEROS         '),
('XXXXXXXXX','GRAY                   '),
('XXXXXXXXX','RICH                   '),
('XXXXXXXXX','SCHNEIDER              '),
('XXXXXXXXX','AIPOALANI              '),
('XXXXXXXXX','VOIGT                  '),
('XXXXXXXXX','LAWTON                 '),
('XXXXXXXXX','DURYEA                 '),
('XXXXXXXXX','COCHRAN                '),
('XXXXXXXXX','FLATH                  '),
('XXXXXXXXX','WILKINS                '),
('XXXXXXXXX','MAYA                   '),
('XXXXXXXXX','PONCE                  '),
('XXXXXXXXX','TIPIKINA               '),
('XXXXXXXXX','TILLERY                '),
('XXXXXXXXX','DAVIS                  '),
('XXXXXXXXX','CARLOS                 '),
('XXXXXXXXX','SAXTON                 '),
('XXXXXXXXX','SHAFFER                '),
('XXXXXXXXX','NGUYEN                 '),
('XXXXXXXXX','TRAN                   '),
('XXXXXXXXX','GRAY                   '),
('XXXXXXXXX','MONTANO                '),
('XXXXXXXXX','MAY                    '),
('XXXXXXXXX','HOFFMANN               '),
('XXXXXXXXX','LINENKO                '),
('XXXXXXXXX','ELDRED                 '),
('XXXXXXXXX','CASTILLO               '),
('XXXXXXXXX','BOWYER                 '),
('XXXXXXXXX','SIBRIAN                '),
('XXXXXXXXX','LEAVENS                '),
('XXXXXXXXX','KELLER                 '),
('XXXXXXXXX','BULL                   '),
('XXXXXXXXX','CRAWFORD               '),
('XXXXXXXXX','ALBA                   '),
('XXXXXXXXX','HOLSTINE               '),
('XXXXXXXXX','KOBEL - DRAKE          '),
('XXXXXXXXX','IVEY                   '),
('XXXXXXXXX','MURRAY                 '),
('XXXXXXXXX','BRYANT                 '),
('XXXXXXXXX','MEZA                   '),
('XXXXXXXXX','JOHNSON                '),
('XXXXXXXXX','GALLEGOS               '),
('XXXXXXXXX','PASCUA                 '),
('XXXXXXXXX','ZHOU                   '),
('XXXXXXXXX','FLOWERS                '),
('XXXXXXXXX','DAVIS                  '),
('XXXXXXXXX','BOYCE                  '),
('XXXXXXXXX','PEARSON                '),
('XXXXXXXXX','STUTTERS               '),
('XXXXXXXXX','SLETVOLD               '),
('XXXXXXXXX','FOX                    '),
('XXXXXXXXX','SCHWANKL               '),
('XXXXXXXXX','DAVENPORT              '),
('XXXXXXXXX','SMITH                  '),
('XXXXXXXXX','KUZMACK                '),
('XXXXXXXXX','CHAVEZ PEREZ           '),
('XXXXXXXXX','STAHL                  '),
('XXXXXXXXX','TIERNEY                '),
('XXXXXXXXX','RAWLS                  '),
('XXXXXXXXX','BACCA RODRIGUEZ        '),
('XXXXXXXXX','MILLER                 '),
('XXXXXXXXX','PORTER                 '),
('XXXXXXXXX','RUBIO                  '),
('XXXXXXXXX','COLLINS                '),
('XXXXXXXXX','DIAB                   '),
('XXXXXXXXX','HERREJON               '),
('XXXXXXXXX','GALVEZ                 '),
('XXXXXXXXX','FUNK                   '),
('XXXXXXXXX','COMER                  ')

SELECT DISTINCT
	SSN,
	RTRIM(LAST_NAME) AS [LAST NAME],
	CASE 
		WHEN TX.BF_SSN IS NOT NULL THEN X
		WHEN TX.BF_SSN IS NOT NULL THEN X
		WHEN TX.BF_SSN IS NOT NULL THEN X
		ELSE X
	END AS TRANSFER_NUMBER
FROM
	@data D
	LEFT JOIN CDW..CS_TransferX TX
		ON TX.BF_SSN = D.SSN
		AND TX.DidTransfer = X
	LEFT JOIN CDW..CS_TransferX TX
		ON TX.BF_SSN = D.SSN
	LEFT JOIN CDW..CS_TransferX TX
		ON TX.BF_SSN = D.SSN
ORDER BY 
	TRANSFER_NUMBER 