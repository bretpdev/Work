#UTLWD17.jcl  Compass/Onelink Enrollment Discrepancies
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWD17.LWD17R1
   then
        rm ${reportdir}/ULWD17.LWD17R1
fi
if test -a ${reportdir}/ULWD17.LWD17R2
   then
        rm ${reportdir}/ULWD17.LWD17R2
fi

# run the program

sas ${codedir}/UTLWD17.sas -log ${reportdir}/ULWD17.LWD17R1  -mautosource
