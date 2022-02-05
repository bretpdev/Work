




CREATE PROCEDURE [dbo].[spRSgetXMLClassReport] 

AS


SELECT TOP 6  Title  AS 'English courses (4 credits)', COALESCE(GradeLevel,'') AS 'Grade Level', COALESCE(Credit,'') AS 'Credit', COALESCE(G1,'') AS ' ', COALESCE(G2,'') AS '  ', COALESCE(G3,'') AS '   ', COALESCE(G4,'') AS '    ', COALESCE(G5,'') AS '     ', COALESCE(G6,'') AS '      ', COALESCE(Weighted,'') AS 'Weighted GPA and Weighted Grade'
FROM         bsys.dbo.RSClassReport
WHERE StudentNum = 255 AND ClassType = 1
FOR XML AUTO, ELEMENTS
--WHERE     Ticket = '5' FOR XML AUTO

SELECT  TOP 6 Title AS 'Math courses (4 credits)', COALESCE(GradeLevel,'') AS 'Grade Level ', COALESCE(Credit,'') AS 'Credit ', COALESCE(G1,'') AS '       ', COALESCE(G2,'') AS '        ', COALESCE(G3,'') AS '         ', COALESCE(G4,'') AS '          ', COALESCE(G5,'') AS '           ', COALESCE(G6,'') AS '            ', COALESCE(Weighted,'') AS 'Weighted GPA and Weighted Grade '
FROM         bsys.dbo.RSClassReport
WHERE StudentNum = 255 AND ClassType = 2
FOR XML AUTO, ELEMENTS
--WHERE     Ticket = '5'FOR XML AUTO, ELEMENTS

SELECT     TOP 8 Title AS 'Science courses (3 credits)', COALESCE(GradeLevel,'') AS 'Grade Level  ', COALESCE(Credit,'') AS 'Credit  ', COALESCE(G1,'') AS '             ', COALESCE(G2,'') AS '              ', COALESCE(G3,'') AS '               ', COALESCE(G4,'') AS '                ', COALESCE(G5,'') AS '                 ', COALESCE(G6,'') AS '                  ', COALESCE(Weighted,'') AS 'Weighted GPA and Weighted Grade  '
FROM         bsys.dbo.RSClassReport
WHERE StudentNum = 255 AND ClassType = 3
FOR XML AUTO, ELEMENTS
--WHERE     Ticket = '5'FOR XML AUTO, ELEMENTS

SELECT     TOP 8 Title AS 'Social Sicence courses (3.5 credits)', COALESCE(GradeLevel,'') AS 'Grade Level   ', COALESCE(Credit,'') AS 'Credit   ', COALESCE(G1,'') AS '                   ', COALESCE(G2,'') AS '                    ', COALESCE(G3,'') AS '                     ', COALESCE(G4,'') AS '                      ', COALESCE(G5,'') AS '                       ', COALESCE(G6,'') AS '                        ', COALESCE(Weighted,'') AS 'Weighted GPA and Weighted Grade   '
FROM         bsys.dbo.RSClassReport
WHERE StudentNum = 255 AND ClassType = 4
FOR XML AUTO, ELEMENTS
--WHERE     Ticket = '5'FOR XML AUTO, ELEMENTS

SELECT     TOP 5 Title AS 'Foreign Language courses (2 credits)', COALESCE(GradeLevel,'') AS 'Grade Level    ', COALESCE(Credit,'') AS 'Credit    ', COALESCE(G1,'') AS '                         ', COALESCE(G2,'') AS '                          ', COALESCE(G3,'') AS '                           ', COALESCE(G4,'') AS '                            ', COALESCE(G5,'') AS '                             ', COALESCE(G6,'') AS '                              ', COALESCE(Weighted,'') AS 'Weighted GPA and Weighted Grade    '
FROM         bsys.dbo.RSClassReport
WHERE StudentNum = 255 AND ClassType = 5
FOR XML AUTO, ELEMENTS
--WHERE     Ticket = '5' FOR XML AUTO, ELEMENTS