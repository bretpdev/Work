#UTLWD04.jcl  FFELP Consolidation Recoveries
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWD04.LWD04R1
   then
        rm ${reportdir}/ULWD04.LWD04R1
fi
if test -a ${reportdir}/ULWD04.LWD04R2
   then
        rm ${reportdir}/ULWD04.LWD04R2
fi

# run the program

sas ${codedir}/UTLWD04.sas -log ${reportdir}/ULWD04.LWD04R1  -mautosource
