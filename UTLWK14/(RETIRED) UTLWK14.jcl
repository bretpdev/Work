#UTLWK14.jcl  PLUS Preapproval by School
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWK14.LWK14R1
   then
        rm ${reportdir}/ULWK14.LWK14R1
fi
if test -a ${reportdir}/ULWK14.LWK14R2
   then
        rm ${reportdir}/ULWK14.LWK14R2
fi
if test -a ${reportdir}/ULWK14.LWK14RZ
   then
        rm ${reportdir}/ULWK14.LWK14RZ
fi

# run the program

sas ${codedir}/UTLWK14.sas -log ${reportdir}/ULWK14.LWK14R1  -mautosource
