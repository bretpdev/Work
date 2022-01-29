#UTLWL06.jcl  LPP Small Balance Report
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWL06.LWL06R1
   then
        rm ${reportdir}/ULWL06.LWL06R1
fi
if test -a ${reportdir}/ULWL06.LWL06R2
   then
        rm ${reportdir}/ULWL06.LWL06R2
fi
if test -a ${reportdir}/ULWL06.LWL06R3
   then
        rm ${reportdir}/ULWL06.LWL06R3
fi
if test -a ${reportdir}/ULWL06.LWL06R4
   then
        rm ${reportdir}/ULWL06.LWL06R4
fi
if test -a ${reportdir}/ULWL06.LWL06RZ
   then
        rm ${reportdir}/ULWL06.LWL06RZ
fi

# run the program

sas ${codedir}/UTLWL06.sas -log ${reportdir}/ULWL06.LWL06R1  -mautosource
