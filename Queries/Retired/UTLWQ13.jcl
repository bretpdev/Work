#UTLWQ13.jcl  Anticipated disbursements at foreign schools
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWQ13.LWQ13R1
   then
        rm ${reportdir}/ULWQ13.LWQ13R1
fi
if test -a ${reportdir}/ULWQ13.LWQ13R2
   then
        rm ${reportdir}/ULWQ13.LWQ13R2
fi
if test -a ${reportdir}/ULWQ13.LWQ13RZ
   then
        rm ${reportdir}/ULWQ13.LWQ13RZ
fi

# run the program

sas ${codedir}/UTLWQ13.sas -log ${reportdir}/ULWQ13.LWQ13R1  -mautosource
