#UTLWG16.jcl  Compass/Onelink Enrollment Discrepancies
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG16.LWG16R1
   then
        rm ${reportdir}/ULWG16.LWG16R1
fi
if test -a ${reportdir}/ULWG16.LWG16R2
   then
        rm ${reportdir}/ULWG16.LWG16R2
fi
if test -a ${reportdir}/ULWG16.LWG16RZ
   then
        rm ${reportdir}/ULWG16.LWG16RZ
fi

# run the program

sas ${codedir}/UTLWG16.sas -log ${reportdir}/ULWG16.LWG16R1  -mautosource
