#UTLWG18.jcl  Compass/Onelink Enrollment Discrepancies
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG18.LWG18R1
   then
        rm ${reportdir}/ULWG18.LWG18R1
fi
if test -a ${reportdir}/ULWG18.LWG18R2
   then
        rm ${reportdir}/ULWG18.LWG18R2
fi

# run the program

sas ${codedir}/UTLWG18.sas -log ${reportdir}/ULWG18.LWG18R1  -mautosource
