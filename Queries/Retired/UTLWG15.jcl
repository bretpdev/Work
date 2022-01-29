#UTLWG15.jcl  Duplicate Reference Review
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG15.LWG15R1
   then
        rm ${reportdir}/ULWG15.LWG15R1
fi
if test -a ${reportdir}/ULWG15.LWG15R2
   then
        rm ${reportdir}/ULWG15.LWG15R2
fi

# run the program

sas ${codedir}/UTLWG15.sas -log ${reportdir}/ULWG15.LWG15R1  -mautosource
