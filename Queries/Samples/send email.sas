FILENAME ERRMESS EMAIL to=("nowens@utahsbr.edu") cc=("jryan@utahsbr.edu");

		DATA _NULL_;
		FILE ERRMESS;
			PUT "This is a test email to make sure we can!";
		RUN;
