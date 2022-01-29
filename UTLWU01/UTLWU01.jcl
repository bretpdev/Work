#UTLWU01.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWU01.LWU01R1
   then
        rm ${reportdir}/ULWU01.LWU01R1
fi
if test -a ${reportdir}/ULWU01.LWU01R2
   then
        rm ${reportdir}/ULWU01.LWU01R2
fi
if test -a ${reportdir}/ULWU01.LWU01RZ
   then
        rm ${reportdir}/ULWU01.LWU01RZ
fi

# run the program

sas ${codedir}/UTLWU01.sas -log ${reportdir}/ULWU01.LWU01R1  -mautosource
