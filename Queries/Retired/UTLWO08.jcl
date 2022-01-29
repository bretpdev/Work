#UTLWO08.jcl  
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO08.LWO08R1
   then
        rm ${reportdir}/ULWO08.LWO08R1
fi
if test -a ${reportdir}/ULWO08.LWO08R2
   then
        rm ${reportdir}/ULWO08.LWO08R2
fi
if test -a ${reportdir}/ULWO08.LWO08R3
   then
        rm ${reportdir}/ULWO08.LWO08R3
fi
if test -a ${reportdir}/ULWO08.LWO08R4
   then
        rm ${reportdir}/ULWO08.LWO08R4
fi
if test -a ${reportdir}/ULWO08.LWO08R5
   then
        rm ${reportdir}/ULWO08.LWO08R5
fi
if test -a ${reportdir}/ULWO08.LWO08R6
   then
        rm ${reportdir}/ULWO08.LWO08R6
fi
if test -a ${reportdir}/ULWO08.LWO08R7
   then
        rm ${reportdir}/ULWO08.LWO08R7
fi
if test -a ${reportdir}/ULWO08.LWO08RZ
   then
        rm ${reportdir}/ULWO08.LWO08RZ
fi

# run the program

sas ${codedir}/UTLWO08.sas -log ${reportdir}/ULWO08.LWO08R1  -mautosource
