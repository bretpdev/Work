#UTLWO13.jcl  Compass/Onelink Enrollment Discrepancies
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO13.LWO13R1
   then
        rm ${reportdir}/ULWO13.LWO13R1
fi
if test -a ${reportdir}/ULWO13.LWO13R2
   then
        rm ${reportdir}/ULWO13.LWO13R2
fi

# run the program

sas ${codedir}/UTLWO13.sas -log ${reportdir}/ULWO13.LWO13R1  -mautosource
