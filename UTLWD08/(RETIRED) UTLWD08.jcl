#UTLWD08.jcl  COMPASS SKIP REFS
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWD08.LWD08R1
   then
        rm ${reportdir}/ULWD08.LWD08R1
fi
if test -a ${reportdir}/ULWD08.LWD08R2
   then
        rm ${reportdir}/ULWD08.LWD08R2
fi

# run the program

sas ${codedir}/UTLWD08.sas -log ${reportdir}/ULWD08.LWD08R1  -mautosource
