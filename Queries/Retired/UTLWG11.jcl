#UTLWG11.jcl  Outstanding Loan Applications
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG11.LWG11R1
   then
        rm ${reportdir}/ULWG11.LWG11R1
fi
if test -a ${reportdir}/ULWG11.LWG11R2
   then
        rm ${reportdir}/ULWG11.LWG11R2
fi

# run the program

sas ${codedir}/UTLWG11.sas -log ${reportdir}/ULWG11.LWG11R1  -mautosource
