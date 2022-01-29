use cls
go

select distinct	
	LetterType,
	LetterOptions,
	LetterChoices,
	LetterID,
	Arc
from 
	cls.cslsltrfed.LoanServicingLetters