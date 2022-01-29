#UTLWE09.jcl  LPP SERVICING DATA FILE FOR FINANCE - MR52
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWE09.LWE09R1
   then
        rm ${reportdir}/ULWE09.LWE09R1
fi
if test -a ${reportdir}/ULWE09.LWE09R2
   then
        rm ${reportdir}/ULWE09.LWE09R2
fi

# run the program

sas ${codedir}/UTLWE09.sas -log ${reportdir}/ULWE09.LWE09R1  -mautosource
