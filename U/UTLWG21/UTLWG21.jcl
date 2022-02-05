#UTLWG21.jcl  FFY DAAR Request Totals
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG21.LWG21R1
   then
        rm ${reportdir}/ULWG21.LWG21R1
fi
if test -a ${reportdir}/ULWG21.LWG21R2
   then
        rm ${reportdir}/ULWG21.LWG21R2
fi
if test -a ${reportdir}/ULWG21.LWG21R3
   then
        rm ${reportdir}/ULWG21.LWG21R3
fi
if test -a ${reportdir}/ULWG21.LWG21R4
   then
        rm ${reportdir}/ULWG21.LWG21R4
fi
if test -a ${reportdir}/ULWG21.LWG21R5
   then
        rm ${reportdir}/ULWG21.LWG21R5
fi
if test -a ${reportdir}/ULWG21.LWG21R6
   then
        rm ${reportdir}/ULWG21.LWG21R6
fi
if test -a ${reportdir}/ULWG21.LWG21R7
   then
        rm ${reportdir}/ULWG21.LWG21R7
fi

# run the program

sas ${codedir}/UTLWG21.sas -log ${reportdir}/ULWG21.LWG21R1  -mautosource
