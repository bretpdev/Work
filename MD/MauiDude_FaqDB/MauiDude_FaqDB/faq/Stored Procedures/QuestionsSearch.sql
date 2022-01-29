CREATE PROCEDURE [faq].[QuestionsSearch]
	@QuestionGroupId int = null,
	@PortfolioIds faq.PortfolioIds readonly,
	@IncludeQuestionsWithoutPortfolios bit,
	@SearchTerm nvarchar(max) = null
AS
	select distinct q.QuestionId, q.QuestionGroupId, q.Question, q.Answer, q.LastUpdatedOn, q.LastUpdatedBy, qg.GroupName as QuestionGroupName
	  from faq.Questions q
	  join faq.QuestionGroups qg on q.QuestionGroupId = qg.QuestionGroupId
	  left join faq.QuestionPortfolios qp on q.QuestionId = qp.QuestionId
	 where (@IncludeQuestionsWithoutPortfolios = 1 or qp.PortfolioId in (select PortfolioId from @PortfolioIds))
	   and (@QuestionGroupId is null or q.QuestionGroupId = @QuestionGroupId)
	   and (@SearchTerm is null or q.Question like '%' + @SearchTerm + '%' or q.Answer like '%' + @SearchTerm + '%')
RETURN 0