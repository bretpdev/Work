SELECT * FROM CDW.FsaInvMet.RX WHERE ReportDate > 'XXXX-XX-XX'


select COUNT(DISTINCT BF_SSN), PerformanceCategory from cdw.FsaInvMet.Monthly_BorrowerLevel GROUP BY PerformanceCategory