#UTLWG06.jcl  pull lists for subrogated loans
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG06.LWG06R1
   then
        rm ${reportdir}/ULWG06.LWG06R1
fi
if test -a ${reportdir}/ULWG06.LWG06R2
   then
        rm ${reportdir}/ULWG06.LWG06R2
fi

# run the program

sas ${codedir}/UTLWG06.sas -log ${reportdir}/ULWG06.LWG06R1  -mautosource
