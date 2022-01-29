#UTLWK04.jcl  Skip With Other Valid Phone or Address Report
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWK04.LWK04R1
   then
        rm ${reportdir}/ULWK04.LWK04R1
fi
if test -a ${reportdir}/ULWK04.LWK04R2
   then
        rm ${reportdir}/ULWK04.LWK04R2
fi
if test -a ${reportdir}/ULWK04.LWK04RZ
   then
        rm ${reportdir}/ULWK04.LWK04RZ
fi

# run the program

sas ${codedir}/UTLWK04.sas -noterminal -log ${reportdir}/ULWK04.LWK04R1  -mautosource
