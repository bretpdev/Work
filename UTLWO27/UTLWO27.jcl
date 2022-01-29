#UTLWO27.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO27.LWO27R1
   then
        rm ${reportdir}/ULWO27.LWO27R1
fi
if test -a ${reportdir}/ULWO27.LWO27R2
   then
        rm ${reportdir}/ULWO27.LWO27R2
fi
if test -a ${reportdir}/ULWO27.LWO27R3
   then
        rm ${reportdir}/ULWO27.LWO27R3
fi
if test -a ${reportdir}/ULWO27.LWO27RZ
   then
        rm ${reportdir}/ULWO27.LWO27RZ
fi

# run the program

sas ${codedir}/UTLWO27.sas -log ${reportdir}/ULWO27.LWO27R1  -mautosource
