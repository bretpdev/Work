#UTLWD23.jcl Collections Daily Activites
#
#set environment variables

 . /uheaa/whse/copy_lib/sasprofile
 . /uheaa/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWD23.LWD23R1
   then
        rm ${reportdir}/ULWD23.LWD23R1
fi
if test -a ${reportdir}/ULWD23.LWD23R2
   then
        rm ${reportdir}/ULWD23.LWD23R2
fi
if test -a ${reportdir}/ULWD23.LWD23R3
   then
        rm ${reportdir}/ULWD23.LWD23R3
fi
if test -a ${reportdir}/ULWD23.LWD23R4
   then
        rm ${reportdir}/ULWD23.LWD23R4
fi
if test -a ${reportdir}/ULWD23.LWD23R5
   then
        rm ${reportdir}/ULWD23.LWD23R5
fi

# run the program

sas ${codedir}/UTLWD23.sas -log ${reportdir}/ULWD23.LWD23R1  -mautosource
