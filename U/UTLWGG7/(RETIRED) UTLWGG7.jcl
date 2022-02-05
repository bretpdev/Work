#UTLWH01.jcl  
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWGG7.LWGG7R1
   then
        rm ${reportdir}/ULWGG7.LWGG7R1
fi
if test -a ${reportdir}/ULWGG7.LWGG7R2
   then
        rm ${reportdir}/ULWGG7.LWGG7R2
fi
if test -a ${reportdir}/ULWGG7.LWGG7RZ
   then
        rm ${reportdir}/ULWGG7.LWGG7RZ
fi
# run the program

sas ${codedir}/UTLWGG7.sas -log ${reportdir}/ULWGG7.LWGG7R1  -mautosource
