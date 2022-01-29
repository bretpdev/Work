#UTLWG19.jcl  Compass/Onelink Enrollment Discrepancies
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG19.LWG19R1
   then
        rm ${reportdir}/ULWG19.LWG19R1
fi
if test -a ${reportdir}/ULWG19.LWG19R2
   then
        rm ${reportdir}/ULWG19.LWG19R2
fi

# run the program

sas ${codedir}/UTLWG19.sas -log ${reportdir}/ULWG19.LWG19R1  -mautosource
