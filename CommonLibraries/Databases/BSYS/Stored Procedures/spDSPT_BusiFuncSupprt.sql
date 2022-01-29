
CREATE PROCEDURE dbo.spDSPT_BusiFuncSupprt 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT A.BusFunction,COALESCE(B.Staff,'') as 'OneLINK Business Unit Lead',
			COALESCE(C.Staff,'') as '2nd OneLINK Business Unit Lead',
			COALESCE(D.Staff,'') as '3rd OneLINK Business Unit Lead',
			COALESCE(E.Staff,'') as '4th OneLINK Business Unit Lead',
			COALESCE(F.Staff,'') as 'COMPASS Business Unit Lead',
			COALESCE(G.Staff,'') as '2nd COMPASS Business Unit Lead',
			COALESCE(H.Staff,'') as '3rd COMPASS Business Unit Lead',
			COALESCE(I.Staff,'') as '4th COMPASS Business Unit Lead',
			COALESCE(J.Staff,'') as 'Primary Support Person',
			COALESCE(K.Staff,'') as '2nd Primary Support Person',
			COALESCE(L.Staff,'') as '3rd Primary Support Person',
			COALESCE(M.Staff,'') as '4th Primary Support Person',
			COALESCE(N.Staff,'') as 'Secondary Support Person',
			COALESCE(O.Staff,'') as '2nd Secondary Support Person',
			COALESCE(P.Staff,'') as '3rd Secondary Support Person',
			COALESCE(Q.Staff,'') as '4th Secondary Support Person' 
	FROM GENR_LST_BusFunctions A 
		LEFT JOIN (
					SELECT BB.BusFunction, 
							BB.Role, 
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
							END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName
				) B ON A.BusFunction = B.BusFunction 
					AND B.Role = 'OneLINK Business Unit Lead' 
		LEFT JOIN (
					SELECT BB.BusFunction,
							BB.Role,
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
							END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName 
				) C ON A.BusFunction = C.BusFunction 
					AND C.Role = '2nd OneLINK Business Unit Lead' 
		LEFT JOIN (
					SELECT BB.BusFunction,
							BB.Role, 
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
							END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName 
				) D ON A.BusFunction = D.BusFunction 
					AND D.Role = '3rd OneLINK Business Unit Lead' 
		LEFT JOIN (
					SELECT BB.BusFunction, 
							BB.Role, 
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
							END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName
				) E ON A.BusFunction = E.BusFunction 
					AND E.Role = '4th OneLINK Business Unit Lead' 
		LEFT JOIN (
					SELECT BB.BusFunction,
							BB.Role,
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
							END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName
				) F ON A.BusFunction = F.BusFunction 
					AND F.Role = 'COMPASS Business Unit Lead' 
		LEFT JOIN (
					SELECT BB.BusFunction,
							BB.Role,
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
							END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName 
				) G ON A.BusFunction = G.BusFunction 
					AND G.Role = '2nd COMPASS Business Unit Lead' 
		LEFT JOIN (
					SELECT BB.BusFunction, 
							BB.Role,
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
							END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName
				) H ON A.BusFunction = H.BusFunction 
					AND H.Role = '3rd COMPASS Business Unit Lead' 
		LEFT JOIN (
					SELECT BB.BusFunction,
							BB.Role,
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
							END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName
				) I ON A.BusFunction = I.BusFunction 
					AND I.Role = '4th COMPASS Business Unit Lead' 
		LEFT JOIN (
					SELECT BB.BusFunction,
							BB.Role,
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
							END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName
				) J ON A.BusFunction = J.BusFunction 
					AND J.Role = 'Primary Support Person' 
		LEFT JOIN (
					SELECT BB.BusFunction,
							BB.Role,
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
							END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName
				) K ON A.BusFunction = K.BusFunction 
					AND K.Role = '2nd Primary Support Person' 
		LEFT JOIN (
					SELECT BB.BusFunction,
							BB.Role,
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
							END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName
				) L ON A.BusFunction = L.BusFunction 
					AND L.Role = '3rd Primary Support Person' 
		LEFT JOIN (
					SELECT BB.BusFunction,
							BB.Role,
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
							END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName 
				) M ON A.BusFunction = M.BusFunction 
					AND M.Role = '4th Primary Support Person' 
		LEFT JOIN (
					SELECT BB.BusFunction,
							BB.Role,
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
							END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName
				) N ON A.BusFunction = N.BusFunction 
					AND N.Role = 'Secondary Support Person' 
		LEFT JOIN (
					SELECT BB.BusFunction,
							BB.Role,
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
							END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName
				) O ON A.BusFunction = O.BusFunction 
					AND O.Role = '2nd Secondary Support Person' 
		LEFT JOIN (
					SELECT BB.BusFunction,
							BB.Role,
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
							END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName
				) P ON A.BusFunction = P.BusFunction 
					AND P.Role = '3rd Secondary Support Person' 
		LEFT JOIN (SELECT BB.BusFunction,
							BB.Role,
							Case BB.Staff 
								WHEN null THEN '' 
								ELSE BBB.FirstName + ' ' + BBB.LastName 
								END as Staff 
					FROM GENR_REF_SupportAssign BB 
					JOIN SYSA_LST_Users BBB ON BB.Staff = BBB.WindowsUserName
				) Q ON A.BusFunction = Q.BusFunction 
					AND Q.Role = '4th Secondary Support Person'
END