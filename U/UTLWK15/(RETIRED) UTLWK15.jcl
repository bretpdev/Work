#UTLWK15.jcl  PLUS Preapproval by School
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWK15.LWK15R1
   then
        rm ${reportdir}/ULWK15.LWK15R1
fi
if test -a ${reportdir}/ULWK15.LWK15R2
   then
        rm ${reportdir}/ULWK15.LWK15R2
fi
if test -a ${reportdir}/ULWK15.LWK15RZ
   then
        rm ${reportdir}/ULWK15.LWK15RZ
fi

# run the program

sas ${codedir}/UTLWK15.sas -log ${reportdir}/ULWK15.LWK15R1  -mautosource
