#UTLWS10.jcl  Borrower Services Cancelled Queue Statistic
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWS10.LWS10R1
   then
        rm ${reportdir}/ULWS10.LWS10R1
fi
if test -a ${reportdir}/ULWS10.LWS10R2
   then
        rm ${reportdir}/ULWS10.LWS10R2
fi

# run the program

sas ${codedir}/UTLWS10.sas -log ${reportdir}/ULWS10.LWS10R1  -mautosource
