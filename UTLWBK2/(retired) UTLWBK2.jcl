#UTLWBK2.jcl  Open Chapter 7 Bankruptcies
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWBK2.LWBK2R1
   then
        rm ${reportdir}/ULWBK2.LWBK2R1
fi
if test -a ${reportdir}/ULWBK2.LWBK2R2
   then
        rm ${reportdir}/ULWBK2.LWBK2R2
fi
if test -a ${reportdir}/ULWBK2.LWBK2RZ
   then
        rm ${reportdir}/ULWBK2.LWBK2RZ
fi

# run the program

sas ${codedir}/UTLWBK2.sas -log ${reportdir}/ULWBK2.LWBK2R1  -mautosource
