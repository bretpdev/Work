#UTLWK02.jcl  successful directory assistance lookups
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWK02.LWK02R1
   then
        rm ${reportdir}/ULWK02.LWK02R1
fi
if test -a ${reportdir}/ULWK02.LWK02R2
   then
        rm ${reportdir}/ULWK02.LWK02R2
fi
if test -a ${reportdir}/ULWK02.LWK02RZ
   then
        rm ${reportdir}/ULWK02.LWK02RZ
fi
# run the program

sas ${codedir}/UTLWK02.sas -log ${reportdir}/ULWK02.LWK02R1  -mautosource
