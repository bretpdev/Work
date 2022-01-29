#UTLWG02.jcl  pull lists for subrogated loans
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG02.LWG02R1
   then
        rm ${reportdir}/ULWG02.LWG02R1
fi
if test -a ${reportdir}/ULWG02.LWG02R2
   then
        rm ${reportdir}/ULWG02.LWG02R2
fi

# run the program

sas ${codedir}/UTLWG02.sas -log ${reportdir}/ULWG02.LWG02R1  -mautosource
