#UTLWF02.jcl  
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWF02.LWF02R1
   then
        rm ${reportdir}/ULWF02.LWF02R1
fi
if test -a ${reportdir}/ULWF02.LWF02R2
   then
        rm ${reportdir}/ULWF02.LWF02R2
fi
if test -a ${reportdir}/ULWF02.LWF02RZ
   then
        rm ${reportdir}/ULWF02.LWF02RZ
fi

# run the program

sas ${codedir}/UTLWF02.sas -log ${reportdir}/ULWF02.LWF02R1  -mautosource
