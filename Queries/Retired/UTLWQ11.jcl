#UTLWQ11.jcl  Compass/Onelink Enrollment Discrepancies
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWQ11.LWQ11R1
   then
        rm ${reportdir}/ULWQ11.LWQ11R1
fi
if test -a ${reportdir}/ULWQ11.LWQ11R2
   then
        rm ${reportdir}/ULWQ11.LWQ11R2
fi

# run the program

sas ${codedir}/UTLWQ11.sas -log ${reportdir}/ULWQ11.LWQ11R1  -mautosource
