#UTLWD21.jcl  Auxiliary Services Cancelled Queue Statistic
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWD21.LWD21R1
   then
        rm ${reportdir}/ULWD21.LWD21R1
fi
if test -a ${reportdir}/ULWD21.LWD21R2
   then
        rm ${reportdir}/ULWD21.LWD21R2
fi

# run the program

sas ${codedir}/UTLWD21.sas -log ${reportdir}/ULWD21.LWD21R1  -mautosource
