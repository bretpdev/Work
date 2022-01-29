#UTLWO26.jcl  two-step consolidation rate descrepancies
#
#set environment variables

 . /uheaa/whse/copy_lib/sasprofile
 . /uheaa/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO26.LWO26R1
   then
        rm ${reportdir}/ULWO26.LWO26R1
fi
if test -a ${reportdir}/ULWO26.LWO26R2
   then
        rm ${reportdir}/ULWO26.LWO26R2
fi
if test -a ${reportdir}/ULWO26.LWO26RZ
   then
        rm ${reportdir}/ULWO26.LWO26RZ
fi

# run the program

sas ${codedir}/UTLWO26.sas -log ${reportdir}/ULWO26.LWO26R1  -mautosource
