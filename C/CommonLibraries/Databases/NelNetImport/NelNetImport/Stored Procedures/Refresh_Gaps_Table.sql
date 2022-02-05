
CREATE PROCEDURE dbo.Refresh_Gaps_Table
AS

	PRINT 'DELETE FROM #Gaps;'
	DELETE FROM #Gaps;

	/* Gaps between forebearances and deferments */
	PRINT 'INSERT INTO #Gaps;'
	INSERT INTO
		#Gaps
	SELECT 
		G.ssn,
		G.loan_number,
		DATEADD(dd, 1, G.before_end_date) [gap_start_date],
		G.after_start_date [gap_end_date],
		G.gap_days
	FROM
		(
			SELECT
				A.ssn,
				A.loan_number,
				A.row_num [after_row_num],
				A.end_date [after_end_date],
				B.row_num [before_row_num],
				B.end_date [before_end_date],
				A.start_date [after_start_date],
				B.sequence_number [before_sequence_number],
				A.sequence_number [after_sequence_number],
				DATEDIFF(dd, B.end_date, A.start_date) [gap_days]
			FROM
				(	/* forb/def info from the forb/def that comes after */
					SELECT
						CASE row_num
							WHEN -999 THEN MAX(row_num) OVER (PARTITION BY Z.ssn, Z.loan_number) + 1
							ELSE row_num
						END [row_num],  -- "-999" record must be the largest sequential row_num for each given borrower/loan
						Z.ssn,
						Z.loan_number,
						Z.sequence_number,
						Z.start_date,
						Z.end_date
					FROM
						(
							SELECT 
								ROW_NUMBER() OVER (PARTITION BY  DF.ssn, DF.loan_number ORDER BY DF.start_date) [row_num],
								DF.ssn,
								DF.loan_number,
								DF.sequence_number,
								DF.start_date,
								DF.end_date
							FROM
								#Deferments_Forbearances DF
							--WHERE
							--	DF.ssn in ('548437576', '111728861')
		
							UNION ALL

							SELECT /* add fake sequence to cover from now through end of time */
								-999 [row_num], -- flag used to identify the record that must be assigned the largest sequential row number for the given borrower/loan
								BDF.ssn,
								BDF.loan_number,
								255 [sequence_number],
								GETDATE() [start_date],
								'12/31/9999' [end_date]
							FROM
								#Bad_Deferments_Forbearances BDF -- used because some bad deferments/forbearances do not have "good" deferments/forbearances to work around
								LEFT JOIN #Deferments_Forbearances DF ON DF.bad_deferments_forbearances_id = BDF.bad_deferments_forbearances_id
							WHERE
								DF.deferments_forbearances_id is NULL
								--AND
								--BDF.ssn in ('548437576', '111728861')
							GROUP BY
								BDF.ssn,
								BDF.loan_number
						) Z
					GROUP BY
						Z.row_num,
						Z.ssn,
						Z.loan_number,
						Z.sequence_number,
						Z.start_date,
						Z.end_date
				) A
				INNER JOIN
				(
					/* forb/def info from the forb/def that comes before */
					SELECT
						ROW_NUMBER() OVER (PARTITION BY  DF.ssn, DF.loan_number ORDER BY DF.start_date) [row_num],
						DF.ssn,
						DF.loan_number,
						DF.sequence_number,
						DF.start_date,
						DF.end_date
					FROM
						#Deferments_Forbearances DF
					--WHERE
					--	DF.ssn in ('548437576', '111728861')

					UNION ALL
					
					SELECT /* add fake sequence to prevent identification of gaps that would be before earliest allowed date */
						0 [row_num],
						MPSD.ssn,
						MPSD.loan_number,
						-1 [sequence_number],
						'1/1/1900' [start_date],
						MPSD.min_plug_start_date [end_date]
					FROM
						#Min_Plug_Start_Date MPSD
					--WHERE
					--	MPSD.ssn in ('548437576', '111728861')
					) B ON B.ssn = A.ssn AND B.loan_number = A.loan_number AND B.row_num + 1 = A.row_num
		) G
	WHERE
		G.gap_days > 0
		AND
		G.before_end_date < GETDATE() -- ignore gaps that begin in the future
	ORDER BY
		G.ssn,
		G.loan_number,
		G.before_sequence_number;

	/* Insert gaps for bad def/forb that don't have any good def/forbs */
	INSERT INTO 
		#Gaps
	SELECT
		BDF.ssn,
		BDF.loan_number,
		MPSD.min_plug_start_date [gap_start_date],
		CAST(DATEADD(dd, MAX(BDF.forb_def_length_days) * -1, GETDATE()) as DATE) [gap_end_date],
		DATEDIFF(dd, MPSD.min_plug_start_date, CAST(DATEADD(dd, MAX(BDF.forb_def_length_days) * -1, GETDATE()) as DATE)) [gap_days]
	FROM
		#Bad_Deferments_Forbearances BDF
		LEFT JOIN #Deferments_Forbearances DF on DF.ssn = BDF.ssn AND DF.loan_number = BDF.loan_number
		LEFT JOIN #Min_Plug_Start_Date MPSD on MPSD.ssn = BDF.ssn and MPSD.loan_number = BDF.loan_number
	WHERE
		DF.deferments_forbearances_id IS NULL
	GROUP BY
		BDF.ssn,
		BDF.loan_number,
		MPSD.min_plug_start_date;

