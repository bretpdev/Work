#UTLWS17.jcl  
#THE REPORT DIRECTORY FOR THIS JOB WAS HARDCODED PER THE REQUEST OF AES
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWS17.LWS17RZ
   then
        rm ${reportdir}/ULWS17.LWS17RZ
fi
if test -a ${reportdir}/ULWS17.LWS17R1
   then
        rm ${reportdir}/ULWS17.LWS17R1
fi
if test -a ${reportdir}/ULWS17.LWS17R2
   then
        rm ${reportdir}/ULWS17.LWS17R2
fi
if test -a ${reportdir}/ULWS17.LWS17R5
   then
        rm ${reportdir}/ULWS17.LWS17R5
fi
if test -a ${reportdir}/ULWS17.LWS17R6
   then
        rm ${reportdir}/ULWS17.LWS17R6
fi
if test -a ${reportdir}/ULWS17.LWS17R7
   then
        rm ${reportdir}/ULWS17.LWS17R7
fi
if test -a ${reportdir}/ULWS17.LWS17R8
   then
        rm ${reportdir}/ULWS17.LWS17R8
fi
if test -a ${reportdir}/ULWS17.LWS17R9
   then
        rm ${reportdir}/ULWS17.LWS17R9
fi
if test -a ${reportdir}/ULWS17.LWS17R10
   then
        rm ${reportdir}/ULWS17.LWS17R10
fi
if test -a ${reportdir}/ULWS17.LWS17R11
   then
        rm ${reportdir}/ULWS17.LWS17R11
fi
if test -a ${reportdir}/ULWS17.LWS17R12
   then
        rm ${reportdir}/ULWS17.LWS17R12
fi

# run the program

sas ${codedir}/UTLWS17.sas -log ${reportdir}/ULWS17.LWS17R1  -mautosource
