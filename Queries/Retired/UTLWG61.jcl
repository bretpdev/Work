#UTLWG61.jcl  pull lists for subrogated loans
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG61.LWG61R1
   then
        rm ${reportdir}/ULWG61.LWG61R1
fi
if test -a ${reportdir}/ULWG61.LWG61R2
   then
        rm ${reportdir}/ULWG61.LWG61R2
fi

# run the program

sas ${codedir}/UTLWG61.sas -log ${reportdir}/ULWG61.LWG61R1  -mautosource
