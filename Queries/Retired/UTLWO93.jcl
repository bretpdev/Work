#UTLWO93.jcl  Credit Union Membership Followup
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO93.LWO93R1
   then
        rm ${reportdir}/ULWO93.LWO93R1
fi
if test -a ${reportdir}/ULWO93.LWO93R2
   then
        rm ${reportdir}/ULWO93.LWO93R2
fi
if test -a ${reportdir}/ULWO93.LWO93R3
   then
        rm ${reportdir}/ULWO93.LWO93R3
fi
if test -a ${reportdir}/ULWO93.LWO93R4
   then
        rm ${reportdir}/ULWO93.LWO93R4
fi
if test -a ${reportdir}/ULWO93.LWO93R5
   then
        rm ${reportdir}/ULWO93.LWO93R5
fi

# run the program

sas ${codedir}/UTLWO93.sas -log ${reportdir}/ULWO93.LWO93R1  -mautosource
