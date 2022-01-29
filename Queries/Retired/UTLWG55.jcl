#UTLWG55.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG55.LWG55R1
   then
        rm ${reportdir}/ULWG55.LWG55R1
fi
if test -a ${reportdir}/ULWG55.LWG55R2
   then
        rm ${reportdir}/ULWG55.LWG55R2
fi

# run the program

sas ${codedir}/UTLWG55.sas -log ${reportdir}/ULWG55.LWG55R1  -mautosource
