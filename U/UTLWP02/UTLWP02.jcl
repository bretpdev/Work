#UTLWP02.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWP02.LWP02R1
   then
        rm ${reportdir}/ULWP02.LWP02R1
fi
if test -a ${reportdir}/ULWP02.LWP02R2
   then
        rm ${reportdir}/ULWP02.LWP02R2
fi
if test -a ${reportdir}/ULWP02.LWP02RZ
   then
        rm ${reportdir}/ULWP02.LWP02RZ
fi

# run the program

sas ${codedir}/UTLWP02.sas -log ${reportdir}/ULWP02.LWP02R1  -mautosource
