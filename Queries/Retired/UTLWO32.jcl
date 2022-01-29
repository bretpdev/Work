#UTLWO32.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO32.LWO32R1
   then
        rm ${reportdir}/ULWO32.LWO32R1
fi
if test -a ${reportdir}/ULWO32.LWO32R2
   then
        rm ${reportdir}/ULWO32.LWO32R2
fi
if test -a ${reportdir}/ULWO32.LWO32R3
   then
        rm ${reportdir}/ULWO32.LWO32R3
fi
if test -a ${reportdir}/ULWO32.LWO32R4
   then
        rm ${reportdir}/ULWO32.LWO32R4
fi
if test -a ${reportdir}/ULWO32.LWO32R5
   then
        rm ${reportdir}/ULWO32.LWO32R5
fi
if test -a ${reportdir}/ULWO32.LWO32RZ
   then
        rm ${reportdir}/ULWO32.LWO32RZ
fi

# run the program

sas ${codedir}/UTLWO32.sas -log ${reportdir}/ULWO32.LWO32R1  -mautosource
