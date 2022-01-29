#UTLWG54.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG54.LWG54R1
   then
        rm ${reportdir}/ULWG54.LWG54R1
fi
if test -a ${reportdir}/ULWG54.LWG54R2
   then
        rm ${reportdir}/ULWG54.LWG54R2
fi

# run the program

sas ${codedir}/UTLWG54.sas -log ${reportdir}/ULWG54.LWG54R1  -mautosource
