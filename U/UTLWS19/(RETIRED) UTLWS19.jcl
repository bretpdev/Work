#UTLWS19.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWS19.LWS19R1
   then
        rm ${reportdir}/ULWS19.LWS19R1
fi
if test -a ${reportdir}/ULWS19.LWS19R2
   then
        rm ${reportdir}/ULWS19.LWS19R2
fi
if test -a ${reportdir}/ULWS19.LWS19R3
   then
        rm ${reportdir}/ULWS19.LWS19R3
fi
if test -a ${reportdir}/ULWS19.LWS19RZ
   then
        rm ${reportdir}/ULWS19.LWS19RZ
fi

# run the program

sas ${codedir}/UTLWS19.sas -log ${reportdir}/ULWS19.LWS19R1  -mautosource
