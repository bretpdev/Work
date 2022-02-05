#UTLWU02.jcl  
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWU02.LWU02R1
   then
        rm ${reportdir}/ULWU02.LWU02R1
fi
if test -a ${reportdir}/ULWU02.LWU02R2
   then
        rm ${reportdir}/ULWU02.LWU02R2
fi
if test -a ${reportdir}/ULWU02.LWU02RZ
   then
        rm ${reportdir}/ULWU02.LWU02RZ
fi
# run the program

sas ${codedir}/UTLWU02.sas -log ${reportdir}/ULWU02.LWU02R1  -mautosource
