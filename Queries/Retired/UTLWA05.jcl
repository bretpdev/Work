#UTLWA05.jcl  quarterly origination fee calculation
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWA05.LWA05R1
   then
        rm ${reportdir}/ULWA05.LWA05R1
fi
if test -a ${reportdir}/ULWA05.LWA05R2
   then
        rm ${reportdir}/ULWA05.LWA05R2
fi

# run the program

sas ${codedir}/UTLWA05.sas -log ${reportdir}/ULWA05.LWA05R1  -mautosource
