CREATE PROCEDURE [faq].[QuestionGroupsSelectAll]
AS
	select QuestionGroupId, GroupName
	  from faq.QuestionGroups
	 order by GroupName
RETURN 0