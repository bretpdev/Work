#UTLWO95.jcl  PLUS Credit Check Results Monthly
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO95.LWO95R1
   then
        rm ${reportdir}/ULWO95.LWO95R1
fi
if test -a ${reportdir}/ULWO95.LWO95R2
   then
        rm ${reportdir}/ULWO95.LWO95R2
fi
if test -a ${reportdir}/ULWO95.LWO95RZ
   then
        rm ${reportdir}/ULWO95.LWO95RZ
fi

# run the program

sas ${codedir}/UTLWO95.sas -log ${reportdir}/ULWO95.LWO95R1  -mautosource
