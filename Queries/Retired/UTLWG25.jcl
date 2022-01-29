#UTLWG25.jcl  FFY DAAR Request Totals
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG25.LWG25R1
   then
        rm ${reportdir}/ULWG25.LWG25R1
fi
if test -a ${reportdir}/ULWG25.LWG25R2
   then
        rm ${reportdir}/ULWG25.LWG25R2
fi
if test -a ${reportdir}/ULWG25.LWG25R3
   then
        rm ${reportdir}/ULWG25.LWG25R3
fi

# run the program

sas ${codedir}/UTLWG25.sas -log ${reportdir}/ULWG25.LWG25R1  -mautosource
