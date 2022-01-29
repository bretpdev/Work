CREATE TABLE [dbo].[QSTA_LST_Departments] (
	DepartmentId int not null IDENTITY,
    [DeptCode] varchar(3) NOT NULL UNIQUE,
    CONSTRAINT [PK_QSTA_LST_Departments] PRIMARY KEY CLUSTERED ([DepartmentId]), 
    CONSTRAINT [AK_QSTA_LST_Departments_DeptCode] UNIQUE (DeptCode)
);

