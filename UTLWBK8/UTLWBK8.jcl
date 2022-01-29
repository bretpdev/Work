#UTLWBK8.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWBK8.LWBK8R1
   then
        rm ${reportdir}/ULWBK8.LWBK8R1
fi
if test -a ${reportdir}/ULWBK8.LWBK8R2
   then
        rm ${reportdir}/ULWBK8.LWBK8R2
fi
if test -a ${reportdir}/ULWBK8.LWBK8RZ
   then
        rm ${reportdir}/ULWBK8.LWBK8RZ
fi

# run the program

sas ${codedir}/UTLWBK8.sas -log ${reportdir}/ULWBK8.LWBK8R1  -mautosource
