#UTLWG01.jcl  DAILY GUARANTEES
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG01.LWG01R1
   then
        rm ${reportdir}/ULWG01.LWG01R1
fi
if test -a ${reportdir}/ULWG01.LWG01R2
   then
        rm ${reportdir}/ULWG01.LWG01R2
fi
if test -a ${reportdir}/ULWG01.LWG01R3
   then
        rm ${reportdir}/ULWG01.LWG01R3
fi
if test -a ${reportdir}/ULWG01.LWG01R4
   then
        rm ${reportdir}/ULWG01.LWG01R4
fi
if test -a ${reportdir}/ULWG01.LWG01R5
   then
        rm ${reportdir}/ULWG01.LWG01R5
fi
if test -a ${reportdir}/ULWG01.LWG01R6
   then
        rm ${reportdir}/ULWG01.LWG01R6
fi

# run the program

sas ${codedir}/UTLWG01.sas -log ${reportdir}/ULWG01.LWG01R1  -mautosource
