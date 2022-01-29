#UTLWO31.jcl  
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO31.LWO31RZ
   then
        rm ${reportdir}/ULWO31.LWO31RZ
fi
if test -a ${reportdir}/ULWO31.LWO31R1
   then
        rm ${reportdir}/ULWO31.LWO31R1
fi
if test -a ${reportdir}/ULWO31.LWO31R2
   then
        rm ${reportdir}/ULWO31.LWO31R2
fi

# run the program

sas ${codedir}/UTLWO31.sas -log ${reportdir}/ULWO31.LWO31R1  -mautosource
