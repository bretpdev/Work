#UTLWD07.jcl  Duplicate Reference Review
#set environment variables

 . /uheaa/whse/copy_lib/sasprofile
 . /uheaa/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWD07.LWD07R1
   then
        rm ${reportdir}/ULWD07.LWD07R1
fi
if test -a ${reportdir}/ULWD07.LWD07R2
   then
        rm ${reportdir}/ULWD07.LWD07R2
fi
if test -a ${reportdir}/ULWD07.LWD07R3
   then
        rm ${reportdir}/ULWD07.LWD07R3
fi

# run the program

sas ${codedir}/UTLWD07.sas -log ${reportdir}/ULWD07.LWD07R1  -mautosource
