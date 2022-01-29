#UTLWD16.jcl  Duplicate Reference Review
#set environment variables

 . /uheaa/whse/copy_lib/sasprofile
 . /uheaa/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWD16.LWD16R1
   then
        rm ${reportdir}/ULWD16.LWD16R1
fi
if test -a ${reportdir}/ULWD16.LWD16R2
   then
        rm ${reportdir}/ULWD16.LWD16R2
fi
if test -a ${reportdir}/ULWD16.LWD16R3
   then
        rm ${reportdir}/ULWD16.LWD16R3
fi

# run the program

sas ${codedir}/UTLWD16.sas -log ${reportdir}/ULWD16.LWD16R1  -mautosource
