#UTLWO15.jcl  POTENTIAL CANCEL/REISSUE REPORT
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO15.LWO15R1
   then
        rm ${reportdir}/ULWO15.LWO15R1
fi
if test -a ${reportdir}/ULWO15.LWO15R2
   then
        rm ${reportdir}/ULWO15.LWO15R2
fi

# run the program

sas ${codedir}/UTLWO15.sas -log ${reportdir}/ULWO15.LWO15R1  -mautosource
