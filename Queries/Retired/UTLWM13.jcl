#UTLWM13.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWM13.LWM13R1
   then
        rm ${reportdir}/ULWM13.LWM13R1
fi
if test -a ${reportdir}/ULWM13.LWM13R2
   then
        rm ${reportdir}/ULWM13.LWM13R2
fi
if test -a /sas/whse/progrevw/rcvfile9.raw
   then
        rm /sas/whse/progrevw/rcvfile9.raw
fi

# run the program

sas ${codedir}/UTLWM13.sas -log ${reportdir}/ULWM13.LWM13R1  -mautosource
