#UTLWE14.jcl  MONTHLY WELLS FARGO MPN DATA FILE
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWE14.LWE14R1
   then
        rm ${reportdir}/ULWE14.LWE14R1
fi
if test -a ${reportdir}/ULWE14.LWE14R2
   then
        rm ${reportdir}/ULWE14.LWE14R2
fi

# run the program

sas ${codedir}/UTLWE14.sas -log ${reportdir}/ULWE14.LWE14R1  -mautosource
